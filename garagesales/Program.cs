using garagesales.Models;
using garagesales.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine(builder.Configuration.GetConnectionString("GarageSaleDatabase"));
var connectionstring = builder.Configuration.GetConnectionString("GarageSaleDatabase");

try
{
    using var connection = new SqlConnection(builder.Configuration.GetConnectionString("GarageSaleDatabase"));
    connection.Open();

    Console.WriteLine("Success");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex}");
}

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Database1Context>(options => options.UseSqlServer(connectionstring));
builder.Services.AddHttpClient<GarageSalesService>(client => client.BaseAddress = new Uri("https://localhost:44332/")); 

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
