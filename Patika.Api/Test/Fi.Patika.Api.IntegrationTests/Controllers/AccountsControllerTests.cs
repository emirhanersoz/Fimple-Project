using Fi.Patika.Api.Domain.Entity;
using Fi.Patika.Api.IntegrationTests.Initialization;
using Fi.Patika.Schema.Model;
using Fi.Test.Extensions;
using FizzWare.NBuilder;
using Newtonsoft.Json;
using Should;
using System.Drawing.Text;
using Xunit;
using Xunit.Abstractions;

namespace Fi.Patika.Api.IntegrationTests.Controllers
{
    public class AccountsControllerTests : PatikaScenariosBase
    {
        private const string basePath = "api/v1/Patika/Accounts";
        private readonly ITestOutputHelper output;
        private HelperMethodsForTests helperMethodsForTests;

        public AccountsControllerTests(ITestOutputHelper output, PatikaApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
        {
            this.output = output;
            helperMethodsForTests = new HelperMethodsForTests(fiTestApplicationFactory);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task CreateAccount_IfRequestedUserAndCustomerExist_ReturnsSuccess_WithItem()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Account>();

            var accountInputModel = Builder<AccountInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            //Act
            var accountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                $"{basePath}", accountInputModel);

            // Assert
            accountCreateResponse.FiShouldBeSuccessStatus();
            accountCreateResponse.Value.ShouldNotBeNull();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task UpdateAccount_WhenCalled_ReturnsSuccess_WithUpdatedAccount()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Account>();

            var accountInputModel = Builder<AccountInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var accountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                $"{basePath}", accountInputModel);

            accountInputModel.AccountType = AccountType.Investment;

            // Act
            var response = await HttpClient.FiPutTestAsync<AccountInputModel?, AccountOutputModel>(
                                            $"{basePath}/{accountCreateResponse.Value.Id}", accountInputModel, false);

            // Assert
            response.FiShouldBeSuccessStatus();
            response.Value.ShouldNotBeNull();
            response.Value.AccountType.ShouldEqual(AccountType.Investment);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task DeleteAccountByKey_WhenCalled_ReturnsSuccess()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Account>();

            var accountInputModel = Builder<AccountInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var accountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                $"{basePath}", accountInputModel);

            // Act
            var response = await HttpClient.FiDeleteTestAsync(
                                            $"{basePath}/{accountCreateResponse.Value.Id}");

            // Assert
            response.FiShouldBeSuccessStatus();
            TestDbContext.Set<Account>().FirstOrDefault(p => p.Id == accountCreateResponse.Value.Id).ShouldBeNull();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetAccountByKey_IfRequestedItemExists_ReturnsSuccess_WithItem()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Account>();

            var accountInputModel = Builder<AccountInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var accountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                $"{basePath}", accountInputModel);

            // Act
            var response = await HttpClient.FiGetTestAsync<AccountOutputModel>(
                                            $"{basePath}/{accountCreateResponse.Value.Id}", false);

            // Assert
            response.FiShouldBeSuccessStatus();
            response.Value.ShouldNotBeNull();
            response.Value.Id.ShouldEqual(accountCreateResponse.Value.Id);
        }
    }
}
