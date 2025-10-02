using ContractLib;
using ContractLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductSelector.Data;
using ProductSelector.Models;
using ProductSelector.ViewModels;
using PulumiDeployment;
using System.Diagnostics;

namespace ProductSelector.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;
        private string CurrentUser => "adil";

        public ProductsController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var user = _db.Users.Include(u => u.UserProducts).ThenInclude(up => up.Product)
                        .FirstOrDefault(u => u.Username == CurrentUser);

            if (user == null)
            {
                user = new User { Username = CurrentUser };
                _db.Users.Add(user);
                _db.SaveChanges();
            }
            ViewBag.user = user.Username;
            var products = _db.Products.ToList();

            var viewModels = products.Select(p => new ProductViewModel
            {
                ProductId = p.ProductId,
                DisplayName = p.DisplayName,
                Version = p.Version,
                IsSelected = user.UserProducts.Any(up => up.ProductIdRef == p.Id)
            }).ToList();

            return View(viewModels);
        }

        public IActionResult Graph()
        {
            // 1. Get current user with their selected products
            var user = _db.Users
                .Include(u => u.UserProducts)
                .ThenInclude(up => up.Product)
                .FirstOrDefault(u => u.Username == CurrentUser);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // 3. Load contracts for the selected products
            var contracts = new List<ProductContract>();
            foreach (var up in user.UserProducts)
            {
                var product = up.Product;
                if (product == null) continue;

                var contractFile = Path.Combine(product.ProjectPath, "contract.yaml");

                if (System.IO.File.Exists(contractFile))
                {
                    contracts.Add(YamlContractParser.LoadFromFile(contractFile));
                }
                else
                {
                    Console.WriteLine($"⚠️ Contract not found for {product.ProductId} at {contractFile}");
                }
            }

            // 4. Build dependency graph
            var graph = DependencyResolver.BuildGraph(contracts);

            // 5. Map into view model
            var vm = new DependencyGraphViewModel
            {
                Products = contracts.Select(c => c.Product.DisplayName).ToList(),
                Connections = graph.ResolvedConnections
            };

            return View(vm);
        }




        public async Task<IActionResult> Deploy(bool destroy)
        {
            // 1. Get current user with related products
            var user = _db.Users
                .Include(u => u.UserProducts)
                .ThenInclude(up => up.Product)
                .FirstOrDefault(u => u.Username == CurrentUser);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // 2. Resolve solution root -> Samples folder
            var exePath = AppContext.BaseDirectory; // bin/Debug/netX.X/
            var solutionRoot = Path.GetFullPath(Path.Combine(exePath, "..", "..", "..", ".."));
            var samplesPath = Path.Combine(solutionRoot, "Samples");

            // 3. Collect contracts for this user's products
            var contracts = new List<ProductContract>();
            foreach (var up in user.UserProducts)
            {
                var product = up.Product;
                if (product == null) continue;

                var contractFile = Path.Combine(product.ProjectPath, "contract.yaml");

                if (System.IO.File.Exists(contractFile))
                {
                    var contract = YamlContractParser.LoadFromFile(contractFile);
                    contract.Product.ProjectPath =  product.ProjectPath;
                    contracts.Add(contract);
                }
                else
                {
                    Console.WriteLine($"Contract not found for {product.ProductId} at {contractFile}");
                }
            }

            // 4. Deploy selected contracts
            if (contracts.Any())
            {
                if(destroy)
                {
                    var res = await Deployer.DeployAsync(contracts, destroy: true);
                    return Ok(new
                    {
                        message = "Destruction finished",
                        //outputs
                    });
                }
                else
                {
                    var result = await Deployer.DeployAsync(contracts, destroy: false);

                    // Collect outputs into a dictionary for response
                    var outputs = new Dictionary<string, object?>();
                    foreach (var kvp in result?.Outputs)
                    {
                        outputs[kvp.Key] = kvp.Value.Value;
                    }

                    return Ok(new
                    {
                        message = "Deployment finished",
                        outputs
                    });
                }
            }   
            else
            {
                Console.WriteLine("No contracts found to deploy.");
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult Select(string productId)
        {
            var user = _db.Users.Include(u => u.UserProducts).First(u => u.Username == CurrentUser);
            var product = _db.Products.First(p => p.ProductId == productId);

            if (!user.UserProducts.Any(up => up.ProductIdRef == product.Id))
            {
                user.UserProducts.Add(new UserProduct
                {
                    ProductIdRef = product.Id,
                    UserId = user.Id
                });
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Unselect(string productId)
        {
            var user = _db.Users.Include(u => u.UserProducts).First(u => u.Username == CurrentUser);
            var product = _db.Products.First(p => p.ProductId == productId);

            var existing = user.UserProducts.FirstOrDefault(up => up.ProductIdRef == product.Id);
            if (existing != null)
            {
                user.UserProducts.Remove(existing);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
