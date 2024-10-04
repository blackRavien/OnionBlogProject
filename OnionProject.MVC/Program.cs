using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnionProject.Application.IoC;
using OnionProject.Domain.Entities;
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
            builder.Services.AddRazorPages(); // Razor Pages servisini ekleyin

            string connectionString = builder.Configuration.GetConnectionString("DefaultConncetion");

            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

            // Identity Configuration
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Cookie ayarlarý - eriþim yetkisi olmayan sayfalarda yönlendirme
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login"; // Giriþ sayfasý
                options.AccessDeniedPath = "/Account/AccessDenied"; // Yetkisiz eriþim sayfasý
            });


            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()); // Autofac için

            builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterModule(new DependencyResolver());
            }); // IoC klasöründeki DependencyResolver sýnýfý burada configuration olarak algýlansýn.

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); // Statik dosyalar için middleware

            app.UseRouting();
            app.UseAuthentication(); // Kimlik doðrulama middleware
            app.UseAuthorization(); // Yetkilendirme middleware

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");



            //deneme:
            app.MapControllerRoute(
                name: "user",
                pattern: "User/{controller=UserPost}/{action=Index}/{id?}");
            //deneme


            // Eðer Area kullanýyorsanýz bu alan rotasý olmalý
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            


            app.Run();
        }
    }
}





//using Autofac;
//using Autofac.Core;
//using Autofac.Extensions.DependencyInjection;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using OnionProject.Application.IoC;
//using OnionProject.Domain.Entities;
//using OnionProject.Infrastructure.Context;

//namespace OnionProject.MVC
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.
//            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
//            // Razor Pages servisini ekleyin
//            builder.Services.AddRazorPages();

//            string connectionString = builder.Configuration.GetConnectionString("DefaultConncetion");

//            builder.Services.AddDbContext<AppDbContext>(/*opt => opt.UseSqlServer("Server=DESKTOP-JCTOH8S;Database=YZL3171BlogDB;Uid=sa;Pwd=123;TrustServerCertificate=True;")*/);
//            builder.Services.AddDbContext<AppDbContext>(/*opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConncetion"))*/);
//            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));
//            //Identity Configuration burada yapýlacak.TODO
//            builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();




//            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()); //autofac için


//            builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
//            {
//                builder.RegisterModule(new DependencyResolver());
//            }); //IoC klasöründeki DependencyResolver sýnýfý burada configuration olarak algýlansýn istediðimiz için.




//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (!app.Environment.IsDevelopment())
//            {
//                app.UseExceptionHandler("/Home/Error");
//                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//                app.UseHsts();
//            }

//            app.UseHttpsRedirection();


//            app.UseRouting();

//            app.UseStaticFiles();

//            app.UseAuthentication(); //ekledik

//            app.UseAuthorization();

//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapControllerRoute(
//                    name: "default",
//                    pattern: "{controller=Home}/{action=Index}/{id?}");

//                endpoints.MapRazorPages(); // Razor sayfalarýný ekle
//            });

//            app.MapControllerRoute(
//            name: "areas",
//            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");



//            //default area en altta kalsýn
//            app.MapControllerRoute(
//                name: "default",
//                pattern: "{controller=Home}/{action=Index}/{id?}");

//            app.Run();
//        }
//    }
//}
