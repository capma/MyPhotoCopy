using MetadataExtractor;
using MetadataExtractor.Formats.Avi;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.Formats.QuickTime;
using PhotoMove.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhotoMove.Models
{
    public class ScanFile
    {
        public enum FileType
        {
            Movie,
            Photo,
            Other
        }

        public string filePath { get; set; }
        public string? fileName { get; set; } = string.Empty;
        public string? fileExtension { get; set; } = string.Empty;
        public DateTime takenDate { get; set; }
        public string? cameraModel { get; set; } = string.Empty;
        public string? cameraMake { get; set; } = string.Empty;
        public bool isValidExif { get; set; } = false;
        public bool isValidTakenDate { get; set; } = false;
        public bool isCopiedOrMoved { get; set; } = false;
        public List<MyTag> tags { get; set; }
        public List<string> errors { get; set; }

        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="file"></param>
        public ScanFile(string file)
        {
            tags = new List<MyTag>();
            errors = new List<string>();

            filePath = file;
            fileName = Path.GetFileName(file);
            fileExtension = Path.GetExtension(file);

            //isValidExif = true;
            //isValidTakenDate = true;
        }

        public void ReadFile(string filePath)
        {
            //var fileMetadata = new ScanFile(filePath);

            try
            {
                var fileType = GetFileType(filePath);

                switch (fileType)
                {
                    case FileType.Photo:
                        ExtractMetadataFromMultimediaFile(filePath);
                        break;
                    case FileType.Movie:
                    case FileType.Other:
                        ExtractMetadataFromNormalFile(filePath);
                        break;
                }
            }
            catch (Exception)
            {
                // Fallback to other metadata extraction methods
            }

            //return fileMetadata;
        }

        public void ReadExifData()
        {
            try
            {
                var directories = ImageMetadataReader.ReadMetadata(filePath);

                foreach (var directory in directories)
                {
                    foreach (var tag in directory.Tags)
                    {
                        if (tag.HasName && 
                                (
                                    tag.Name.Contains(Exif.ExifTag) 
                                        || tag.Name.Contains(Exif.TakenDate)
                                        || tag.Name.Contains(Exif.CameraMakeTag)
                                        || tag.Name.Contains(Exif.CameraModelTag)
                                )
                            )
                        {
                            MyTag myTag = new();
                            myTag.DirectoryName = directory.Name;
                            myTag.TagName = tag.Name;
                            myTag.Description = tag?.Description;

                            tags.Add(myTag);
                        }
                    }

                    if (directory.HasError)
                    {
                        foreach (var error in directory.Errors)
                        {
                            errors.Add($"ERROR: {error}");
                        }
                    }
                }

                if (tags.Count == 0)
                {
                    isValidExif = false;
                    isValidTakenDate = false;
                }

                if (tags.Count > 0)
                {
                    string? takenDateString = GetOriginalTakenDate();
                    if (takenDateString != null) 
                    {
                        DateTime? orginalDate = GetValidDateTime(takenDateString);
                        if (orginalDate != null)
                        {
                            takenDate = (DateTime)orginalDate;
                        }
                        else
                        {
                            isValidTakenDate = false;
                        }
                    }
                    else
                    {
                        isValidTakenDate = false;
                    }

                    cameraModel = GetCameraModel();
                    cameraMake = GetCameraMake();
                }
                
            }
            catch (Exception)
            {
                isValidExif = false;
                isValidTakenDate = false;
            }
        }

        private string? GetOriginalTakenDate()
        {
            var originDate = tags.Where(x => x.DirectoryName.Contains(Exif.ExifTag) && x.TagName.Contains(Exif.TakenDate))
                                    .FirstOrDefault()?
                                    .Description
                                    ;
            return originDate;
        }

        private string? GetCameraModel()
        {
            var cameraModel = tags.Where(x => x.DirectoryName.Contains(Exif.ExifTag) && x.TagName.Contains(Exif.CameraModelTag))
                                    .FirstOrDefault()?
                                    .Description
                                    ;

            return cameraModel;
        }

        private string? GetCameraMake()
        {
            var cameraMake = tags.Where(x => x.DirectoryName.Contains(Exif.ExifTag) && x.TagName.Contains(Exif.CameraMakeTag))
                                    .FirstOrDefault()?
                                    .Description
                                    ;

            return cameraMake;
        }

        private DateTime? GetValidDateTime(string input)
        {
            DateTime temp;
            DateTime temp2;
            string[] formats =
            {
                "dd/MM/yyyy HH:mm:ss", "dd-MM-yyyy HH:mm:ss", "dd.MM.yyyy HH:mm:ss", "dd MM yyyy HH:mm:ss", // DMY formats
                "yyyy/MM/dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss", "yyyy.MM.dd HH:mm:ss", "yyyy MM dd HH:mm:ss", // YMD formats
                "MM/dd/yyyy HH:mm:ss", "MM-dd-yyyy HH:mm:ss", "MM.dd.yyyy HH:mm:ss", "MM dd yyyy HH:mm:ss", // MDY formats
                "yyyy:MM:dd HH:mm:ss", "yyyy-MM-ddTHH:mm:ss.fffzzz" // ISO 8601 format
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
                var directories = ImageMetadataReader.ReadMetadata(filePath);

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

        private void ExtractMetadataFromMultimediaFile(string filePath)
        {
            var directories = ImageMetadataReader.ReadMetadata(filePath);

            //string cameraModel = string.Empty;
            //string cameraMake = string.Empty;
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

            //cameraModel = cameraModel ?? string.Empty;
            //cameraMake = cameraMake ?? string.Empty;

            if (!string.IsNullOrEmpty(cameraModel) && !string.IsNullOrEmpty(cameraMake))
                isValidExif = true;

            DateTime? orginalDate = GetValidDateTime(originalTakenDateString);
            if (orginalDate != null)
            {
                takenDate = (DateTime)(orginalDate);
                isValidTakenDate = true;
            }
        }

        private void ExtractMetadataFromNormalFile(string filePath)
        {
            // Use alternative methods to extract metadata for normal files
            // For example, you can try to retrieve the creation date using the .NET file class:
            var creationTime = File.GetCreationTime(filePath);
            if (creationTime != default)
            {
                takenDate = creationTime;
                isValidTakenDate = true;
            }
        }
    }
}
