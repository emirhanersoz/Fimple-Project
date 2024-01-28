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
    }
}
