using dii.storage;
using dii.storage.cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Data;
using TodoApp.Hubs;

public class Program 
{
    public static void Main(string[] args) 
    {
        var builder = WebApplication.CreateBuilder(args);

        IServiceCollection services = new ServiceCollection();
        new Startup().ConfigureServices(services);

        var optimizer = Optimizer.Get();
        var diiCosmosContext = DiiCosmosContext.Get();

        builder.Services.AddSingleton(optimizer);
        builder.Services.AddSingleton(diiCosmosContext);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddSignalR();

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

        app.UseEndpoints(endpoints => 
        {
            endpoints.MapHub<TodoHub>("/todohub");
        });

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}