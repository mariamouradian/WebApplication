using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Seminar1.Abstraction;
using Seminar1.Repo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddMemoryCache(o => o.TrackStatistics = true);

// Добавляем контекст базы данных
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseLazyLoadingProxies()
           .UseNpgsql(builder.Configuration.GetConnectionString("ProductDatabase")));

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterType<ProductRepository>().As<IProductRepository>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();