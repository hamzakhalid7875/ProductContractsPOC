using System.Collections.Generic;

namespace ProductSelector.ViewModels
{
    public class DependencyGraphViewModel
    {
        public List<string> Products { get; set; } = new();
        public Dictionary<string, string> Connections { get; set; } = new();
    }
}
