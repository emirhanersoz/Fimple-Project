using Fi.Patika.Api.IntegrationTests.Initialization;
using Fi.Patika.Schema.Model;
using Fi.Test.Extensions;
using FizzWare.NBuilder;
using Should;
using Xunit;
using Xunit.Abstractions;

namespace Fi.Patika.Api.IntegrationTests.Controllers
{
    public class SupportRequestsControllerTests : PatikaScenariosBase
    {
        private const string basePath = "api/v1/Patika/SupportRequests";
        private readonly ITestOutputHelper output;
        private HelperMethodsForTests helperMethodsForTests;

        public SupportRequestsControllerTests(ITestOutputHelper output, PatikaApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
        {
            this.output = output;
            helperMethodsForTests = new HelperMethodsForTests(fiTestApplicationFactory);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task CreateSupportRequest_RequestedNotExist_ReturnsSuccess_WithItem()
        {
            //Arrange
            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

            var userInputModel = Builder<UserInputModel>.CreateNew()
                    .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray).With(p => p.Id = 1)
                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var customerInputModel = Builder<CustomerInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            customerInputModel.UserId = 1;

            var supportRequestInputModel = Builder<SupportRequestInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            supportRequestInputModel.CustomerId = 1;

            //Act
            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
                 "api/v1/Patika/Customers", customerInputModel);

            var supportRequestCreateResponse = await HttpClient.FiPostTestAsync<SupportRequestInputModel, SupportRequestOutputModel>(
                $"{basePath}", supportRequestInputModel);

            var checkUserResponse = await HttpClient.FiGetTestAsync<UserOutputModel>(
                 $"api/v1/Patika/Users/{userCreateResponse.Value.Id}", false);

            var checkCustomerResponse = await HttpClient.FiGetTestAsync<CustomerOutputModel>(
                "api/v1/Patika/Customers/{customerCreateResponse.Value.Id}", false);

            var checkSupportRequestResponse = await HttpClient.FiGetTestAsync<SupportRequestOutputModel>(
                $"{basePath}/{supportRequestCreateResponse.Value.Id}", false);

            //Assert
            checkSupportRequestResponse.FiShouldBeSuccessStatus();
            checkSupportRequestResponse.Value.ShouldNotBeNull();
        }
    }
}
