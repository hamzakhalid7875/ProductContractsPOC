using ContractLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractLib
{
    public class DependencyResolver
    {
        public class ResolvedGraph
        {
            public List<ProductContract> Products { get; set; } = new();
            public Dictionary<string, string> ResolvedConnections { get; set; } = new();
        }

        /// <summary>
        /// Match product outputs to inputs
        /// </summary>
        public static ResolvedGraph BuildGraph(List<ProductContract> prodContracts)
        {
            var graph = new ResolvedGraph { Products = prodContracts };

            foreach (var prodContract in prodContracts)
            {
                foreach (var input in prodContract.Inputs)
                {
                    var provider = prodContracts
                        .SelectMany(p => p.Outputs, (p, o) => new { Product = p, Output = o })
                        .FirstOrDefault(x => x.Output.Name == input.Name && x.Output.Type == input.Type);

                    if (provider != null)
                    {
                        // Example: product-api.authUrl -> product-auth.authUrl
                        var key = $"{prodContract.Product.Id}.{input.Name}";
                        var value = $"{provider.Product.Product.Id}.{provider.Output.Name}";
                        graph.ResolvedConnections[key] = value;
                    }
                    else if (input.Required)
                    {
                        throw new InvalidOperationException(
                            $"Unresolved dependency: {prodContract.Product.Id} requires {input.Name} ({input.Type})");
                    }
                }
            }

            return graph;
        }
    }
}
