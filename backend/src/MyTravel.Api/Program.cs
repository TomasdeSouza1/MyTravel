using System.Text;                                                                                               
using System.Threading.RateLimiting;                                                                             
using Microsoft.AspNetCore.Authentication.JwtBearer;                                                             
using Microsoft.AspNetCore.Identity;                                                                             
using Microsoft.AspNetCore.RateLimiting;                                                                         
using Microsoft.EntityFrameworkCore;                                                                             
using Microsoft.IdentityModel.Tokens;                                                                            
using MyTravel.Api.Middlewares;                                                                                  
using MyTravel.Application.Interfaces.Auth;
using MyTravel.Application.Interfaces.Trip;
using MyTravel.Application.Interfaces.Itinerary;
using MyTravel.Application.Interfaces.Activity;                                                                           
using MyTravel.Domain.Entities;                                                                                  
using MyTravel.Infrastructure.Persistence;                                                                       
using MyTravel.Infrastructure.Services; 
using MyTravel.Application.DI;
using MyTravel.Api.Filters;
using Scalar.AspNetCore;
using MyTravel.Infrastructure.Services.Trips;
using MyTravel.Infrastructure.Services.Itineraries;
using MyTravel.Infrastructure.Services.Activities;
using MyTravel.Application.Interfaces.Flights;                                                                                                         
using MyTravel.Infrastructure.Services.Flights; 



var builder = WebApplication.CreateBuilder(args);

                                                                
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("ConnectionStrings:DefaultConnection no está configurado.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>(name: "database");
//Manejo de excepciones 
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
// OpenAPI / Scalar - el registro es obligatorio antes de app.MapOpenApi()
builder.Services.AddOpenApi();

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

    //Lockout
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
.AddRoles<IdentityRole<Guid>>()
.AddEntityFrameworkStores<ApplicationDbContext>();
//JWT
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException("Jwt:Key no está configurado.");
}

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
builder.Services.AddScoped<IItineraryDayService, ItineraryDayService>();
builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IFlightService, FlightService>();
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
//Endpoint de salud del sistema 
app.MapHealthChecks("/health");                                
app.MapControllers().RequireRateLimiting("fixed-by-ip");


app.Run();                                                                                             
