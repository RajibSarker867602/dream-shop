using AutoMapper;
using DreamShop.Services.ShoppingCartAPI.Data;
using DreamShop.Services.ShoppingCartAPI.Extensions;
using DreamShop.Services.ShoppingCartAPI.Models;
using DreamShop.Services.ShoppingCartAPI.Services;
using DreamShop.Services.ShoppingCartAPI.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[]{}
        }
    });
});

// http client service registration
builder.Services.AddHttpClient("product",
    c => c.BaseAddress
        = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]));

// service registration
builder.Services.AddScoped<IProductService, ProductService>();

// for object circular loop
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
}).AddXmlDataContractSerializerFormatters();
builder.Services.Configure<FormOptions>(x =>
{
    x.ValueCountLimit = int.MaxValue;
    x.ValueLengthLimit = int.MaxValue;
    x.MemoryBufferThreshold = Int32.MaxValue;
    x.MultipartBodyLengthLimit = long.MaxValue;
});

// register db context
builder.Services.AddDbContext<ShoppingCartAPIDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// auto mapper configuration
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

#region add authentication to validating jwt token

builder.AddAppAuthentication();
builder.Services.AddAuthorization();

#endregion add authentication to validating jwt token

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

app.Run();