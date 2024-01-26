using Azure;
using Fi.Patika.Api.IntegrationTests.Initialization;
using Fi.Patika.Schema.Model;
using Fi.Test.Extensions;
using FizzWare.NBuilder;
using Newtonsoft.Json;
using Should;
using Xunit;
using Xunit.Abstractions;

namespace Fi.Patika.Api.IntegrationTests.Controllers
{
    public class MoneyTransfersControllerTests : PatikaScenariosBase
    {
        private const string basePath = "api/v1/Patika/MoneyTransfers";
        private readonly ITestOutputHelper output;
        private HelperMethodsForTests helperMethodsForTests;

        public MoneyTransfersControllerTests(ITestOutputHelper output, PatikaApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
        {
            this.output = output;
            helperMethodsForTests = new HelperMethodsForTests(fiTestApplicationFactory);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task Create_IfItemExists_ReturnsSuccess_WithUpdatedBalance()
        {
            // Arrange
            const decimal balance = 1500;
            const decimal amount = 500;

            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

            var userInputModel = Builder<UserInputModel>.CreateNew()
                    .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray).With(p => p.Id = 1)
                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var customerInputModel = Builder<CustomerInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            customerInputModel.UserId = 1;

            var senderAccountInputModel = Builder<AccountInputModel>.CreateNew()
                     .With(p => p.Balance = balance)
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            senderAccountInputModel.Id = 1;
            senderAccountInputModel.CustomerId = 1;

            var destAccountInputModel = Builder<AccountInputModel>.CreateNew()
                     .With(p => p.Balance = balance)
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            destAccountInputModel.Id = 2;
            destAccountInputModel.CustomerId = 1;

            var moneyTransferInputModel = Builder<MoneyTransferInputModel>.CreateNew()
                     .With(p => p.Amount = amount)
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            moneyTransferInputModel.AccountId = 1;
            moneyTransferInputModel.DestAccountId = 2;

            //Act
            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
                "api/v1/Patika/Customers", customerInputModel);

            var senderAccountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                 "api/v1/Patika/Accounts", senderAccountInputModel);

            var destAccountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                 "api/v1/Patika/Accounts", destAccountInputModel);

            var moneytTransferCreateResponse = await HttpClient.FiPostTestAsync<MoneyTransferInputModel, MoneyTransferOutputModel>(
                $"{basePath}", moneyTransferInputModel);

            var checkUserResponse = await HttpClient.FiGetTestAsync<UserOutputModel>(
                 $"api/v1/Patika/Users/{userCreateResponse.Value.Id}", false);

            var checkCustomerResponse = await HttpClient.FiGetTestAsync<CustomerOutputModel>(
                $"api/v1/Patika/Customers/{customerCreateResponse.Value.Id}", false);

            var checkSenderAccountResponse = await HttpClient.FiGetTestAsync<AccountOutputModel>(
                $"api/v1/Patika/Accounts/{senderAccountCreateResponse.Value.Id}", false);

            var checkDescAccountResponse = await HttpClient.FiGetTestAsync<AccountOutputModel>(
                $"api/v1/Patika/Accounts/{destAccountCreateResponse.Value.Id}", false);

            var checkMoneyTransferResponse = await HttpClient.FiGetTestAsync<MoneyTransferInputModel>(
                $"{basePath}/{moneytTransferCreateResponse.Value.Id}", false);

            // Assert
            checkMoneyTransferResponse.FiShouldBeSuccessStatus();
            checkMoneyTransferResponse.Value.ShouldNotBeNull();
            checkMoneyTransferResponse.Value.Amount.ShouldEqual(amount);

            output.WriteLine($"Money Transfer Response: {checkSenderAccountResponse.Value}");
            output.WriteLine($"Money Transfer Response: {checkDescAccountResponse.Value}");
            output.WriteLine($"Money Transfer Response: {checkMoneyTransferResponse.Value}");
        }
    }
}
