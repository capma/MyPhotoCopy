using System;

namespace PhotoMove.Models
{
    public class ScanFile
    {
        // general properties
        public string fileName { get; set; } = string.Empty;
        public string filePath { get; set; } = string.Empty;
        public string? fileExtension { get; set; }

        // EXIF properties
        public string? ExifSubIfdDirectory_takenDate { get; set; }
        //public string? ExifSubIfdDirectory_cameraModel { get; set; }

        public string? ExifIfd0Directory_takenDate { get; set; }
        public string? ExifIfd0Directory_cameraModel { get; set; }
        public string? ExifIfd0Directory_cameraMake { get; set; }

        //public string? ExifThumbnailDirectory_takenDate { get; set; }
        //public string? ExifThumbnailDirectory_cameraModel { get; set; }

        // XMP properties
        public string? XMP_takenDate { get; set; }

        public DateTime takenDate { get; set; }
        public string? cameraModel { get; set; } = string.Empty;
        public string? cameraMake { get; set; } = string.Empty;


        // other properties
        public bool isValidExif { get; set; } = false;
        public bool isValidTakenDate { get; set; } = false;
    }
}
