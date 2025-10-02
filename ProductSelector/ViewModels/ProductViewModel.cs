namespace ProductSelector.ViewModels
{
    public class ProductViewModel
    {
        public string ProductId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}
