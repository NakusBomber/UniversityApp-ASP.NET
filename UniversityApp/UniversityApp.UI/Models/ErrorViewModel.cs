namespace UniversityApp.UI.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ErrorIdNotEmpty => !string.IsNullOrEmpty(RequestId);
    }
}
