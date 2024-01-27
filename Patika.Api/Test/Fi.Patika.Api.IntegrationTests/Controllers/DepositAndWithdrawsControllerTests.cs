using Fi.Patika.Api.IntegrationTests.Initialization;
using Fi.Patika.Schema.Model;
using Fi.Test.Extensions;
using FizzWare.NBuilder;
using Should;
using Xunit;
using Xunit.Abstractions;

namespace Fi.Patika.Api.IntegrationTests.Controllers
{
    public class DepositAndWithdrawsControllerTests : PatikaScenariosBase
    {
        private const string basePath = "api/v1/Patika/DepositAndWithdraws";
        private readonly ITestOutputHelper output;
        private HelperMethodsForTests helperMethodsForTests;

        public DepositAndWithdrawsControllerTests(ITestOutputHelper output, PatikaApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
        {
            this.output = output;
            helperMethodsForTests = new HelperMethodsForTests(fiTestApplicationFactory);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task DepositToAccount_IfRequestedNotExist_ReturnsSuccess_WithUpdatedBalance()
        {
            //Arrange
            const decimal balance = 100000;
            const decimal withdrawAmount = 10000;
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

            var depositAndWithdrawInputModel = Builder<DepositAndWithdrawInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            depositAndWithdrawInputModel.isSucceded = false;
            depositAndWithdrawInputModel.AccountId = 1;
            depositAndWithdrawInputModel.Amount = withdrawAmount;

            //Act
            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
                "api/v1/Patika/Customers", customerInputModel);

            var accountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                "api/v1/Patika/Accounts", accountInputModel);

            var depositAndWithdrawCreateResponse = await HttpClient.FiPostTestAsync<DepositAndWithdrawInputModel, DepositAndWithdrawOutputModel>(
                $"{basePath}", depositAndWithdrawInputModel);

            var checkDepositResponse = await HttpClient.FiPutTestAsync<DepositAndWithdrawInputModel, DepositAndWithdrawOutputModel>(
                $"{basePath}/{depositAndWithdrawInputModel.Id}/deposit", depositAndWithdrawInputModel, false);

            var checDepositAndWithdrawResponse = await HttpClient.FiGetTestAsync<DepositAndWithdrawOutputModel>(
                $"{basePath}/{depositAndWithdrawCreateResponse.Value.Id}", false);

            var checkUpdatedAccount = await HttpClient.FiGetTestAsync<AccountOutputModel>(
                     $"api/v1/Patika/Accounts/{accountCreateResponse.Value.Id}", false);

            // Assert
            checDepositAndWithdrawResponse.FiShouldBeSuccessStatus();
            checDepositAndWithdrawResponse.Value.ShouldNotBeNull();

            output.WriteLine(checkUpdatedAccount.Value.Balance.ToString());
        }

        [Fact, Trait("Category", "Integration")]
        public async Task WithdrawToAccount_IfRequestedNotExist_ReturnsSuccess_WithUpdatedBalance()
        {
            //Arrange
            const decimal balance = 100000;
            const decimal withdrawAmount = 10000;
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

            var depositAndWithdrawInputModel = Builder<DepositAndWithdrawInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            depositAndWithdrawInputModel.isSucceded = false;
            depositAndWithdrawInputModel.AccountId = 1;
            depositAndWithdrawInputModel.Amount = withdrawAmount;

            //Act
            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
                "api/v1/Patika/Customers", customerInputModel);

            var accountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                "api/v1/Patika/Accounts", accountInputModel);

            var depositAndWithdrawCreateResponse = await HttpClient.FiPostTestAsync<DepositAndWithdrawInputModel, DepositAndWithdrawOutputModel>(
                $"{basePath}", depositAndWithdrawInputModel);

            var checkWithdrawResponse = await HttpClient.FiPutTestAsync<DepositAndWithdrawInputModel, DepositAndWithdrawOutputModel>(
                $"{basePath}/{depositAndWithdrawInputModel.Id}/withdraw", depositAndWithdrawInputModel, false);

            var checDepositAndWithdrawResponse = await HttpClient.FiGetTestAsync<DepositAndWithdrawOutputModel>(
                $"{basePath}/{depositAndWithdrawCreateResponse.Value.Id}", false);

            var checkUpdatedAccount = await HttpClient.FiGetTestAsync<AccountOutputModel>(
                     $"api/v1/Patika/Accounts/{accountCreateResponse.Value.Id}", false);

            // Assert
            checDepositAndWithdrawResponse.FiShouldBeSuccessStatus();
            checDepositAndWithdrawResponse.Value.ShouldNotBeNull();

            output.WriteLine(checkUpdatedAccount.Value.Balance.ToString());
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetDepositAndWithByAccountKey_IfRequestedItemExists_ReturnsSuccess_WithItem()
        {
            //Arrange
            const decimal balance = 100000;
            const decimal withdrawAmount = 10000;
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

            var depositAndWithdrawInputModel = Builder<DepositAndWithdrawInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            depositAndWithdrawInputModel.isSucceded = false;
            depositAndWithdrawInputModel.AccountId = 1;
            depositAndWithdrawInputModel.Amount = withdrawAmount;

            //Act
            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
                "api/v1/Patika/Customers", customerInputModel);

            var accountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                "api/v1/Patika/Accounts", accountInputModel);

            var depositAndWithdrawCreateResponse = await HttpClient.FiPostTestAsync<DepositAndWithdrawInputModel, DepositAndWithdrawOutputModel>(
                $"{basePath}", depositAndWithdrawInputModel);

            // Act
            var response = await HttpClient.FiGetTestAsync<DepositAndWithdrawOutputModel>(
                                            $"{basePath}/{depositAndWithdrawCreateResponse.Value.Id}", false);

            // Assert
            response.FiShouldBeSuccessStatus();
            response.Value.ShouldNotBeNull();
            response.Value.Id.ShouldEqual(depositAndWithdrawCreateResponse.Value.Id);
        }
    }
}
