using Autofac;
using Fi.Infra.Const;
using Fi.Infra.Schema.Model;
using Fi.Infra.Schema.Const;
using Fi.Persistence.Relational.Interfaces;
using Fi.Test.Extensions;
using Fi.Test.IntegrationTests;
using Fi.Test.IntegrationTests.Interfaces;
using Moq;
using FizzWare.NBuilder;
using Fi.Patika.Api.Domain.Entity;

namespace Fi.Patika.Api.IntegrationTests.Initialization;

public class PatikaApplicationFactory : FiIntegrationTestApplicationFactory<Startup>
{
    public override void ConfigureDbContext(IServiceCollection services)
    {
        services.AddTestDbContext<MockDbContext>();
        /* 
        If you have LookupDbContext, delete comment lines for this block.
        services.AddTestLookupDbContext<MockLookupDbContext>();
        */
    }

    protected override async void ModuleSeedData(IFiModuleDbContext mockDbContext, IServiceProvider sp)
    {/*
        var dbContext = (MockDbContext)mockDbContext;

        for(int i = 0; i < 5; i++)
        {
            var user = Builder<User>.CreateNew()
                .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var account = Builder<Account>.CreateNew()
                .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            user.Id = i;
            
            await dbContext.AddAsync(user);
            await dbContext.AddAsync(account);
            await dbContext.SaveChangesAsync();
        }*/


        /* 
        If you want to add global seed data for all your test, you can add them in here.
        This block is just an example, prepare this block according to your business logic.

        var dbContext = (MockDbContext)mockDbContext;

        var entityUser = Builder<Employee>.CreateNew()
                                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        entityUser.UserId = TestingConstants.UserId;
        entityUser.Email = TestingConstants.UserEmail;
        entityUser.Id = 0;
        dbContext.Employee.Add(entityUser);

        var entityApp = Builder<ApplicationDefinition>.CreateNew()
                                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        entityApp.UserId = null;
        entityApp.ApplicationCode = CommonConstants.FimpleWebSuiteApplicationCode;
        entityApp.ApplicationProviderId = null;
        entityApp.Id = 0;
        entityApp.UserId = TestingConstants.UserId;
        dbContext.ApplicationDefinition.Add(entityApp);

        dbContext.SaveChanges();
        */
    }

    protected override void LookupSeedData(IMockLookupDbContext lookupDbContext, IServiceProvider sp)
    {
        /* 
        If you have LookupDbContext, delete comment lines for this block.
        var dbContext = (MockLookupDbContext)lookupDbContext;

        lock (LookupDbContextObject)
        {
            try
            {
                This block is just an example, prepare this block according to your business logic.
                var serviceDef = dbContext.ServiceDefinitionLookup
                                    .Where(x => x.Name == "TenantSettings")
                                    .AsNoTracking().FirstOrDefault();
                if (serviceDef == null)
                {
                    dbContext.ServiceDefinitionLookup.Add(new Domain.LookupEntity.ServiceDefinitionLookup
                    {
                        Id = RandomHelper.GenerateRandomNumber(10000),
                        Description = "TenantSettings",
                        UniqueName = "TenantSettings",
                        Name = "TenantSettings",
                        ServiceType = ServiceType.Core,
                        IsActive = true,
                        ApplicationOwnership = FiApplicationOwnership.Fimple
                    });
                }

                dbContext.SaveChanges();
            }
            catch
            {
                // ignored
            }
        }
        */
    }

    public override IFiModuleDbContext CreateDbContext(IComponentContext componentContext)
    {
        return componentContext.Resolve<MockDbContext>();
    }

    public override List<string> TestScopeKeys => new List<string>
    {
        /*
        ScopeKeys.Patika.View_Patika,
        ScopeKeys.Patika.List_Patika,
        ScopeKeys.Patika.Create_Patika,
        ScopeKeys.Patika.Delete_Patika,
        ScopeKeys.Patika.Update_Patika
        */
    };

    protected override void ConfigureTestDependencies(ContainerBuilder containerBuilder)
    {
        // Modify your dependencies in this method.

        /* 
        If you have LookupDbContext, delete comment lines for this block.
        containerBuilder.Register<FiLookupDbContext>(x => x.Resolve<MockLookupDbContext>());
        */
    }
}