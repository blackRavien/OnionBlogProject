using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.EntityFrameworkCore;
using OnionProject.Application.IoC;
using OnionProject.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using OnionProject.Domain.Entities;

namespace OnionProject.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string connectionString = builder.Configuration.GetConnectionString("DefaultConncetion");
            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

            // Identity Setup
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();


            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()); //autofac için


            builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterModule(new DependencyResolver());
            }); //IoC klasöründeki DependencyResolver sýnýfý burada configuration olarak algýlansýn istediðimiz için.

            // API Program.cs dosyasýnda CORS ayarlarý
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles(); // Statik dosyalar için middleware DENEME

            app.UseCors("AllowAllOrigins"); // CORS'u kullanýyorum

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
