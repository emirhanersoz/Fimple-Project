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

        public MoneyTransfersControllerTests(ITestOutputHelper output, PatikaApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
        {
        }

        [Fact, Trait("Category", "Integration")]
        public async Task CreateTransfer_IfAccountsExist_ReturnsSuccess_WithUpdatedBalances()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<MoneyTransfer>();

            var moneyTransferInputModel = Builder<MoneyTransferInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            //Act
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
            await TestDbContext.EnsureEntityIsEmpty<MoneyTransfer>();

            var moneyTransferInputModel = Builder<MoneyTransferInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

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
            await TestDbContext.EnsureEntityIsEmpty<MoneyTransfer>();

            var moneyTransferInputModel = Builder<MoneyTransferInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

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
            await TestDbContext.EnsureEntityIsEmpty<MoneyTransfer>();

            var moneyTransferInputModel = Builder<MoneyTransferInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

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

        [Fact, Trait("Category", "Integration")]
        public async Task CreateTransfer_IfSenderAccountNotFound_ReturnsBadRequest()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<MoneyTransfer>();

            var moneyTransferInputModel = Builder<MoneyTransferInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            moneyTransferInputModel.AccountId = 9999;

            //Act
            var moneyTransferCreateResponse = await HttpClient.FiPostTestAsync<MoneyTransferInputModel, MoneyTransferOutputModel>(
                $"{basePath}/transfer", moneyTransferInputModel);

            // Assert
            moneyTransferCreateResponse.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task CreateTransfer_IfDestinationAccountNotFound_ReturnsBadRequest()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<MoneyTransfer>();

            var moneyTransferInputModel = Builder<MoneyTransferInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            moneyTransferInputModel.DestAccountId = 9999;

            //Act
            var moneyTransferCreateResponse = await HttpClient.FiPostTestAsync<MoneyTransferInputModel, MoneyTransferOutputModel>(
                $"{basePath}/transfer", moneyTransferInputModel);

            // Assert
            moneyTransferCreateResponse.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task CreateTransfer_NotEnoughBalance_ReturnsBadRequest()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<MoneyTransfer>();

            var moneyTransferInputModel = Builder<MoneyTransferInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            moneyTransferInputModel.AccountId = 1;
            moneyTransferInputModel.Amount = 90000;

            // Act
            var moneyTransferCreateResponse = await HttpClient.FiPostTestAsync<MoneyTransferInputModel, MoneyTransferOutputModel>(
                    $"{basePath}/transfer", moneyTransferInputModel);

            // Assert
            moneyTransferCreateResponse.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task CreateTransfer_IfDailyLimitExceeded_ReturnsBadRequest()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<MoneyTransfer>();

            var invalidAccountModel = Builder<Account>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            invalidAccountModel.Balance = 50000;
            invalidAccountModel.TotailDailyTransferAmount = 40000;

            var moneyTransferInputModel = Builder<MoneyTransferInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            moneyTransferInputModel.AccountId = 9999;
            moneyTransferInputModel.Amount = 20000;

            // Act
            var moneyTransferCreateResponse = await HttpClient.FiPostTestAsync<MoneyTransferInputModel, MoneyTransferOutputModel>(
                    $"{basePath}/transfer", moneyTransferInputModel);

            // Assert
            moneyTransferCreateResponse.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task UpdateNonexistentMoneyTransfer_WhenCalled_ReturnsBadRequestStatus()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<MoneyTransfer>();

            var moneyTransferInputModel = Builder<MoneyTransferInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            moneyTransferInputModel.AccountId = 1;

            int nonExistentMoneyTransferId = 9999;

            // Act
            var response = await HttpClient.FiPutTestAsync<MoneyTransferInputModel?, MoneyTransferOutputModel>(
                                            $"{basePath}/{nonExistentMoneyTransferId}", new MoneyTransferInputModel(), false);

            // Assert
            response.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task DeleteNonexistentMoneyTransfer_WhenCalled_ReturnsBadRequestStatus()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<MoneyTransfer>();

            var moneyTransferInputModel = Builder<MoneyTransferInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            moneyTransferInputModel.AccountId = 1;

            int nonExistentMoneyTransferId = 9999;

            // Act
            var response = await HttpClient.FiDeleteTestAsync($"{basePath}/{nonExistentMoneyTransferId}");

            // Assert
            response.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetNonexistentMoneyTransferByKey_IfRequestedItemNotExist_ReturnsBadRequestStatus()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<MoneyTransfer>();

            var moneyTransferInputModel = Builder<MoneyTransferInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            moneyTransferInputModel.AccountId = 1;

            int nonExistentMoneyTransferId = 9999;

            // Act
            var response = await HttpClient.FiGetTestAsync<MoneyTransferOutputModel>($"{basePath}/{nonExistentMoneyTransferId}", false);

            // Assert
            response.FiShouldBeBadRequestStatus();
        }
    }
}
