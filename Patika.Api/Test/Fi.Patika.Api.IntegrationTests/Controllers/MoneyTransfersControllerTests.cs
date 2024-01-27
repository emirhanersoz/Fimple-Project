using Azure;
using Fi.Patika.Api.Domain.Entity;
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
        public async Task CreateTransfer_IfAccountsExist_ReturnsSuccess_WithUpdatedBalances()
        {
            // Arrange
            const decimal balance = 1500;
            const decimal totalDailyTransferLimit = 1000;
            const decimal amount = 500;

            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

            var userInputModel = Builder<UserInputModel>.CreateNew()
                    .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray).With(p => p.Id = 1)
                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var customerInputModel = Builder<CustomerInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            customerInputModel.UserId = 1;

            var senderAccountInputModel = Builder<AccountInputModel>.CreateNew()
                     .With(p => p.Balance = balance).With(p => p.TotailDailyTransferAmount = totalDailyTransferLimit)
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
                $"{basePath}/transfer", moneyTransferInputModel);

            var response = await HttpClient.FiGetTestAsync<MoneyTransferOutputModel>(
                                $"{basePath}/{moneytTransferCreateResponse.Value.Id}", false);

            // Assert
            moneytTransferCreateResponse.FiShouldBeSuccessStatus();
            moneytTransferCreateResponse.Value.ShouldNotBeNull();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task UpdateMoneyTransfer_WhenCalled_ReturnsSuccess_WithUpdatedMoneyTransfer()
        {
            // Arrange
            const decimal balance = 1500;
            const decimal totalDailyTransferLimit = 1000;
            const decimal amount = 500;

            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

            var userInputModel = Builder<UserInputModel>.CreateNew()
                    .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray).With(p => p.Id = 1)
                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var customerInputModel = Builder<CustomerInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            customerInputModel.UserId = 1;

            var senderAccountInputModel = Builder<AccountInputModel>.CreateNew()
                     .With(p => p.Balance = balance).With(p => p.TotailDailyTransferAmount = totalDailyTransferLimit)
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
                $"{basePath}/transfer", moneyTransferInputModel);

            moneyTransferInputModel.Comment = "CommentComment";

            // Act
            var response = await HttpClient.FiPutTestAsync<MoneyTransferInputModel?, MoneyTransferOutputModel>(
                                            $"{basePath}/{moneytTransferCreateResponse.Value.Id}", moneyTransferInputModel, false);

            // Assert
            response.FiShouldBeSuccessStatus();
            response.Value.ShouldNotBeNull();
            response.Value.Comment.ShouldEqual("CommentComment");
        }

        [Fact, Trait("Category", "Integration")]
        public async Task DeleteAccountByKey_WhenCalled_ReturnsSuccess()
        {
            // Arrange
            const decimal balance = 1500;
            const decimal totalDailyTransferLimit = 1000;
            const decimal amount = 500;

            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

            var userInputModel = Builder<UserInputModel>.CreateNew()
                    .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray).With(p => p.Id = 1)
                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var customerInputModel = Builder<CustomerInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            customerInputModel.UserId = 1;

            var senderAccountInputModel = Builder<AccountInputModel>.CreateNew()
                     .With(p => p.Balance = balance).With(p => p.TotailDailyTransferAmount = totalDailyTransferLimit)
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

            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
                "api/v1/Patika/Customers", customerInputModel);

            var senderAccountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                 "api/v1/Patika/Accounts", senderAccountInputModel);

            var destAccountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                 "api/v1/Patika/Accounts", destAccountInputModel);

            var moneytTransferCreateResponse = await HttpClient.FiPostTestAsync<MoneyTransferInputModel, MoneyTransferOutputModel>(
                $"{basePath}/transfer", moneyTransferInputModel);

            // Act
            var response = await HttpClient.FiDeleteTestAsync(
                                            $"{basePath}/{moneytTransferCreateResponse.Value.Id}");

            // Assert
            response.FiShouldBeSuccessStatus();
            TestDbContext.Set<MoneyTransfer>().FirstOrDefault(p => p.Id == moneytTransferCreateResponse.Value.Id).ShouldBeNull();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetMoneyTransferByAccountKey_IfRequestedItemExists_ReturnsSuccess_WithItem()
        {
            // Arrange
            const decimal balance = 1500;
            const decimal totalDailyTransferLimit = 1000;
            const decimal amount = 500;

            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

            var userInputModel = Builder<UserInputModel>.CreateNew()
                    .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray).With(p => p.Id = 1)
                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var customerInputModel = Builder<CustomerInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            customerInputModel.UserId = 1;

            var senderAccountInputModel = Builder<AccountInputModel>.CreateNew()
                     .With(p => p.Balance = balance).With(p => p.TotailDailyTransferAmount = totalDailyTransferLimit)
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

            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
                "api/v1/Patika/Customers", customerInputModel);

            var senderAccountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                 "api/v1/Patika/Accounts", senderAccountInputModel);

            var destAccountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                 "api/v1/Patika/Accounts", destAccountInputModel);

            var moneytTransferCreateResponse = await HttpClient.FiPostTestAsync<MoneyTransferInputModel, MoneyTransferOutputModel>(
                $"{basePath}/transfer", moneyTransferInputModel);

            // Act
            var response = await HttpClient.FiGetTestAsync<MoneyTransferOutputModel>(
                                            $"{basePath}/{moneytTransferCreateResponse.Value.Id}", false);

            // Assert
            response.FiShouldBeSuccessStatus();
            response.Value.ShouldNotBeNull();
            response.Value.Id.ShouldEqual(moneytTransferCreateResponse.Value.Id);
        }
    }
}
