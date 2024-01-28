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
    public class PayeesControllerTests : PatikaScenariosBase
    {
        private const string basePath = "api/v1/Patika/Payees";

        public PayeesControllerTests(ITestOutputHelper output, PatikaApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
        {
        }

        [Fact, Trait("Category", "Integration")]
        public async Task CreatePayee_IfRequestedCustomerAndAccountExist_ReturnsSuccess_WithCreatedPayee()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Payee>();

            var payeeInputModel = Builder<PayeeInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            payeeInputModel.AccountId = 1;

            //Act
            var payeeCreateResponse = await HttpClient.FiPostTestAsync<PayeeInputModel, PayeeOutputModel>(
                $"{basePath}", payeeInputModel);

            // Assert
            payeeCreateResponse.FiShouldBeSuccessStatus();
            payeeCreateResponse.Value.ShouldNotBeNull();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task PaymentPayee_IfRequestedPayeeExist_ReturnsSuccess_WithUpdatedPayment()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Payee>();

            var payeeInputModel = Builder<PayeeInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            payeeInputModel.Amount = 1000;
            
            //Act
            var payeeCreateResponse = await HttpClient.FiPostTestAsync<PayeeInputModel, PayeeOutputModel>(
                $"{basePath}", payeeInputModel);

            var response = await HttpClient.FiPutTestAsync<PayeeInputModel, PayeeOutputModel>(
                                            $"{basePath}/Payment/{payeeCreateResponse.Value.Id}", payeeInputModel, false);

            var responseAccount = await HttpClient.FiGetTestAsync<AccountOutputModel>(
                $"api/v1/Patika/Accounts/{response.Value.AccountId}");

            // Assert
            response.FiShouldBeSuccessStatus();
            response.Value.ShouldNotBeNull();
            response.Value.isPayment.ShouldEqual(true);

            var temp = await TestDbContext.FindAsync<Account>(response.Value.AccountId);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task UpdatePayee_WhenCalled_ReturnsSuccess_WithUpdatedPayee()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Payee>();

            var payeeInputModel = Builder<PayeeInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            payeeInputModel.AccountId = 1;

            var payeeCreateResponse = await HttpClient.FiPostTestAsync<PayeeInputModel, PayeeOutputModel>(
                $"{basePath}", payeeInputModel);

            payeeInputModel.PayeeType = PayeeType.Internet;

            // Act
            var response = await HttpClient.FiPutTestAsync<PayeeInputModel?, PayeeOutputModel>(
                                            $"{basePath}/{payeeCreateResponse.Value.Id}", payeeInputModel, false);

            // Assert
            response.FiShouldBeSuccessStatus();
            response.Value.ShouldNotBeNull();
            response.Value.PayeeType.ShouldEqual(PayeeType.Internet);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task DeletePayeeByKey_WhenCalled_ReturnsSuccess()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Payee>();

            var payeeInputModel = Builder<PayeeInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            payeeInputModel.AccountId = 1;

            var payeeCreateResponse = await HttpClient.FiPostTestAsync<PayeeInputModel, PayeeOutputModel>(
                $"{basePath}", payeeInputModel);

            // Act
            var response = await HttpClient.FiDeleteTestAsync(
                                            $"{basePath}/{payeeCreateResponse.Value.Id}");

            // Assert
            response.FiShouldBeSuccessStatus();
            TestDbContext.Set<Payee>().FirstOrDefault(p => p.Id == payeeCreateResponse.Value.Id).ShouldBeNull();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetPayeeByAccountKey_IfRequestedItemExists_ReturnsSuccess_WithItem()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Payee>();

            var payeeInputModel = Builder<PayeeInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            payeeInputModel.AccountId = 1;

            var payeeCreateResponse = await HttpClient.FiPostTestAsync<PayeeInputModel, PayeeOutputModel>(
                $"{basePath}", payeeInputModel);

            // Act
            var response = await HttpClient.FiGetTestAsync<PayeeOutputModel>(
                                            $"{basePath}/{payeeCreateResponse.Value.Id}", false);

            // Assert
            response.FiShouldBeSuccessStatus();
            response.Value.ShouldNotBeNull();
            response.Value.Id.ShouldEqual(payeeCreateResponse.Value.Id);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task CreatePayee_IfRequestedCustomerAndAccountNotExist_ReturnsBadRequest()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Payee>();

            var payeeInputModel = Builder<PayeeInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            payeeInputModel.AccountId = 9999;

            //Act
            var payeeCreateResponse = await HttpClient.FiPostTestAsync<PayeeInputModel, PayeeOutputModel>(
                $"{basePath}", payeeInputModel);

            // Assert
            payeeCreateResponse.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task PaymentPayee_IfNotEnoughBalanceAccount_ReturnsBadRequest()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Payee>();

            var payeeInputModel = Builder<PayeeInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            payeeInputModel.AccountId = 1;

            // Act
            var response = await HttpClient.FiPutTestAsync<PayeeInputModel, PayeeOutputModel>(
                                            $"{basePath}/Payment/{payeeInputModel.Id}", new PayeeInputModel { Amount = 10000000 }, false);

            // Assert
            response.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task PaymentPayee_IfisPaymentTrue_ReturnsBadRequest()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Payee>();

            var payeeInputModel = Builder<PayeeInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            payeeInputModel.AccountId = 1;

            // Act
            var response = await HttpClient.FiPutTestAsync<PayeeInputModel, PayeeOutputModel>(
                                            $"{basePath}/Payment/{payeeInputModel.Id}", new PayeeInputModel { isPayment = true }, false);

            // Assert
            response.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task UpdateNonexistentPayee_WhenCalled_ReturnsBadRequest()
        {
            // Arrange
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Payee>();

            var payeeInputModel = Builder<PayeeInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            payeeInputModel.AccountId = 1;

            int nonExistentPayeeId = 9999;
            // Act
            var response = await HttpClient.FiPutTestAsync<PayeeInputModel?, PayeeOutputModel>(
                                            $"{basePath}/{nonExistentPayeeId}", new PayeeInputModel(), false);

            // Assert
            response.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task DeleteNonexistentPayee_WhenCalled_ReturnsBadRequest()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Payee>();

            var payeeInputModel = Builder<PayeeInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            payeeInputModel.AccountId = 1;

            int nonExistentPayeeId = 9999; // Varolmayan bir payee ID'si

            // Act
            var response = await HttpClient.FiDeleteTestAsync($"{basePath}/{nonExistentPayeeId}");

            // Assert
            response.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetNonexistentPayeeByKey_IfRequestedItemNotExist_ReturnsBadRequest()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Payee>();

            var payeeInputModel = Builder<PayeeInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            payeeInputModel.AccountId = 1;

            int nonExistentPayeeId = 9999;

            // Act
            var response = await HttpClient.FiGetTestAsync<PayeeOutputModel>($"{basePath}/{nonExistentPayeeId}", false);

            // Assert
            response.FiShouldBeBadRequestStatus();
        }
    }
}
