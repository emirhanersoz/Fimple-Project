using Fi.Infra.Utility;
using Fi.Patika.Api.Domain.Entity;
using Fi.Patika.Api.IntegrationTests.Initialization;
using Fi.Patika.Schema.Model;
using Fi.Test.Extensions;
using FizzWare.NBuilder;
using Should;
using StructureMap.Diagnostics.TreeView;
using Xunit;
using Xunit.Abstractions;

namespace Fi.Patika.Api.IntegrationTests.Controllers
{
    public class CreditsControllerTests : PatikaScenariosBase
    {
        private const string basePath = "api/v1/Patika/Credits";

        public CreditsControllerTests(ITestOutputHelper output, PatikaApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
        {
        }

        [Fact, Trait("Category", "Integration")]
        public async Task CreateCredit_IfRequestedCreditNotExist_ReturnsSuccess_WithItem()
        {
           
            // Arrange
            var inputModel = Builder<CreditInputModel>.CreateNew()
                .With(p => p.TotalAmount = 10000).With(p => p.MontlyPayment = 1000)
                .With(p => p.RepaymentPeriodMonths = 10)
                .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            inputModel.LoanDate = DateTimeHelper.UtcNow;

            // Act
            var responsePost = await HttpClient.FiPostTestAsync<CreditInputModel, CreditOutputModel>(
                                            $"{basePath}", inputModel);

            var response = await HttpClient.FiPostTestAsync(
                                            $"{basePath}/{responsePost.Value.Id}");

            // Assert
            responsePost.FiShouldBeSuccessStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task LoanCreditForAccount_IfRequestedCreditAndAccountNotExist_ReturnsSuccess_WithItem()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<AccountCredit>();

            var creditInputModel = Builder<CreditInputModel>.CreateNew()
               .With(p => p.TotalAmount = 10000).With(p => p.MontlyPayment = 1000)
               .With(p => p.RepaymentPeriodMonths = 10)
               .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            creditInputModel.Id = 1;

            var accountCreditInputModel = Builder<AccountCreditInputModel>.CreateNew()
               .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            accountCreditInputModel.AccountId = 1;
            accountCreditInputModel.CreditId = 1;

            var creditCreateResponsePost = await HttpClient.FiPostTestAsync<CreditInputModel, CreditOutputModel>(
                $"{basePath}", creditInputModel);

            var accountCreditCreateResponsePost = await HttpClient.FiPostTestAsync<AccountCreditInputModel, AccountCreditOutputModel>(
                "api/v1/Patika/AccountCredits", accountCreditInputModel);

            //Act
            var loanCreditResponsePost = await HttpClient.FiPostTestAsync<AccountCreditInputModel, AccountCreditOutputModel>(
               $"{basePath}/getCredit", accountCreditInputModel);

            // Assert
            accountCreditCreateResponsePost.FiShouldBeSuccessStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task DeleteCreditByKey_WhenCalled_ReturnsSuccess()
        {
            // Arrange
            var inputModel = Builder<CreditInputModel>.CreateNew()
                .With(p => p.TotalAmount = 10000).With(p => p.MontlyPayment = 1000)
                .With(p => p.RepaymentPeriodMonths = 10)
                .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            inputModel.LoanDate = DateTimeHelper.UtcNow;

            var responsePost = await HttpClient.FiPostTestAsync<CreditInputModel, CreditOutputModel>(
                                            $"{basePath}", inputModel);

            // Act
            var response = await HttpClient.FiDeleteTestAsync(
                                            $"{basePath}/{responsePost.Value.Id}");

            // Assert
            response.FiShouldBeSuccessStatus();
            TestDbContext.Set<Credit>().FirstOrDefault(p => p.Id == responsePost.Value.Id).ShouldBeNull();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetCreditByKey_IfRequestedItemExists_ReturnsSuccess_WithItem()
        {
            // Arrange
            var inputModel = Builder<CreditInputModel>.CreateNew()
                .With(p => p.TotalAmount = 10000).With(p => p.MontlyPayment = 1000)
                .With(p => p.RepaymentPeriodMonths = 10)
                .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            inputModel.LoanDate = DateTimeHelper.UtcNow;

            var responsePost = await HttpClient.FiPostTestAsync<CreditInputModel, CreditOutputModel>(
                                            $"{basePath}", inputModel);

            // Act
            var response = await HttpClient.FiGetTestAsync<CreditOutputModel>(
                                            $"{basePath}/{responsePost.Value.Id}", false);

            // Assert
            response.FiShouldBeSuccessStatus();
            response.Value.ShouldNotBeNull();
            response.Value.Id.ShouldEqual(responsePost.Value.Id);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetCreditsByParameters_IfItemsExist_ReturnsSuccess_WithList()
        {
            // Arrange
            var inputModel = Builder<CreditInputModel>.CreateNew()
                .With(p => p.TotalAmount = 10000).With(p => p.MontlyPayment = 1000)
                .With(p => p.RepaymentPeriodMonths = 10)
                .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            inputModel.LoanDate = DateTimeHelper.UtcNow;

            var responsePost = await HttpClient.FiPostTestAsync<CreditInputModel, CreditOutputModel>(
                                            $"{basePath}", inputModel);

            // Act
            var response = await HttpClient.FiGetTestAsync<List<CreditOutputModel>>(
                                            $"{basePath}/ByParameters", false);

            // Assert
            response.FiShouldBeSuccessStatus();
            response.Value.ShouldNotBeNull();
            response.Value.Count.ShouldBeGreaterThan(0);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task UpdateNonexistentCredit_WhenCalled_ReturnsBadRequestStatus()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<Credit>();

            var creditInputModel = Builder<CreditInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            creditInputModel.Id = 1;

            int nonExistentCreditId = 9999;

            // Act
            var response = await HttpClient.FiPutTestAsync<CreditInputModel?, CreditOutputModel>(
                                            $"{basePath}/{nonExistentCreditId}", new CreditInputModel(), false);

            // Assert
            response.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task DeleteNonexistentCredit_WhenCalled_ReturnsBadRequestStatus()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<Credit>();

            var creditInputModel = Builder<CreditInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            creditInputModel.Id = 1;

            int nonExistentCreditId = 9999;

            // Act
            var response = await HttpClient.FiDeleteTestAsync($"{basePath}/{nonExistentCreditId}");

            // Assert
            response.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetNonexistentCreditByKey_IfRequestedItemNotExist_ReturnsBadRequestStatus()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<Credit>();

            var creditInputModel = Builder<CreditInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            creditInputModel.Id = 1;

            int nonExistentCreditId = 9999;

            // Act
            var response = await HttpClient.FiGetTestAsync<CreditOutputModel>($"{basePath}/{nonExistentCreditId}", false);

            // Assert
            response.FiShouldBeBadRequestStatus();
        }
    }
}
