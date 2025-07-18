using TaskApi.Models;
using TaskApi.Services;
using TaskApi.Helpers;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using StackExchange.Redis;


//TODO
//buradaki logger configurationını classlara taşı
//sadece addSingleton kısmını değiştirerek logger değiştirebileyim
//sonraki logger nlogger olucak

var builder = WebApplication.CreateBuilder(args);

//serilog yapoyom

// Serilog.Debugging.SelfLog.Enable(msg => Console.Error.WriteLine(msg));


LoggingProvider.ConfigureLogging(builder.Configuration, builder.Host);


builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IListService, ListService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<TokenGenerate>();
builder.Services.AddSingleton<ILogService, NlogService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect("localhost:6379"));

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
}
);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
    options.RequireHttpsMetadata = false;
});

builder.Services.AddAuthorization();
var app = builder.Build();





using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        dbContext.Database.EnsureCreated(); // Ensures the database exists without migrations
        Console.WriteLine("Database schema verified successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database schema verification failed: {ex.Message}");
    }
}

app.UseCors("AllowAll");
app.UseRouting();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();