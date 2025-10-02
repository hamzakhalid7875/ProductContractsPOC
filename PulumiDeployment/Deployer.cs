using ContractLib;
using ContractLib.Models;
using Pulumi;
using Pulumi.Automation;
using Pulumi.Docker;
using Pulumi.Docker.Inputs;

namespace PulumiDeployment
{
    public class Deployer
    {
        //public static async Task<UpResult> DeployAsync(List<ProductContract> contracts, bool destroy = false)
        //{
        //    // Build dependency graph
        //    var graph = DependencyResolver.BuildGraph(contracts);

        //    var program = PulumiFn.Create(() =>
        //    {
        //        foreach (var contract in contracts)
        //        {
        //            // Build a nice image name, e.g., product-api:1.0
        //            var imageName = $"{contract.Product.Id.ToLower()}:{contract.Product.Version}";

        //            // Use the ProjectPath from DB/contract instead of hardcoded relative path
        //            var projectPath = contract.Product.ProjectPath;

        //            var image = new Image(contract.Product.Id, new ImageArgs
        //            {
        //                Build = new DockerBuildArgs
        //                {
        //                    Context = projectPath
        //                },
        //                ImageName = imageName,
        //                SkipPush = true
        //            });

        //            var container = new Container(contract.Product.Id + "-container", new ContainerArgs
        //            {
        //                Image = image.ImageName,
        //                Name = contract.Product.Id.ToLower() + "-container",
        //                Ports =
        //                {
        //                    new ContainerPortArgs
        //                    {
        //                        Internal = 80,
        //                        External = contract.Ports.FirstOrDefault()?.ContainerPort
        //                    }
        //                }
        //            });
        //        }
        //    });


        //    var stackArgs = new InlineProgramArgs("ProductDeployer", "dev", program)
        //    {
        //        WorkDir = Directory.GetCurrentDirectory(),
        //        EnvironmentVariables = new Dictionary<string, string>
        //        {
        //            ["PULUMI_CONFIG_PASSPHRASE"] = "",
        //            ["PULUMI_SECRETS_PROVIDER"] = "plaintext",
        //            ["PULUMI_BACKEND_URL"] = "file://./"
        //        }
        //    };

        //    var stack = await LocalWorkspace.CreateOrSelectStackAsync(stackArgs);

        //    if (destroy)
        //    {
        //        Console.WriteLine("🗑 Destroying resources...");
        //        var result = await stack.DestroyAsync(new DestroyOptions
        //        {
        //            OnStandardOutput = Console.WriteLine,
        //            OnStandardError = Console.Error.WriteLine
        //        });
        //        return (UpResult)result; // no UpResult for destroy
        //    }
        //    else
        //    {
        //        await stack.RefreshAsync(new RefreshOptions { OnStandardOutput = Console.WriteLine });

        //        var result = await stack.UpAsync(new UpOptions
        //        {
        //            OnStandardOutput = Console.WriteLine,
        //            OnStandardError = Console.Error.WriteLine
        //        });

        //        return result;
        //    }
        //}

        //public static async Task<UpResult?> DeployAsync(List<ProductContract> contracts, bool destroy = false)
        //{
        //    // Build dependency graph
        //    var graph = DependencyResolver.BuildGraph(contracts);


        //    var program = PulumiFn.Create(() =>
        //    {
        //        // Map productId -> external port (for ENV URL building)
        //        var productPorts = contracts.ToDictionary(
        //            c => c.Product.Id,
        //            c => c.Ports.FirstOrDefault()?.ContainerPort ?? 0
        //        );

        //        foreach (var contract in contracts)
        //        {
        //            var imageName = $"{contract.Product.Id.ToLower()}:{contract.Product.Version}";
        //            var projectPath = contract.Product.ProjectPath;

        //            var image = new Image(contract.Product.Id, new ImageArgs
        //            {
        //                Build = new DockerBuildArgs
        //                {
        //                    Context = projectPath
        //                },
        //                ImageName = imageName,
        //                SkipPush = true
        //            });

        //            // Collect environment variables for this product from dependency graph
        //            var envList = new InputList<string>();

        //            foreach (var kvp in graph.ResolvedConnections)
        //            {
        //                // Assuming key = "TargetProductId:InputName"
        //                //         val = "SourceProductId:OutputName"
        //                var targetParts = kvp.Key.Split(':');
        //                var sourceParts = kvp.Value.Split(':');

        //                if (targetParts.Length == 2 && sourceParts.Length == 2)
        //                {
        //                    var targetProductId = targetParts[0];
        //                    var targetInput = targetParts[1];

        //                    var sourceProductId = sourceParts[0];
        //                    var sourceOutput = sourceParts[1];

        //                    if (targetProductId == contract.Product.Id &&
        //                        productPorts.TryGetValue(sourceProductId, out var port) && port > 0)
        //                    {
        //                        var envName = targetInput.ToUpperInvariant() + "_URL";
        //                        var url = $"http://localhost:{port}/{sourceOutput}";
        //                        envList.Add($"{envName}={url}");
        //                    }
        //                }
        //            }

        //            var container = new Container(contract.Product.Id + "-container", new ContainerArgs
        //            {
        //                Image = image.ImageName,
        //                Name = contract.Product.Id.ToLower() + "-container",
        //                Ports =
        //        {
        //            new ContainerPortArgs
        //            {
        //                Internal = 80,
        //                External = contract.Ports.FirstOrDefault()?.ContainerPort
        //            }
        //        },
        //                Envs = envList
        //            });
        //        }
        //    });

        //    var stackArgs = new InlineProgramArgs("ProductDeployer", "dev", program)
        //    {
        //        WorkDir = Directory.GetCurrentDirectory(),
        //        EnvironmentVariables = new Dictionary<string, string>
        //        {
        //            ["PULUMI_CONFIG_PASSPHRASE"] = "",
        //            ["PULUMI_SECRETS_PROVIDER"] = "plaintext",
        //            ["PULUMI_BACKEND_URL"] = "file://./"
        //        }
        //    };

        //    var stack = await LocalWorkspace.CreateOrSelectStackAsync(stackArgs);

        //    if (destroy)
        //    {
        //        Console.WriteLine("🗑 Destroying resources...");
        //        await stack.DestroyAsync(new DestroyOptions
        //        {
        //            OnStandardOutput = Console.WriteLine,
        //            OnStandardError = Console.Error.WriteLine
        //        });
        //        return null;
        //    }
        //    else
        //    {
        //        await stack.RefreshAsync(new RefreshOptions { OnStandardOutput = Console.WriteLine });

        //        var result = await stack.UpAsync(new UpOptions
        //        {
        //            OnStandardOutput = Console.WriteLine,
        //            OnStandardError = Console.Error.WriteLine
        //        });

        //        return result;
        //    }
        //}


        #region last working snippet
        //public static async Task<UpResult?> DeployAsync(List<ProductContract> contracts, bool destroy = false)
        //{
        //    // Build dependency graph
        //    var graph = DependencyResolver.BuildGraph(contracts);

        //    var program = PulumiFn.Create(() =>
        //    {
        //        // Map productId -> external port (read from the contract's Ports)
        //        var productPorts = contracts.ToDictionary(
        //            c => c.Product.Id,
        //            c => c.Ports.FirstOrDefault()?.ContainerPort ?? 0);

        //        // For each contract produce image + container + envs
        //        foreach (var contract in contracts)
        //        {
        //            var imageName = $"{contract.Product.Id.ToLower()}:{contract.Product.Version}";

        //            // Prefer ProjectPath on contract.Product (set by caller); fallback to Samples folder
        //            var projectPath = contract.Product.ProjectPath;
        //            if (string.IsNullOrWhiteSpace(projectPath))
        //            {
        //                var exePath = AppContext.BaseDirectory;
        //                var solutionRoot = Path.GetFullPath(Path.Combine(exePath, "..", "..", "..", ".."));
        //                projectPath = Path.Combine(solutionRoot, "Samples", contract.Product.Id);
        //            }

        //            // Ensure path is absolute
        //            projectPath = Path.GetFullPath(projectPath);

        //            var image = new Image(contract.Product.Id, new ImageArgs
        //            {
        //                Build = new DockerBuildArgs
        //                {
        //                    Context = projectPath
        //                },
        //                ImageName = imageName,
        //                SkipPush = true
        //            });

        //            // Build env list for this container (Pulumi expects InputList<string> of "KEY=VALUE")
        //            var envList = new InputList<string>();

        //            // Find all resolved connections where this contract is the CONSUMER.
        //            // Keys are "consumerId.inputName", values are "providerId.outputName"
        //            foreach (var kvp in graph.ResolvedConnections)
        //            {
        //                if (!kvp.Key.StartsWith(contract.Product.Id + ".", StringComparison.OrdinalIgnoreCase))
        //                    continue;

        //                // parse consumer key -> [consumerId, inputName]
        //                var keyParts = kvp.Key.Split(new[] { '.' }, 2);
        //                // parse provider value -> [providerId, outputName]
        //                var valParts = kvp.Value.Split(new[] { '.' }, 2);

        //                if (keyParts.Length != 2 || valParts.Length != 2) continue;

        //                var targetInput = keyParts[1];        // e.g. "authUrl" (the input name on consumer)
        //                var sourceProductId = valParts[0];    // e.g. "product-auth"
        //                var sourceOutputName = valParts[1];   // e.g. "authUrl" (the output name on provider)

        //                // find provider external port (must be set in contract.Ports)
        //                if (!productPorts.TryGetValue(sourceProductId, out var externalPort) || externalPort <= 0)
        //                    continue; // can't build URL without an external port

        //                // Build sensible env var name for the consumer.
        //                // e.g. input "authUrl" -> AUTH_URL
        //                var envVarName = MakeEnvVarNameFromInput(targetInput);

        //                // Derive a path from the provider output name:
        //                // heuristics: strip "Url" / "URL" / "Endpoint" / "endpoint" suffixes and lowercase
        //                var routePath = MakeRouteFromOutputName(sourceOutputName);

        //                // Build URL using host.docker.internal so container can reach host-mapped port.
        //                // (Docker Desktop on Windows/Mac supports host.docker.internal)
        //                var url = $"http://host.docker.internal:{externalPort}/{routePath}".TrimEnd('/');

        //                envList.Add($"{envVarName}={url}");
        //            }

        //            // Create container - internal port is 80 (your service should listen on 80 inside container)
        //            var externalPortToUse = contract.Ports.FirstOrDefault()?.ContainerPort ?? 0;

        //            var container = new Container(contract.Product.Id + "-container", new ContainerArgs
        //            {
        //                Image = image.ImageName,
        //                Name = contract.Product.Id.ToLower() + "-container",
        //                Ports =
        //            {
        //                new ContainerPortArgs
        //                {
        //                    Internal = 80,
        //                    External = externalPortToUse
        //                }
        //            },
        //                Envs = envList
        //            });
        //        }
        //    });

        //    var stackArgs = new InlineProgramArgs("ProductDeployer", "dev", program)
        //    {
        //        WorkDir = Directory.GetCurrentDirectory(),
        //        EnvironmentVariables = new Dictionary<string, string>
        //        {
        //            ["PULUMI_CONFIG_PASSPHRASE"] = "",
        //            ["PULUMI_SECRETS_PROVIDER"] = "plaintext",
        //            ["PULUMI_BACKEND_URL"] = "file://./"
        //        }
        //    };

        //    var stack = await LocalWorkspace.CreateOrSelectStackAsync(stackArgs);

        //    if (destroy)
        //    {
        //        Console.WriteLine("🗑 Destroying resources...");
        //        await stack.DestroyAsync(new DestroyOptions
        //        {
        //            OnStandardOutput = Console.WriteLine,
        //            OnStandardError = Console.Error.WriteLine
        //        });
        //        return null;
        //    }
        //    else
        //    {
        //        await stack.RefreshAsync(new RefreshOptions { OnStandardOutput = Console.WriteLine });

        //        var result = await stack.UpAsync(new UpOptions
        //        {
        //            OnStandardOutput = Console.WriteLine,
        //            OnStandardError = Console.Error.WriteLine
        //        });

        //        return result;
        //    }
        //}

        //private static string MakeEnvVarNameFromInput(string inputName)
        //{
        //    // Example: "authUrl" -> "AUTH_URL"
        //    var s = inputName.Replace('.', '_'); // defensive
        //                                         // if it already contains "url" suffix, keep it; otherwise append _URL for clarity
        //    if (s.EndsWith("Url", StringComparison.OrdinalIgnoreCase) ||
        //        s.EndsWith("URL", StringComparison.OrdinalIgnoreCase))
        //    {
        //        // transform camelCase to SNAKE_CASE-ish
        //        return ToUpperSnake(s);
        //    }
        //    else
        //    {
        //        return ToUpperSnake(s) + "_URL";
        //    }
        //}

        //private static string MakeRouteFromOutputName(string outputName)
        //{
        //    if (string.IsNullOrWhiteSpace(outputName)) return string.Empty;

        //    var route = outputName;

        //    // remove common suffixes
        //    if (route.EndsWith("Url", StringComparison.OrdinalIgnoreCase))
        //        route = route.Substring(0, route.Length - 3);
        //    if (route.EndsWith("URL", StringComparison.OrdinalIgnoreCase))
        //        route = route.Substring(0, route.Length - 3);
        //    if (route.EndsWith("Endpoint", StringComparison.OrdinalIgnoreCase))
        //        route = route.Substring(0, route.Length - 8);

        //    // e.g. "authUrl" -> "auth"
        //    return route.Trim().ToLowerInvariant();
        //}

        //private static string ToUpperSnake(string camel)
        //{
        //    if (string.IsNullOrEmpty(camel)) return camel;
        //    var sb = new System.Text.StringBuilder();
        //    foreach (var c in camel)
        //    {
        //        if (char.IsUpper(c) && sb.Length > 0) sb.Append('_');
        //        sb.Append(char.ToUpperInvariant(c));
        //    }
        //    return sb.ToString();
        //}
        #endregion


        public static async Task<UpResult> DeployAsync(List<ProductContract> contracts, bool destroy = false)
        {
            // Build dependency graph (resolves inputs -> outputs)
            var graph = DependencyResolver.BuildGraph(contracts);

            foreach (var contract in contracts)
            {
                // Docker image name, e.g. product-api:1.0.0
                var imageName = $"{contract.Product.Id.ToLower()}:{contract.Product.Version}";
                var projectPath = contract.Product.ProjectPath;

                // Build Docker image
                //var image = new Image(contract.Product.Id, new ImageArgs
                //{
                //    Build = new DockerBuildArgs
                //    {
                //        Context = projectPath
                //    },
                //    ImageName = imageName,
                //    SkipPush = true
                //});

                // Collect env vars from dependency graph
                var envs = new Dictionary<string, string>();
                foreach (var input in contract.Inputs)
                {
                    var key = $"{contract.Product.Id}.{input.Name}";
                    if (graph.ResolvedConnections.TryGetValue(key, out var value))
                    {
                        var parts = value.Split('.');
                        var targetProductId = parts[0];
                        var outputName = parts[1];

                        // Create ENV var name like AUTH_URL, API_URL, etc.
                        var envVar = input.Name.ToUpperInvariant(); //$"{targetProductId}_{outputName}".ToUpperInvariant();
                        var targetPort = contracts
                            .First(p => p.Product.Id == targetProductId)
                            .Ports.First().externalPort;

                        envs[envVar] = $"http://localhost:{targetPort}";
                    }
                }

                // Deploy Docker container
                //var container = new Container(contract.Product.Id + "-container", new ContainerArgs
                //{
                //    Image = image.ImageName,
                //    Name = contract.Product.Id.ToLower() + "-container",
                //    Envs = envs.Select(kv => $"{kv.Key}={kv.Value}").ToList(),
                //    Ports =
                //{
                //        new ContainerPortArgs
                //        {
                //            Internal = contract.Ports.First().containerPort,  // usually 80
                //            External = contract.Ports.First().externalPort    // from contract.yaml
                //        }
                //}
                //});
            }


            var program = PulumiFn.Create(() =>
            {
                foreach (var contract in contracts)
                {
                    // Docker image name, e.g. product-api:1.0.0
                    var imageName = $"{contract.Product.Id.ToLower()}:{contract.Product.Version}";
                    var projectPath = contract.Product.ProjectPath;

                    // Build Docker image
                    var image = new Image(contract.Product.Id, new ImageArgs
                    {
                        Build = new DockerBuildArgs
                        {
                            Context = projectPath
                        },
                        ImageName = imageName,
                        SkipPush = true
                    });

                    // Collect env vars from dependency graph
                    var envs = new Dictionary<string, string>();
                    foreach (var input in contract.Inputs)
                    {
                        var key = $"{contract.Product.Id}.{input.Name}";
                        if (graph.ResolvedConnections.TryGetValue(key, out var value))
                        {
                            var parts = value.Split('.');
                            var targetProductId = parts[0];
                            var outputName = parts[1];

                            // Create ENV var name like AUTH_URL, API_URL, etc.
                            var envVar = input.Name.ToUpperInvariant();
                            var targetPort = contracts
                                .First(p => p.Product.Id == targetProductId)
                                .Ports.First().externalPort;

                            envs[envVar] = $"http://localhost:{targetPort}";
                        }
                    }

                    // Deploy Docker container
                    var container = new Container(contract.Product.Id + "-container", new ContainerArgs
                    {
                        Image = image.ImageName,
                        Name = contract.Product.Id.ToLower() + "-container",
                        Envs = envs.Select(kv => $"{kv.Key}={kv.Value}").ToList(),
                        Ports =
                {
                        new ContainerPortArgs
                        {
                            Internal = contract.Ports.First().containerPort,  // usually 80
                            External = contract.Ports.First().externalPort    // from contract.yaml
                        }
                }
                    });
                }
            });

            // Pulumi stack setup
            var stackArgs = new InlineProgramArgs("ProductDeployer", "dev", program)
            {
                WorkDir = Directory.GetCurrentDirectory(),
                EnvironmentVariables = new Dictionary<string, string>
                {
                    ["PULUMI_CONFIG_PASSPHRASE"] = "",
                    ["PULUMI_SECRETS_PROVIDER"] = "plaintext",
                    ["PULUMI_BACKEND_URL"] = "file://./"
                }
            };

            var stack = await LocalWorkspace.CreateOrSelectStackAsync(stackArgs);

            if (destroy)
            {
                Console.WriteLine("🗑 Destroying resources...");
                await stack.DestroyAsync(new DestroyOptions
                {
                    OnStandardOutput = Console.WriteLine,
                    OnStandardError = Console.Error.WriteLine
                });
                return null;
            }
            else
            {
                await stack.RefreshAsync(new RefreshOptions { OnStandardOutput = Console.WriteLine });

                var result = await stack.UpAsync(new UpOptions
                {
                    OnStandardOutput = Console.WriteLine,
                    OnStandardError = Console.Error.WriteLine
                });

                return result;
            }
        }
    }
}
