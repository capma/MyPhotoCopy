using MetadataExtractor;
using PhotoMove.Constants;
using System;
using System.Collections.Generic;
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
            isValidExif = true;
            isValidTakenDate = true;
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
    }
}
