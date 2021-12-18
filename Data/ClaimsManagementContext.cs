using ClaimsManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClaimsManagementSystem.Data;

public class ClaimsManagementContext : DbContext
{
    public ClaimsManagementContext(DbContextOptions<ClaimsManagementContext> options)
        : base(options)
    {
    }

    public DbSet<Claim> Claims => Set<Claim>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClaimsManagementContext).Assembly);
    }
}
