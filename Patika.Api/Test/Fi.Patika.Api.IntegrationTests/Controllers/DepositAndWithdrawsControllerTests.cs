using Fi.Patika.Api.Domain.Entity;
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

        public DepositAndWithdrawsControllerTests(ITestOutputHelper output, PatikaApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
        {
        }

        [Fact, Trait("Category", "Integration")]
        public async Task DepositToAccount_IfRequestedNotExist_ReturnsSuccess_WithUpdatedBalance()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<DepositAndWithdraw>();

            var depositAndWithdrawInputModel = Builder<DepositAndWithdrawInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            depositAndWithdrawInputModel.Amount = 5000;

            //Act
            var depositAndWithdrawCreateResponse = await HttpClient.FiPostTestAsync<DepositAndWithdrawInputModel, DepositAndWithdrawOutputModel>(
                $"{basePath}", depositAndWithdrawInputModel);

            var checkWithdrawResponse = await HttpClient.FiPutTestAsync<DepositAndWithdrawInputModel, DepositAndWithdrawOutputModel>(
                $"{basePath}/{depositAndWithdrawCreateResponse.Value.Id}/deposit", depositAndWithdrawInputModel, false);

            var checDepositAndWithdrawResponse = await HttpClient.FiGetTestAsync<DepositAndWithdrawOutputModel>(
                $"{basePath}/{checkWithdrawResponse.Value.Id}", false);

            var responseAccount = await HttpClient.FiGetTestAsync<AccountOutputModel>(
                    $"api/v1/Patika/Accounts/{depositAndWithdrawInputModel.AccountId}", false);

            // Assert
            checDepositAndWithdrawResponse.FiShouldBeSuccessStatus();
            checDepositAndWithdrawResponse.Value.ShouldNotBeNull();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task WithdrawToAccount_IfRequestedNotExist_ReturnsSuccess_WithUpdatedBalance()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<DepositAndWithdraw>();

            var depositAndWithdrawInputModel = Builder<DepositAndWithdrawInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            depositAndWithdrawInputModel.Amount = 4000;

            //Act
            var depositAndWithdrawCreateResponse = await HttpClient.FiPostTestAsync<DepositAndWithdrawInputModel, DepositAndWithdrawOutputModel>(
                $"{basePath}", depositAndWithdrawInputModel);

            var checkWithdrawResponse = await HttpClient.FiPutTestAsync<DepositAndWithdrawInputModel, DepositAndWithdrawOutputModel>(
                $"{basePath}/{depositAndWithdrawCreateResponse.Value.Id}/withdraw", depositAndWithdrawInputModel, false);

            var checDepositAndWithdrawResponse = await HttpClient.FiGetTestAsync<DepositAndWithdrawOutputModel>(
                $"{basePath}/{checkWithdrawResponse.Value.Id}", false);
            
            var responseAccount = await HttpClient.FiGetTestAsync<AccountOutputModel>(
                    $"api/v1/Patika/Accounts/{depositAndWithdrawInputModel.AccountId}", false);
            
            // Assert
            checDepositAndWithdrawResponse.FiShouldBeSuccessStatus();
            checDepositAndWithdrawResponse.Value.ShouldNotBeNull();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetDepositAndWithByAccountKey_IfRequestedItemExists_ReturnsSuccess_WithItem()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Payee>();

            var depositAndWithdrawInputModel = Builder<DepositAndWithdrawInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            //Act
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

        [Fact, Trait("Category", "Integration")]
        public async Task DepositToAccount_IfRequestedNotExist_ReturnsNotFound()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<DepositAndWithdraw>();

            var depositAndWithdrawInputModel = Builder<DepositAndWithdrawInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            depositAndWithdrawInputModel.Amount = 4000;

            int nonExistentId = 9999;

            // Act
            var response = await HttpClient.FiPutTestAsync<DepositAndWithdrawInputModel, DepositAndWithdrawOutputModel>(
                            $"{basePath}/{nonExistentId}/deposit", depositAndWithdrawInputModel, false);

            // Assert
            response.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task WithdrawToAccount_IfRequestedNotExist_ReturnsNotFound()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<DepositAndWithdraw>();

            var depositAndWithdrawInputModel = Builder<DepositAndWithdrawInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            depositAndWithdrawInputModel.Amount = 5000;

            int nonExistentId = 9999;

            // Act
            var response = await HttpClient.FiPutTestAsync<DepositAndWithdrawInputModel, DepositAndWithdrawOutputModel>(
                            $"{basePath}/{nonExistentId}/withdraw", depositAndWithdrawInputModel, false);

            // Assert
            response.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetDepositAndWithdrawByInvalidAccountKey_IfRequestedItemNotExist_ReturnsNotFound()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<DepositAndWithdraw>();

            var depositAndWithdrawInputModel = Builder<DepositAndWithdrawInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            int nonExistentId = 9999;

            // Act
            var response = await HttpClient.FiGetTestAsync<DepositAndWithdrawOutputModel>(
                                            $"{basePath}/{nonExistentId}", false);

            // Assert
            response.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task DepositAndWithdrawToAccount_IfNegativeAmount_ReturnsBadRequest()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<DepositAndWithdraw>();

            var accountModel = Builder<Account>.CreateNew()
                .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            accountModel.Id = 2;

            var depositAndWithdrawInputModel = Builder<DepositAndWithdrawInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            depositAndWithdrawInputModel.AccountId = 2;
            depositAndWithdrawInputModel.Amount = -1000;

            // Act
            var response = await HttpClient.FiPostTestAsync<DepositAndWithdrawInputModel, DepositAndWithdrawOutputModel>(
                                $"{basePath}", depositAndWithdrawInputModel);

            // Assert
            response.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task WithdrawToAccount_IfInsufficientBalance_ReturnsBadRequest()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<DepositAndWithdraw>();

            var accountModel = Builder<Account>.CreateNew()
                .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            accountModel.Id = 2;

            var depositAndWithdrawInputModel = Builder<DepositAndWithdrawInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            depositAndWithdrawInputModel.AccountId = 2;
            depositAndWithdrawInputModel.Amount = -1000;

            var withdrawResponse = await HttpClient.FiPutTestAsync<DepositAndWithdrawInputModel, DepositAndWithdrawOutputModel>(
                            $"{basePath}/{depositAndWithdrawInputModel.Id}/withdraw", depositAndWithdrawInputModel, false);

            // Assert
            withdrawResponse.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task DepositToAccount_IfInsufficientBalance_ReturnsBadRequest()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<DepositAndWithdraw>();

            var accountModel = Builder<Account>.CreateNew()
                .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            accountModel.Id = 2;

            var depositAndWithdrawInputModel = Builder<DepositAndWithdrawInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            depositAndWithdrawInputModel.AccountId = 2;
            depositAndWithdrawInputModel.Amount = -1000;

            var withdrawResponse = await HttpClient.FiPutTestAsync<DepositAndWithdrawInputModel, DepositAndWithdrawOutputModel>(
                            $"{basePath}/{depositAndWithdrawInputModel.Id}/deposit", depositAndWithdrawInputModel, false);

            // Assert
            withdrawResponse.FiShouldBeBadRequestStatus();
        }
    }
}
