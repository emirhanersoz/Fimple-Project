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
using System.Runtime.Versioning;
using Fi.Patika.Api.IntegrationTests.Controllers;
using System.Security.Cryptography;
using Fi.Infra.Utility;
using k8s.KubeConfigModels;

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
    {
        var dbContext = (MockDbContext)mockDbContext;

        byte[] byteArray = GeneratorByteCodes();

        var entityUser = Builder<Domain.Entity.User>.CreateNew()
                .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        entityUser.PasswordHash = byteArray;
        entityUser.PasswordSalt = byteArray;
        entityUser.Id = 1;

        var entityLogin = Builder<Login>.CreateNew()
                .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        entityLogin.Id = 1;
        entityLogin.UserId = 1;
        entityLogin.LoginTime = DateTimeHelper.UtcNow;

        var entityCustomer = Builder<Customer>.CreateNew()
            .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        entityCustomer.Id = 1;
        entityCustomer.UserId = 1;

        var entitySupportRequest = Builder<SupportRequest>.CreateNew()
            .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        entitySupportRequest.Id = 1;
        entitySupportRequest.CustomerId = 1;
        entitySupportRequest.isAnswered = false;

        var entityAccount = Builder<Account>.CreateNew()
            .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        entityAccount.Id = 1;
        entityAccount.CustomerId = 1;
        entityAccount.Salary = 10000;
        entityAccount.Balance = 10000;

        var entityDescAccount = Builder<Account>.CreateNew()
            .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        entityDescAccount.Id = 2;
        entityDescAccount.CustomerId = 1;
        entityDescAccount.Salary = 20000;
        entityDescAccount.Balance = 20000;

        var entityMoneyTransfer = Builder<MoneyTransfer>.CreateNew()
            .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        entityMoneyTransfer.Id = 1;
        entityMoneyTransfer.AccountId = 1;
        entityMoneyTransfer.DestAccountId = 2;

        var entityPayee = Builder<Payee>.CreateNew()
            .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        entityPayee.Id = 1;
        entityPayee.AccountId = 1;
        /*
        var entityDepositAndWithdraw = Builder<DepositAndWithdraw>.CreateNew()
            .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        entityDepositAndWithdraw.Id = 1;
        entityDepositAndWithdraw.AccountId = 1;
        */
        var entityCredit = Builder<Credit>.CreateNew()
            .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        entityCredit.Id = 1;
        entityCredit.MontlyPayment = 2500;
        entityCredit.RepaymentPeriodMonths = 20;
        entityCredit.LoanDate = DateTimeHelper.UtcNow.AddMonths(1);
        entityCredit.TotalAmount = 50000;

        var entityAccountCredit = Builder<AccountCredit>.CreateNew()
            .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        entityAccountCredit.Id = 1;
        entityAccountCredit.AccountId = 1;
        entityAccountCredit.CreditId = 1;

        await dbContext.AddAsync(entityUser);
        await dbContext.AddAsync(entityLogin);
        await dbContext.AddAsync(entityCustomer);
        await dbContext.AddAsync(entitySupportRequest);
        await dbContext.AddAsync(entityAccount);
        await dbContext.AddAsync(entityDescAccount);
        await dbContext.AddAsync(entityMoneyTransfer);
        await dbContext.AddAsync(entityPayee);
        //await dbContext.AddAsync(entityDepositAndWithdraw);
        await dbContext.AddAsync(entityCredit);
        await dbContext.AddAsync(entityAccountCredit);
        await dbContext.SaveChangesAsync();


        byte[] GeneratorByteCodes()
        {
            int byteCount = 16;
            byte[] byteArray = new byte[byteCount];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(byteArray);
            }

            return byteArray;
        }


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