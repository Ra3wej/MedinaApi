using AspNetCoreRateLimit;
using MedinaApi.Data;
using MedinaApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<Medina_Api_DbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json")
    .Build();
//
builder.Services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    /// In case signalR hubs added uncomment this and change the routes acordingly. 
    //option.Events = new JwtBearerEvents
    //{

    //    OnMessageReceived = context =>
    //    {
    //        var accessToken = context.Request.Query["access_token"];
    //        //Console.WriteLine("______________________________________________");
    //        //Console.WriteLine("Acess token is empty or null " + string.IsNullOrEmpty(accessToken));
    //        // If the request is for our hub...
    //        //Console.WriteLine("acess token" + accessToken);
    //        var path = context.HttpContext.Request.Path;
    //        //Console.WriteLine("Path starts with signal r" + path.StartsWithSegments("/chatHub"));
    //        //Console.WriteLine("______________________________________________");
    //        if (!string.IsNullOrEmpty(accessToken) &&
    //            (path.StartsWithSegments("/chatHub")))
    //        {
    //            //Console.WriteLine("Reached here");
    //            // Read the token out of the query string
    //            context.Token = accessToken;
    //        }
    //        return Task.CompletedTask;
    //    }
    //};
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetValue<string>("Jwt:Key")))
    };

});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
builder.Services.AddOptions();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
//builder.Services.AddSingleton<IFirebaseServices, FirebaseServices>();

var app = builder.Build();
app.UseIpRateLimiting();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
}

app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//           Path.Combine(builder.Environment.ContentRootPath, "Files")),
//    RequestPath = "/Files"
//});


app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
    app.MapControllers().AllowAnonymous();
else
    app.MapControllers();

app.UseIpRateLimiting();
app.Run();
