using FI.AtividadeEntrevista.Application;
using FI.AtividadeEntrevista.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(builder.Environment.ContentRootPath, "App_Data"));

builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(opcoes => opcoes.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
