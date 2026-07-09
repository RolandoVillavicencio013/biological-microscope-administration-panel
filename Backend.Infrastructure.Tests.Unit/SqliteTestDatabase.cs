using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Persistence;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Tests.Unit;

/// <summary>Creates an isolated SQLite in-memory <see cref="AppDbContext"/> for a single test.</summary>
internal sealed class SqliteTestDatabase : IDisposable
{
    private readonly SqliteConnection _connection;

    public AppDbContext Context { get; }

    public SqliteTestDatabase()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        Context = CreateContext();
        Context.Database.EnsureCreated();
    }

    /// <summary>Creates a new, independently-tracked <see cref="AppDbContext"/> against the same
    /// underlying database, simulating a fresh per-request scope like production DI would provide.</summary>
    public AppDbContext CreateContext() =>
        new(new DbContextOptionsBuilder<AppDbContext>().UseSqlite(_connection).Options);

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
    }
}
