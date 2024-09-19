using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using OnionProject.Application.IoC;
using OnionProject.Infrastructure.Context;

namespace OnionProject.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

            string connectionString = builder.Configuration.GetConnectionString("DefaultConncetion");

            builder.Services.AddDbContext<AppDbContext>(/*opt => opt.UseSqlServer("Server=DESKTOP-JCTOH8S;Database=YZL3171BlogDB;Uid=sa;Pwd=123;TrustServerCertificate=True;")*/);
            builder.Services.AddDbContext<AppDbContext>(/*opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConncetion"))*/);
            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));



            //Identity Configuration burada yapýlacak.



            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()); //autofac için


            builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterModule(new DependencyResolver());
            }); //IoC klasöründeki DependencyResolver sýnýfý burada configuration olarak algýlansýn istediðimiz için.




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

            app.UseAuthentication(); //ekledik

            app.UseAuthorization();

            app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");



            //default area en altta kalsýn
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
