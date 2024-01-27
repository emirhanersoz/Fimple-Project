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
        private HelperMethodsForTests helperMethodsForTests;
        private readonly ITestOutputHelper output;

        public CreditsControllerTests(ITestOutputHelper output, PatikaApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
        {
            this.output = output;
            helperMethodsForTests = new HelperMethodsForTests(fiTestApplicationFactory);
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
            output.WriteLine(inputModel.LoanDate.ToString());
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetCreditForAccount_IfRequestedCreditAndAccountNotExist_ReturnsSuccess_WithItem()
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

            var creditInputModel = Builder<CreditInputModel>.CreateNew()
               .With(p => p.TotalAmount = 10000).With(p => p.MontlyPayment = 1000)
               .With(p => p.RepaymentPeriodMonths = 10)
               .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            creditInputModel.Id = 1;

            var accountCreditInputModel = Builder<AccountCreditInputModel>.CreateNew()
               .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            accountCreditInputModel.AccountId = 1;
            accountCreditInputModel.CreditId = 1;

            //Act
            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
                "api/v1/Patika/Customers", customerInputModel);

            var accountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                "api/v1/Patika/Accounts", accountInputModel);

            var creditCreateResponsePost = await HttpClient.FiPostTestAsync<CreditInputModel, CreditOutputModel>(
                $"{basePath}", creditInputModel);

            var accountCreditCreateResponsePost = await HttpClient.FiPostTestAsync<AccountCreditInputModel, AccountCreditOutputModel>(
                "api/v1/Patika/AccountCredits", accountCreditInputModel);

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
    }
}
