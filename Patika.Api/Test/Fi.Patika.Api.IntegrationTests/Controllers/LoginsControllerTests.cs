using Fi.Patika.Api.IntegrationTests.Initialization;
using Fi.Patika.Schema.Model;
using Fi.Test.Extensions;
using FizzWare.NBuilder;
using Xunit.Abstractions;
using Xunit;
using Should;
using Newtonsoft.Json;
using Azure;
using Fi.Infra.Utility;
using Fi.Patika.Api.Domain.Entity;

namespace Fi.Patika.Api.IntegrationTests.Controllers
{
    public class LoginsControllerTests : PatikaScenariosBase
    {
        private const string basePath = "api/v1/Patika/Logins";

        public LoginsControllerTests(ITestOutputHelper output, PatikaApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
        {
        }

        [Fact, Trait("Category", "Integration")]
        public async Task CreateLogin_IfUserExists_ReturnsSuccess_WithItem()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Login>();

            var loginInputModel = Builder<LoginInputModel>.CreateNew() 
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            loginInputModel.UserId = 1;

            //Act
            var loginCreateResponse = await HttpClient.FiPostTestAsync<LoginInputModel, LoginOutputModel>(
                $"{basePath}", loginInputModel);

            //Assert
            loginCreateResponse.FiShouldBeSuccessStatus();
            loginCreateResponse.Value.ShouldNotBeNull();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task UpdateLogin_WhenCalled_ReturnsSuccess_WithUpdatedItem()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Login>();

            var loginInputModel = Builder<LoginInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            loginInputModel.UserId = 1;

            //Act
            var loginCreateResponse = await HttpClient.FiPostTestAsync<LoginInputModel, LoginOutputModel>(
                $"{basePath}", loginInputModel);
            loginInputModel.LoginTime = DateTimeHelper.UtcNow.AddDays(1);

            // Act
            var response = await HttpClient.FiPutTestAsync<LoginInputModel?, LoginOutputModel>(
                                            $"{basePath}/{loginCreateResponse.Value.Id}", loginInputModel, false);

            // Assert
            response.FiShouldBeSuccessStatus();
            response.Value.ShouldNotBeNull();
            response.Value.Equals(loginInputModel.LoginTime);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetLoginById_IfRequestedItemExist_ReturnsSuccess_WithItem()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Login>();

            var loginInputModel = Builder<LoginInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            loginInputModel.UserId = 1;

            //Act
            var loginCreateResponse = await HttpClient.FiPostTestAsync<LoginInputModel, LoginOutputModel>(
                $"{basePath}", loginInputModel);

            var checkLoginResponse = await HttpClient.FiGetTestAsync<LoginOutputModel>(
                 $"{basePath}/{loginCreateResponse.Value.Id}", false);

            // Assert
            checkLoginResponse.FiShouldBeSuccessStatus();
            checkLoginResponse.Value.ShouldNotBeNull();
            checkLoginResponse.Value.Id.ShouldEqual(checkLoginResponse.Value.Id);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetAllLoginsWithUserId_IfItemsExist_ReturnSuccess_WithList()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Login>();

            var loginInputModel = Builder<LoginInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            loginInputModel.UserId = 1;

            //Act
            var loginCreateResponse = await HttpClient.FiPostTestAsync<LoginInputModel, LoginOutputModel>(
                $"{basePath}", loginInputModel);

            var checkLoginResponse = await HttpClient.FiGetTestAsync<List<LoginOutputModel>>(
                 $"{basePath}/ByParameters", false);

            // Assert
            checkLoginResponse.FiShouldBeSuccessStatus();
            checkLoginResponse.Value.ShouldNotBeNull();
            checkLoginResponse.Value.Count.ShouldBeGreaterThan(0);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task DeleteLoginsByKey_WhenCalled_ReturnsSuccess()
        {
            ///Arrange
            await TestDbContext.EnsureEntityIsEmpty<Login>();

            var loginInputModel = Builder<LoginInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            loginInputModel.UserId = 1;

            var loginCreateResponse = await HttpClient.FiPostTestAsync<LoginInputModel, LoginOutputModel>(
                $"{basePath}", loginInputModel);

            // Act
            var response = await HttpClient.FiDeleteTestAsync(
                                            $"{basePath}/{loginCreateResponse.Value.Id}");

            // Assert
            response.FiShouldBeSuccessStatus();
            TestDbContext.Set<Login>().FirstOrDefault(p => p.Id == loginCreateResponse.Value.Id).ShouldBeNull();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task CreateLogin_IfNotFoundUser_ReturnsBadRequest()
        {
            //Arrange
            await TestDbContext.EnsureEntityIsEmpty<Login>();

            var invalidModel = Builder<LoginInputModel>.CreateNew()
                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            invalidModel.UserId = 9999;

            // Act
            var response = await HttpClient.FiPostTestAsync<LoginInputModel, LoginOutputModel>(
                                $"{basePath}", invalidModel);

            // Assert
            response.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task UpdateNonexistentLogin_WhenCalled_ReturnsBadRequestStatus()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<Login>();

            var loginInputModel = Builder<LoginInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            loginInputModel.UserId = 1;

            int nonExistentloginId = 9999;

            // Act
            var response = await HttpClient.FiPutTestAsync<LoginInputModel?, LoginOutputModel>(
                                            $"{basePath}/{nonExistentloginId}", new LoginInputModel(), false);

            // Assert
            response.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task DeleteNonexistentLogin_WhenCalled_ReturnsBadRequestStatus()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<Login>();

            var loginInputModel = Builder<LoginInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
           loginInputModel.UserId = 1;

            int nonExistentLoginId = 9999;

            // Act
            var response = await HttpClient.FiDeleteTestAsync($"{basePath}/{nonExistentLoginId}");

            // Assert
            response.FiShouldBeBadRequestStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetNonexistentLoginByKey_IfRequestedItemNotExist_ReturnsBadRequestStatus()
        {
            // Arrange
            await TestDbContext.EnsureEntityIsEmpty<Login>();

            var loginInputModel = Builder<LoginInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            loginInputModel.UserId = 1;

            int nonExistentLoginId = 9999;

            // Act
            var response = await HttpClient.FiGetTestAsync<LoginOutputModel>($"{basePath}/{nonExistentLoginId}", false);

            // Assert
            response.FiShouldBeBadRequestStatus();
        }
    }
}
