using DynamoXmlConverter.UI.Models.Shared;

namespace DynamoXmlConverter.UI.Models.Home
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            this.Alerts = new List<AlertViewModel>();
        }

        public List<AlertViewModel> Alerts { get; set; }
    }
}