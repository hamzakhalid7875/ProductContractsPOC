//using ContractLib;
//using ContractLib.Models;
//using Pulumi;
//using Pulumi.Automation;
//using Pulumi.Docker;
//using System.ComponentModel;
//using static System.Net.Mime.MediaTypeNames;

//namespace ProductSelector.Services
//{
//    public class DeploymentService
//    {
//        public async Task DeployAsync(List<ProductContract> contracts)
//        {
//            // Build dependency graph
//            var graph = DependencyResolver.BuildGraph(contracts);

//            // Create Pulumi program dynamically
//            var program = PulumiFn.Create(() =>
//            {
//                foreach (var contract in contracts)
//                {
//                    // Image name = product id
//                    var imageName = $"{contract.Product.Id.ToLower()}:{contract.Product.Version}";

//                    var image = new Image(contract.Product.Id, new ImageArgs
//                    {
//                        Build = new DockerBuild { Context = $"../../../../Samples/{contract.Product.Id}" },
//                        ImageName = imageName,
//                    });

//                    // Run container
//                    var container = new Container(contract.Product.Id + "-container", new ContainerArgs
//                    {
//                        Image = image.ImageName,
//                        Name = contract.Product.Id.ToLower(),
//                        Ports =
//                        {
//                            new ContainerPortArgs { Internal = 80, External = GetPort(contract.Product.Id) }
//                        }
//                    });
//                }
//            });

//            // Setup Pulumi stack
//            var stackArgs = new InlineProgramArgs("local", "dev", program);
//            var stack = await LocalWorkspace.CreateOrSelectStackAsync(stackArgs);

//            await stack.UpAsync();
//        }

//        private int GetPort(string productId)
//        {
//            // naive static mapping for demo
//            return productId switch
//            {
//                "product-auth" => 5001,
//                "product-api" => 5002,
//                "product-frontend" => 5003,
//                _ => 5000
//            };
//        }
//    }
//}
