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

            // Cookie ayarlar� - eri�im yetkisi olmayan sayfalarda y�nlendirme
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login"; // Giri� sayfas�
                options.AccessDeniedPath = "/Account/AccessDenied"; // Yetkisiz eri�im sayfas�
            });


            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()); // Autofac i�in

            builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterModule(new DependencyResolver());
            }); // IoC klas�r�ndeki DependencyResolver s�n�f� burada configuration olarak alg�lans�n.

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); // Statik dosyalar i�in middleware

            app.UseRouting();
            app.UseAuthentication(); // Kimlik do�rulama middleware
            app.UseAuthorization(); // Yetkilendirme middleware

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");



            //deneme:
            app.MapControllerRoute(
                name: "user",
                pattern: "User/{controller=UserPost}/{action=Index}/{id?}");
            //deneme


            // E�er Area kullan�yorsan�z bu alan rotas� olmal�
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
//            //Identity Configuration burada yap�lacak.TODO
//            builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();




//            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()); //autofac i�in


//            builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
//            {
//                builder.RegisterModule(new DependencyResolver());
//            }); //IoC klas�r�ndeki DependencyResolver s�n�f� burada configuration olarak alg�lans�n istedi�imiz i�in.




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

//                endpoints.MapRazorPages(); // Razor sayfalar�n� ekle
//            });

//            app.MapControllerRoute(
//            name: "areas",
//            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");



//            //default area en altta kals�n
//            app.MapControllerRoute(
//                name: "default",
//                pattern: "{controller=Home}/{action=Index}/{id?}");

//            app.Run();
//        }
//    }
//}
