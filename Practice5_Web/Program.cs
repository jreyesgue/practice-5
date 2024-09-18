using Microsoft.EntityFrameworkCore;
using Practice5_DataAccess.Data;
using Practice5_DataAccess.Service.ADO;
using Practice5_DataAccess.Service.EF;
using Practice5_Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ProductServiceEF>();
builder.Services.AddScoped<ProductServiceADO>(provider =>
    new ProductServiceADO(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<InventoryServiceEF>();
builder.Services.AddScoped<InventoryServiceADO>(provider =>
    new InventoryServiceADO(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<PurchaseServiceEF>();
builder.Services.AddScoped<PurchaseServiceADO>(provider =>
    new PurchaseServiceADO(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<SaleServiceEF>();
builder.Services.AddScoped<SaleServiceADO>(provider =>
    new SaleServiceADO(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ServiceFactory>();
builder.Services.AddSession();

var app = builder.Build();

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
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
