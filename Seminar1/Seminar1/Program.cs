using Microsoft.EntityFrameworkCore;
using Seminar1.Models;
using Seminar1.Repo;
using System;
using System.Reflection;
using AutoMapper;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Seminar1.Abstraction;

namespace Seminar1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(ContainerBuilder =>
            {
                ContainerBuilder.RegisterType<ProductRepository>().As<IProductRepository>();
            });

            //builder.Services.AddSingleton<IProductRepository, ProductRepository>();
            builder.Services.AddMemoryCache(o => o.TrackStatistics = true);
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (builder.Environment.IsDevelopment())
            {
                app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();

        }

    }
}
