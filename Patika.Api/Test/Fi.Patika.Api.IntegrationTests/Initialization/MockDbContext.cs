using Fi.Infra.Context;
using Fi.Infra.Options;
using Fi.Persistence.Relational.Context.Factory;
using Fi.Patika.Api.Persistence;
using Fi.Test.IntegrationTests.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Fi.Patika.Api.IntegrationTests.Initialization;

public class MockDbContext : FiPatikaDbContext, IMockDbContext
{
    private readonly DbConnection _dbConnection;

    public MockDbContext(IFiDbContextFactory factory, DbConnection dbConnection) : base(factory)
    {
        _dbConnection = dbConnection;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.UseSqlite(_dbConnection);
    }
}
/* 
If you have LookupDbContext, delete comment lines for this block.
public class MockLookupDbContext : FiLookupDbContext, IMockLookupDbContext
{
    public MockLookupDbContext(IFiOptions fiOptions, ISessionContextDI sessionContextDi) :
        base(fiOptions, sessionContextDi)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.UseInMemoryDatabase("InMemoryDb");
    }
} */