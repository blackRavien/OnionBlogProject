
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.EntityFrameworkCore;
using OnionProject.Application.IoC;
using OnionProject.Infrastructure.Context;

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



            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()); //autofac i�in


            builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterModule(new DependencyResolver());
            }); //IoC klas�r�ndeki DependencyResolver s�n�f� burada configuration olarak alg�lans�n istedi�imiz i�in.

            // API Program.cs dosyas�nda CORS ayarlar�
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

            app.UseCors("AllowAllOrigins"); // CORS'u kullan�yorum

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
