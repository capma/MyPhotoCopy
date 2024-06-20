using MetadataExtractor;
using MetadataExtractor.Formats.Avi;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.Formats.QuickTime;
using Newtonsoft.Json;
using PhotoMove.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PhotoMove.Models
{
    public class ScanFile
    {
        public string filePath { get; set; }
        public string? fileName { get; set; } = string.Empty;
        public string? fileExtension { get; set; } = string.Empty;
        public DateTime takenDate { get; set; }
        public string? cameraModel { get; set; } = string.Empty;
        public string? cameraMake { get; set; } = string.Empty;
        public bool isValidExif { get; set; } = false;
        public bool isValidTakenDate { get; set; } = false;
        public bool isTransferred { get; set; } = false;
        public bool isProcessed { get; set; } = false;
        public string destination { get; set; }
        public string action { get; set; }
        public DateTime fileDate { get; set; }

        private IReadOnlyList<MetadataExtractor.Directory> directories { get; set; }

        public ScanFile()
        {
            
        }

        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="file"></param>
        public ScanFile(string file)
        {
            filePath = file;
            fileName = Path.GetFileName(file);
            fileExtension = Path.GetExtension(file);
        }

        public void ReadFile(string filePath)
        {
            try
            {
                directories = ImageMetadataReader.ReadMetadata(filePath);

                var fileType = GetFileType(filePath);

                switch (fileType)
                {
                    case FileType.Photo:
                    case FileType.Movie:
                        ExtractMetadataFromPhotoFile(filePath);
                        break;
                    //case FileType.Movie:
                    //    ExtractMetadataFromMovieFile(filePath);
                    //    break;
                    case FileType.Other:
                        TryExtractCreateDateFromFile(filePath);
                        break;
                }
            }
            catch (Exception)
            {
                TryExtractCreateDateFromFile(filePath);
            }
        }

        public DateTime? GetValidDateTime(string input)
        {
            DateTime temp;
            DateTime temp2;
            string[] formats =
            {
                "dd/MM/yyyy HH:mm:ss", "dd-MM-yyyy HH:mm:ss", "dd.MM.yyyy HH:mm:ss", "dd MM yyyy HH:mm:ss", // DMY formats
                "yyyy/MM/dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss", "yyyy.MM.dd HH:mm:ss", "yyyy MM dd HH:mm:ss", // YMD formats
                "MM/dd/yyyy HH:mm:ss", "MM-dd-yyyy HH:mm:ss", "MM.dd.yyyy HH:mm:ss", "MM dd yyyy HH:mm:ss", // MDY formats
                "yyyy:MM:dd HH:mm:ss", "yyyy-MM-ddTHH:mm:ss.fffzzz", "yyyy:MM:dd HH:mm:sszzz" // ISO 8601 format
            };

            if (DateTime.TryParseExact(input, formats, null, System.Globalization.DateTimeStyles.None, out temp))
                //if (DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
                return temp;
            else if (DateTime.TryParse(input, out temp2))
                return temp2;
            else
                return null;
        }

        private FileType GetFileType(string filePath)
        {
            try
            {
                bool isPhoto = directories.Any(x => (x is ExifIfd0Directory
                                                    || x is ExifSubIfdDirectory
                                                    || x is JpegDirectory)
                            && (x.ContainsTag(ExifDirectoryBase.TagModel)
                                || x.ContainsTag(ExifDirectoryBase.TagMake)
                                || x.ContainsTag(ExifDirectoryBase.TagDateTimeOriginal)));
                if (isPhoto)
                {
                    return FileType.Photo;
                }

                bool isMovie = directories.Any(x => x is QuickTimeFileTypeDirectory
                                                || x is QuickTimeMetadataHeaderDirectory
                                                || x is QuickTimeMovieHeaderDirectory
                                                || x is AviDirectory);
                if (isMovie)
                {
                    return FileType.Movie;
                }
            }
            catch (Exception)
            {
                // Fallback to other file type detection methods
            }

            return FileType.Other;
        }

        private void ExtractMetadataFromPhotoFile(string filePath)
        {
            string originalTakenDateString = string.Empty;
            bool isFound = false;

            foreach (var directory in directories)
            {
                if (isFound)
                    break;

                foreach (var tag in directory.Tags)
                {
                    cameraModel = (tag.Type == ExifDirectoryBase.TagModel) ? tag.Description + "" : cameraModel;
                    cameraMake = (tag.Type == ExifDirectoryBase.TagMake) ? tag.Description + "" : cameraMake;
                    originalTakenDateString = (tag.Type == ExifDirectoryBase.TagDateTimeOriginal) ? tag.Description + "" : originalTakenDateString;

                    if (!string.IsNullOrEmpty(cameraMake)
                        && !string.IsNullOrEmpty(cameraMake)
                        && !string.IsNullOrEmpty(originalTakenDateString)
                        )
                    {
                        isFound = true;
                        break;
                    }
                }
            }

            //isValidExif = (!string.IsNullOrEmpty(cameraModel) && !string.IsNullOrEmpty(cameraMake));
            isValidExif = true;

            DateTime? orginalDate = GetValidDateTime(originalTakenDateString);
            if (orginalDate != null)
            {
                takenDate = (DateTime)(orginalDate);
                isValidTakenDate = true;
            }
        }

        private void ExtractMetadataFromMovieFile(string filePath)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = @"exiftool.exe",
                        Arguments = $"-Make -Model -CreateDate -json \"{filePath}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // Parse the output and add it to the list
                var metadata = JsonConvert.DeserializeObject<List<FileMetadata>>(output);

                cameraModel = metadata?.FirstOrDefault()?.Model;
                cameraMake = metadata?.FirstOrDefault()?.Make;
                isValidExif = true;

                string originalTakenDateString = metadata?.FirstOrDefault()?.CreateDate + "";
                DateTime? orginalDate = GetValidDateTime(originalTakenDateString);
                if (orginalDate != null)
                {
                    takenDate = (DateTime)(orginalDate);
                    isValidTakenDate = true;
                }
            }
            catch (Exception)
            {
                isValidExif = false;
                isValidTakenDate = false;
                TryExtractCreateDateFromFile(filePath);
            }
        }

        private void TryExtractCreateDateFromFile(string filePath)
        {
            try
            {
                // Use alternative methods to extract metadata for normal files
                // For example, you can try to retrieve the creation date using the .NET file class:
                var creationTime = File.GetCreationTime(filePath);
                if (creationTime != default)
                {
                    //takenDate = creationTime;
                    fileDate = creationTime;
                }
            }
            catch (Exception)
            {
                // silent
            }
        }
    }
}
