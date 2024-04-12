namespace PhotoMove.Models
{
    public class ScanFileReport
    {
        public int Index { get; set; }
        public string File { get; set; } = string.Empty;
        public string? Make { get; set; } = string.Empty;
        public string? Model { get; set; } = string.Empty;
        public string Date { get; set; }
        public string Time { get; set; }
    }
}
