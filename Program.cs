using System.Text;
using ClaimsManagementSystem.Data;
using ClaimsManagementSystem.Middleware;
using ClaimsManagementSystem.Services.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IClaimsService, ClaimsService>();

var useInMemoryForDevelopment = builder.Configuration.GetValue<bool>("DataStore:UseInMemoryForDevelopment");
builder.Services.AddDbContext<ClaimsManagementContext>(options =>
{
    if (builder.Environment.IsDevelopment() && useInMemoryForDevelopment)
    {
        options.UseInMemoryDatabase("ClaimsManagementDb");
        return;
    }

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var jwtSigningKey = builder.Configuration["Jwt:SigningKey"];

if (string.IsNullOrWhiteSpace(jwtIssuer) || string.IsNullOrWhiteSpace(jwtAudience) || string.IsNullOrWhiteSpace(jwtSigningKey))
{
    throw new InvalidOperationException("JWT configuration is missing. Please configure Jwt:Issuer, Jwt:Audience, and Jwt:SigningKey.");
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSigningKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ClaimsReadAccess", policy => policy.RequireRole("Admin", "Adjuster"));
    options.AddPolicy("ClaimsCreateAccess", policy => policy.RequireRole("Admin", "Adjuster", "Customer"));
    options.AddPolicy("ClaimsUpdateAccess", policy => policy.RequireRole("Admin", "Adjuster"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ClaimsManagementContext>();
    if (!dbContext.Database.IsInMemory())
    {
        dbContext.Database.Migrate();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
