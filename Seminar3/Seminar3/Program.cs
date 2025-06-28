using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Seminar3.Abstractions;
using Seminar3.Mapper;
using Seminar3.Query;
using Seminar3.Services;

namespace Seminar3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("db") ??
        "Host=localhost;Port=5433;Database=my_GraphQL_db;Username=postgres;Password=Example"));

            builder.Services.AddScoped<IStockService, StockService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(cb =>
            {
                cb.Register(c => new AppDbContext(builder.Configuration.GetConnectionString("db"))).InstancePerDependency();
            });

            builder.Services.AddScoped<IStockService, StockService>();

            builder.Services
                .AddGraphQLServer()
                .AddQueryType<MySimpleQuery>()
                .AddQueryType<StockQuery>();

            builder.Services.AddHttpClient("ProductService", client =>
            {
                client.BaseAddress = new Uri("http://product-service-url/");
            });

            builder.Services.AddHttpClient("StockService", client =>
            {
                client.BaseAddress = new Uri("http://stock-service-url/");
            });

            builder.Configuration.GetConnectionString("db");

            builder.Services.AddMemoryCache();
            builder.Services.AddAutoMapper(typeof(MapperProfile));

            builder.Services.AddSingleton<IProductService, ProductService>();
            builder.Services.AddSingleton<IStorageService, StorageService>();
            builder.Services.AddSingleton<ICategoryService, CategoryService>();

            builder.Services.AddSingleton<AppDbContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapGraphQL();

            app.Run();
        }
    }
}
