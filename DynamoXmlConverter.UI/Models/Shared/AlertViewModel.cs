namespace DynamoXmlConverter.UI.Models.Shared
{
    public class AlertViewModel
    {
        public AlertType AlertType { get; set; }

        public string? Text { get; set; }
    }

    public enum AlertType
    {
        Success,
        Info,
        Warning,
        Error
    }
}