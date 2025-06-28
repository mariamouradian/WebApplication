using Microsoft.EntityFrameworkCore;
using Seminar3.Abstractions;
using Seminar3.Mapper;
using Seminar3.Services;
using Seminar3;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Seminar3.Mutation;
using Seminar3.Query;

var builder = WebApplication.CreateBuilder(args);

// Add services

builder.Services.AddScoped<IProductRepository, MyRepository>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация сервисов
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStockService, StockService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<MySimpleQuery>()
    .AddMutationType<MySimpleMutation>()
    .AddType<MySimpleMutation>()
    .ModifyRequestOptions(opt =>
    {
        opt.IncludeExceptionDetails = builder.Environment.IsDevelopment();
    });

// MemoryCache
builder.Services.AddMemoryCache();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MapperProfile));

var app = builder.Build();

// Configure pipeline
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
