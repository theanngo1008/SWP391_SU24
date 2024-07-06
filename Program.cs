using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using JewelryProductionOrder.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using JewelryProductionOrder.BusinessLogic.RequestModels.Gold;
using JewelryProductionOrder.BusinessLogic.Services.Implementation;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using JewelryProductionOrder.BusinessLogic.Services.Interface;
//using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add services to the container 
builder.Services.AddDbContext<JewelrySystemDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("JewelrySystemDBConn")));

builder.Services.AddHttpClient<SpotGoldPriceService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<SpotGoldPriceService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<JewelryService>();
builder.Services.AddScoped<GemstoneService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();
builder.Services.AddScoped<IServiceWrapper, ServiceWrapper>();

builder.Services.Configure<GoldApiSettings>(builder.Configuration.GetSection("GoldAPI"));

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = builder.Configuration["JWTAuthentication:Issuer"],
            ValidAudience = builder.Configuration["JWTAuthentication:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTAuthentication:Key"]!))
        };
    });

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "JWT Authorization using the Bearer scheme. \"Bearer\" is not needed.Just paste the jwt"
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

//Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

//Add Session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
/*
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("appsettings.json")
});*/
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception: {ex.Message}");
        throw;
    }
});

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowAll");

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
