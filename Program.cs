using ClaimsManagementSystem.Data;
using ClaimsManagementSystem.Middleware;
using ClaimsManagementSystem.Services.Claims;
using Microsoft.EntityFrameworkCore;

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
app.UseAuthorization();
app.MapControllers();
app.Run();
