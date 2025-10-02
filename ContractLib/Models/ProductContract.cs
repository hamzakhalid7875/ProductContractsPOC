using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ContractLib.Models
{
    public class ProductContract
    {
        public ProductInfo Product { get; set; } = new();
        public List<Port> Ports { get; set; } = new();
        public List<ContractInput> Inputs { get; set; } = new();
        public List<ContractOutput> Outputs { get; set; } = new();
        public List<ConfigItem> Config { get; set; } = new();
    }

    public class ProductInfo
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Version { get; set; } = "0.1.0";
        public string Image { get; set; } = string.Empty;
        public string ProjectPath { get; set; }
    }

    public class Port
    {
        public string Name { get; set; } = string.Empty;
        public int containerPort { get; set; }
        public int externalPort { get; set; }
    }

    public class ContractInput
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool Required { get; set; }
    }

    public class ContractOutput
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }

    public class ConfigItem
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;  // secret, string, int, etc.
        public bool Required { get; set; }
    }
}
