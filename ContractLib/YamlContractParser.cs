using ContractLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ContractLib
{
    public static class YamlContractParser
    {
        public static ProductContract LoadFromFile(string path)
        {
            var yaml = File.ReadAllText(path);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();

            var contract = deserializer.Deserialize<ProductContract>(yaml);
            if (contract == null)
            {
                throw new InvalidOperationException($"Invalid contract file: {path}");
            }

            return contract;
        }
    }
}
