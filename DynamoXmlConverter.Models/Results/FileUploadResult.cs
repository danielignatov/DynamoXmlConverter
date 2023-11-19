namespace DynamoXmlConverter.Models.Results
{
    /// <summary>
    /// Outcome of a file upload attempt
    /// </summary>
    public class FileUploadResult
    {
        /// <summary>
        /// Denotes a successful acceptance of the file
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Provides information about the file upload attempt
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// File name of the received file
        /// </summary>
        public string? FileName { get; set; }
    }
}