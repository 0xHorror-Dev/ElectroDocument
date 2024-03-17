namespace ElectroDocument.Models
{
    public class ErrorViewModel : LayoutModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
