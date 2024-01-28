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
            await TestDbContext.EnsureEntityIsEmpty<DepositAndWithdraw>();

            var depositAndWithdrawInputModel = Builder<DepositAndWithdrawInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            depositAndWithdrawInputModel.Amount = 4000;

            //Act
            var depositAndWithdrawCreateResponse = await HttpClient.FiPostTestAsync<DepositAndWithdrawInputModel, DepositAndWithdrawOutputModel>(
                $"{basePath}", depositAndWithdrawInputModel);

            var checkDepositResponse = await HttpClient.FiPutTestAsync<DepositAndWithdrawInputModel, DepositAndWithdrawOutputModel>(
                $"{basePath}/{depositAndWithdrawInputModel.Id}/deposit", depositAndWithdrawInputModel, false);

            var checDepositAndWithdrawResponse = await HttpClient.FiGetTestAsync<DepositAndWithdrawOutputModel>(
                $"{basePath}/{depositAndWithdrawCreateResponse.Value.Id}", false);

            var responseAccount = await HttpClient.FiGetTestAsync<AccountOutputModel>(
               $"api/v1/Patika/Accounts/{depositAndWithdrawInputModel.AccountId}", false);
            // Assert
            checDepositAndWithdrawResponse.FiShouldBeSuccessStatus();
            checDepositAndWithdrawResponse.Value.ShouldNotBeNull();

            output.WriteLine(responseAccount.Value.Balance.ToString());
        }

        [Fact, Trait("Category", "Integration")]
        public async Task WithdrawToAccount_IfRequestedNotExist_ReturnsSuccess_WithUpdatedBalance()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<DepositAndWithdraw>();

            var depositAndWithdrawInputModel = Builder<DepositAndWithdrawInputModel>.CreateNew()
                     .With(p => p.AccountId = 2)
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            depositAndWithdrawInputModel.Amount = 5000;

            //Act
            var depositAndWithdrawCreateResponse = await HttpClient.FiPostTestAsync<DepositAndWithdrawInputModel, DepositAndWithdrawOutputModel>(
                $"{basePath}", depositAndWithdrawInputModel);

            var checkWithdrawResponse = await HttpClient.FiPutTestAsync<DepositAndWithdrawInputModel, DepositAndWithdrawOutputModel>(
                $"{basePath}/{depositAndWithdrawInputModel.Id}/withdraw", depositAndWithdrawInputModel, false);

            var checDepositAndWithdrawResponse = await HttpClient.FiGetTestAsync<DepositAndWithdrawOutputModel>(
                $"{basePath}/{depositAndWithdrawCreateResponse.Value.Id}", false);
            
            var responseAccount = await HttpClient.FiGetTestAsync<AccountOutputModel>(
                    $"api/v1/Patika/Accounts/{depositAndWithdrawInputModel.AccountId}", false);
            
            // Assert
            checDepositAndWithdrawResponse.FiShouldBeSuccessStatus();
            checDepositAndWithdrawResponse.Value.ShouldNotBeNull();

            output.WriteLine(responseAccount.Value.Balance.ToString());

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
    }
}
