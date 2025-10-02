using Microsoft.EntityFrameworkCore;
using ProductSelector.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Sync Products from YAML contracts at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Resolve Samples folder from solution root
    var basePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Samples");
    basePath = Path.GetFullPath(basePath);

    foreach (var dir in Directory.GetDirectories(basePath))
    {
        var contractFile = Path.Combine(dir, "contract.yaml");
        if (File.Exists(contractFile))
        {
            var contract = ContractLib.YamlContractParser.LoadFromFile(contractFile);

            var productId = contract.Product.Id;
            var displayName = contract.Product.DisplayName;
            var version = contract.Product.Version;

            // Use folder path as ProjectPath
            var projectPath = dir;

            var existing = db.Products.FirstOrDefault(p => p.ProductId == productId);
            if (existing == null)
            {
                db.Products.Add(new ProductSelector.Models.Product
                {
                    ProductId = productId,
                    DisplayName = displayName,
                    Version = version,
                    ProjectPath = projectPath // ✅ store path
                });
            }
            else
            {
                existing.DisplayName = displayName;
                existing.Version = version;
                existing.ProjectPath = projectPath; // ✅ update if path changes
            }
        }
    }

    db.SaveChanges();
}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
