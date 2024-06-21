using BE.Entities;
using BE.Models;
using BE.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
builder.Services.AddHttpClient<SpotMetalPriceService>();
builder.Services.AddScoped<SpotMetalPriceService>();
builder.Services.AddScoped<AccountService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<JewelrySystemDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<GoldApiSettings>(builder.Configuration.GetSection("GoldAPI"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("MN"));
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("AD"));
    options.AddPolicy("RequireCustomerRole", policy => policy.RequireRole("US"));
    options.AddPolicy("RequireSalesStaffRole", policy => policy.RequireRole("SS"));
    options.AddPolicy("RequireDesignStaffRole", policy => policy.RequireRole("DS"));
    options.AddPolicy("RequireProductionStaffRole", policy => policy.RequireRole("PS"));
    // Thêm các chính sách khác nếu cần
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

/*FirebaseApp.Create(new AppOptions()
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
