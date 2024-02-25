using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace RinhaBackend.Infrastructure.Data;

public class RinhaContext : DbContext
{
    public RinhaContext(DbContextOptions<RinhaContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}