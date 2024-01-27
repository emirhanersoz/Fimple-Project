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
            const decimal balance = 1000;
            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

            var userInputModel = Builder<UserInputModel>.CreateNew()
                    .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray).With(p => p.Id = 1)
                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var customerInputModel = Builder<CustomerInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            customerInputModel.UserId = 1;

            var accountInputModel = Builder<AccountInputModel>.CreateNew()
                     .With(p => p.Balance = balance)
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            accountInputModel.CustomerId = 1;

            //Act
            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
                "api/v1/Patika/Customers", customerInputModel);

            var accountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                $"{basePath}", accountInputModel);

            // Assert
            accountCreateResponse.FiShouldBeSuccessStatus();
            accountCreateResponse.Value.ShouldNotBeNull();
            accountCreateResponse.Value.Balance.ShouldEqual(balance);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task UpdateAccount_WhenCalled_ReturnsSuccess_WithUpdatedAccount()
        {
            //Arrange
            const decimal balance = 1000;
            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

            var userInputModel = Builder<UserInputModel>.CreateNew()
                    .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray).With(p => p.Id = 1)
                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var customerInputModel = Builder<CustomerInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            customerInputModel.UserId = 1;

            var accountInputModel = Builder<AccountInputModel>.CreateNew()
                     .With(p => p.Balance = balance)
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            accountInputModel.CustomerId = 1;

            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
                "api/v1/Patika/Customers", customerInputModel);

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
            const decimal balance = 1000;
            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

            var userInputModel = Builder<UserInputModel>.CreateNew()
                    .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray).With(p => p.Id = 1)
                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var customerInputModel = Builder<CustomerInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            customerInputModel.UserId = 1;

            var accountInputModel = Builder<AccountInputModel>.CreateNew()
                     .With(p => p.Balance = balance)
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            accountInputModel.CustomerId = 1;

            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
                "api/v1/Patika/Customers", customerInputModel);

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
            const decimal balance = 1000;
            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

            var userInputModel = Builder<UserInputModel>.CreateNew()
                    .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray).With(p => p.Id = 1)
                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var customerInputModel = Builder<CustomerInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            customerInputModel.UserId = 1;

            var accountInputModel = Builder<AccountInputModel>.CreateNew()
                     .With(p => p.Balance = balance)
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            accountInputModel.CustomerId = 1;

            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
                "api/v1/Patika/Customers", customerInputModel);

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
