namespace DynamoXmlConverter.Models.Results
{
    /// <summary>
    /// Outcome of upload operation
    /// </summary>
    public class UploadOperationResult
    {
        public UploadOperationResult()
        {
            this.Files = new List<FileUploadResult>();
        }

        /// <summary>
        /// General message about the outcome of the operation
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// List of individual results per uploaded file
        /// </summary>
        public List<FileUploadResult> Files { get; set; }
    }
}