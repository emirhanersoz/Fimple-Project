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
        public async Task CreateSupportRequest_IfRequestedCustomerAndUserExist_ReturnsSuccess_WithCreatedRequest()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<SupportRequest>();

            var supportRequestInputModel = Builder<SupportRequestInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            supportRequestInputModel.CustomerId = 1;

            //Act
            var supportRequestCreateResponse = await HttpClient.FiPostTestAsync<SupportRequestInputModel, SupportRequestOutputModel>(
                $"{basePath}", supportRequestInputModel);

            //Assert
            supportRequestCreateResponse.FiShouldBeSuccessStatus();
            supportRequestCreateResponse.Value.ShouldNotBeNull();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetSupportRequestByKey_IfRequestedItemExists_ReturnsSuccess_WithItem()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<SupportRequest>();

            var supportRequestInputModel = Builder<SupportRequestInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            supportRequestInputModel.CustomerId = 1;

            var supportRequestCreateResponse = await HttpClient.FiPostTestAsync<SupportRequestInputModel, SupportRequestOutputModel>(
                $"{basePath}", supportRequestInputModel);

            //Act
            var checkSupportRequestResponse = await HttpClient.FiGetTestAsync<SupportRequestOutputModel>(
                $"{basePath}/{supportRequestCreateResponse.Value.Id}", false);

            // Assert
            checkSupportRequestResponse.FiShouldBeSuccessStatus();
            checkSupportRequestResponse.Value.ShouldNotBeNull();
            checkSupportRequestResponse.Value.Id.ShouldEqual(checkSupportRequestResponse.Value.Id);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task AnsweredSupportRequest_WhenCalled_ReturnsSuccess_WithUpdatedRequest()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<SupportRequest>();

            var supportRequestInputModel = Builder<SupportRequestInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            supportRequestInputModel.CustomerId = 1;

            var supportRequestCreateResponse = await HttpClient.FiPostTestAsync<SupportRequestInputModel, SupportRequestOutputModel>(
                $"{basePath}", supportRequestInputModel);

            // Act
            var response = await HttpClient.FiPutTestAsync<SupportRequestInputModel?, SupportRequestOutputModel>(
                                            $"{basePath}/{supportRequestCreateResponse.Value.Id}", supportRequestInputModel, false);

            var checkSupportRequestResponse = await HttpClient.FiGetTestAsync<SupportRequestOutputModel>(
                $"{basePath}/{supportRequestCreateResponse.Value.Id}", false);

            checkSupportRequestResponse.FiShouldBeSuccessStatus();
            checkSupportRequestResponse.Value.ShouldNotBeNull();
            checkSupportRequestResponse.Value.Answered.ShouldEqual("Answered1");
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetAllRequestsWithUserId_IfItemsExist_ReturnSuccess_WithList()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<SupportRequest>();

            var supportRequestInputModel = Builder<SupportRequestInputModel>.CreateNew()
                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            supportRequestInputModel.CustomerId = 1;

            var supportRequestCreateResponse = await HttpClient.FiPostTestAsync<SupportRequestInputModel, SupportRequestOutputModel>(
                $"{basePath}", supportRequestInputModel);

            //Act
            var checkLoginResponse = await HttpClient.FiGetTestAsync<List<SupportRequestOutputModel>>(
                 $"{basePath}/ByParameters", false);

            // Assert
            checkLoginResponse.FiShouldBeSuccessStatus();
            checkLoginResponse.Value.ShouldNotBeNull();
            checkLoginResponse.Value.Count.ShouldBeGreaterThan(0);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task DeleteRequestByKey_WhenCalled_ReturnsSuccess()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<SupportRequest>();

            var supportRequestInputModel = Builder<SupportRequestInputModel>.CreateNew()
                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            supportRequestInputModel.CustomerId = 1;

            var supportRequestCreateResponse = await HttpClient.FiPostTestAsync<SupportRequestInputModel, SupportRequestOutputModel>(
                $"{basePath}", supportRequestInputModel);

            // Act
            var response = await HttpClient.FiDeleteTestAsync(
                                            $"{basePath}/{supportRequestCreateResponse.Value.Id}");

            // Assert
            response.FiShouldBeSuccessStatus();
            TestDbContext.Set<SupportRequest>().FirstOrDefault(p => p.Id == supportRequestCreateResponse.Value.Id).ShouldBeNull();
        }
    }
}
