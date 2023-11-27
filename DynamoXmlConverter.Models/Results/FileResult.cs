namespace DynamoXmlConverter.Models.Results
{
    /// <summary>
    /// Retreve file result
    /// </summary>
    public class FileResult
    {
        public FileResult()
        {
            MimeType = "application/octet-stream";
        }

        public bool Success { get; set; }

        public byte[]? Data { get; set; }

        public string MimeType { get; set; }

        public string? FileName { get; set; }
    }
}