namespace Web.API.Helpers
{
    public class EmailDto
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public byte[]? Attachments { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public string? StoredFileName { get; set; }
    }
}
