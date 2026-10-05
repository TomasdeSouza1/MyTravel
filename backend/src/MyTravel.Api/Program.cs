using System.Text;                                                                                               
using System.Threading.RateLimiting;                                                                             
using Microsoft.AspNetCore.Authentication.JwtBearer;                                                             
using Microsoft.AspNetCore.Identity;                                                                             
using Microsoft.AspNetCore.RateLimiting;                                                                         
using Microsoft.EntityFrameworkCore;                                                                             
using Microsoft.IdentityModel.Tokens;                                                                            
using MyTravel.Api.Middlewares;                                                                                  
using MyTravel.Application.Interfaces;                                                                           
using MyTravel.Domain.Entities;                                                                                  
using MyTravel.Infrastructure.Persistence;                                                                       
using MyTravel.Infrastructure.Services; 
using MyTravel.Application.DI;
using MyTravel.Api.Filters;
using Scalar.AspNetCore;
using MyTravel.Infrastructure.Services.Trips;



var builder = WebApplication.CreateBuilder(args);

                                                                
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>(name: "database");
//Manejo de excepciones 
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

//RateLimiting
builder.Services.AddRateLimiter(rateLimiterOptions =>                                                            
    {                                                                                                                
        rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;                               
        rateLimiterOptions.AddPolicy("fixed-by-ip", httpContext =>                                                   
        {                                                                                                            
            var clientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";                          
            return RateLimitPartition.GetFixedWindowLimiter(clientIp, _ => new FixedWindowRateLimiterOptions         
            {                                                                                                        
                PermitLimit = 100,                                                                                   
                Window = TimeSpan.FromMinutes(1),                                                                    
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,                                             
                QueueLimit = 0                                                                                       
            });                                                                                                      
        });                                                                                                          
    }); 
 //Identity                                            
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 10;
})
.AddRoles<IdentityRole<Guid>>()
.AddEntityFrameworkStores<ApplicationDbContext>();
//JWT
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key no esta configurado");

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero
    };
});

//inyeccion de dependecias.
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddApplication();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseRateLimiter();

//Authentication antes de Authorization                                                   
app.UseAuthentication();
app.UseAuthorization();
//Endpoint de saluid del sistema 
app.MapHealthChecks("/health");                                
app.MapControllers().RequireRateLimiting("fixed-by-ip");


app.Run();                                                                                             
