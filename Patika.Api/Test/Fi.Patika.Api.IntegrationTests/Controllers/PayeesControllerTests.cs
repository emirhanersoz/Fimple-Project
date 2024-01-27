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
        private readonly ITestOutputHelper output;
        private HelperMethodsForTests helperMethodsForTests;

        public PayeesControllerTests(ITestOutputHelper output, PatikaApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
        {
            this.output = output;
            helperMethodsForTests = new HelperMethodsForTests(fiTestApplicationFactory);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task CreatePayee_IfRequestedCustomerAndAccountExist_ReturnsSuccess_WithCreatedPayee()
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

            var payeeInputModel = Builder<PayeeInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            payeeInputModel.AccountId = 1;

            //Act
            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
                "api/v1/Patika/Customers", customerInputModel);

            var accountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                "api/v1/Patika/Accounts", accountInputModel);

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
            const decimal balance = 1000;
            const decimal payeeAmount = 100;

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

            var payeeInputModel = Builder<PayeeInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            payeeInputModel.isPayment = false;
            payeeInputModel.AccountId = 1;
            payeeInputModel.Amount = payeeAmount;

            //Act
            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
                "api/v1/Patika/Customers", customerInputModel);

            var accountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                "api/v1/Patika/Accounts", accountInputModel);

            var payeeCreateResponse = await HttpClient.FiPostTestAsync<PayeeInputModel, PayeeOutputModel>(
                $"{basePath}", payeeInputModel);

            var response = await HttpClient.FiPutTestAsync<PayeeInputModel, PayeeOutputModel>(
                                            $"{basePath}/Payment/{payeeCreateResponse.Value.Id}", payeeInputModel, false);

            var checkUpdatedAccount = await HttpClient.FiGetTestAsync<AccountOutputModel>(
                    $"api/v1/Patika/Accounts/{accountCreateResponse.Value.Id}", false);

            // Assert
            response.FiShouldBeSuccessStatus();
            response.Value.ShouldNotBeNull();
            response.Value.isPayment.ShouldEqual(true);
            checkUpdatedAccount.Value.Balance.ShouldEqual(balance - payeeAmount);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task UpdatePayee_WhenCalled_ReturnsSuccess_WithUpdatedPayee()
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

            var payeeInputModel = Builder<PayeeInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            payeeInputModel.AccountId = 1;

            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
                "api/v1/Patika/Customers", customerInputModel);

            var accountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                "api/v1/Patika/Accounts", accountInputModel);

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

            var payeeInputModel = Builder<PayeeInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            payeeInputModel.AccountId = 1;

            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
                "api/v1/Patika/Customers", customerInputModel);

            var accountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                "api/v1/Patika/Accounts", accountInputModel);

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

            var payeeInputModel = Builder<PayeeInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            payeeInputModel.AccountId = 1;

            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
                "api/v1/Patika/Customers", customerInputModel);

            var accountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                "api/v1/Patika/Accounts", accountInputModel);

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
    }
}
