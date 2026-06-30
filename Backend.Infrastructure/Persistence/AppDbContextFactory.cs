using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Persistence;

/// <summary>Design-time factory used by EF Core tooling (migrations) when the full DI host is not available.</summary>
internal sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql("Host=localhost;Database=biomicroscope_admin;Username=postgres;Password=postgres")
            .Options;
        return new AppDbContext(options);
    }
}
