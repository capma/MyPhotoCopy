namespace PhotoMove.Models
{
    public class ScanStatistics
    {
        public int totalCount { get; set; } = 0;
        public int exifFileCount { get; set; } = 0;
        public int validDateCount { get; set; } = 0;
        public int noValidDateCount { get; set; } = 0;
    }
}
