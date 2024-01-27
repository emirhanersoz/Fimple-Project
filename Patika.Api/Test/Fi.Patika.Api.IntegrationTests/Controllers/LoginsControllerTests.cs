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
        private HelperMethodsForTests helperMethodsForTests;

        private readonly ITestOutputHelper output;

        public LoginsControllerTests(ITestOutputHelper output, PatikaApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
        {
            this.output = output;
            helperMethodsForTests = new HelperMethodsForTests(fiTestApplicationFactory);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task CreateLogin_IfUserExists_ReturnsSuccess_WithItem()
        {
            //Arrange
            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

            var userInputModel = Builder<UserInputModel>.CreateNew()
                    .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray).With(p => p.Id = 1)
                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var loginInputModel = Builder<LoginInputModel>.CreateNew() 
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            loginInputModel.UserId = 1;

            //Act
            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var loginCreateResponse = await HttpClient.FiPostTestAsync<LoginInputModel, LoginOutputModel>(
                $"{basePath}", loginInputModel);

            //Assert
            loginCreateResponse.FiShouldBeSuccessStatus();
            loginCreateResponse.Value.ShouldNotBeNull();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task UpdateLogin_WhenCalled_ReturnsSuccess_WithUpdatedItem()
        {
            // Arrange
            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

            var userInputModel = Builder<UserInputModel>.CreateNew()
                    .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray).With(p => p.Id = 1)
                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var loginInputModel = Builder<LoginInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            loginInputModel.UserId = 1;

            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var loginCreateResponse = await HttpClient.FiPostTestAsync<LoginInputModel, LoginOutputModel>(
                $"{basePath}", loginInputModel);
            loginInputModel.LoginTime = DateTimeHelper.UtcNow.AddDays(1);

            // Act
            var response = await HttpClient.FiPutTestAsync<LoginInputModel?, LoginOutputModel>(
                                            $"{basePath}/{loginCreateResponse.Value.Id}", loginInputModel, false);

            // Assert
            response.FiShouldBeSuccessStatus();
            response.Value.ShouldNotBeNull();

            output.WriteLine(response.Value.LoginTime.ToString());
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetLoginById_IfRequestedItemExist_ReturnsSuccess_WithItem()
        {
            //Arrange
            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

            var userInputModel = Builder<UserInputModel>.CreateNew()
                    .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray).With(p => p.Id = 1)
                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var loginInputModel = Builder<LoginInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            loginInputModel.UserId = 1;

            //Act
            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var loginCreateResponse = await HttpClient.FiPostTestAsync<LoginInputModel, LoginOutputModel>(
                $"{basePath}", loginInputModel);

            var checkUserResponse = await HttpClient.FiGetTestAsync<UserOutputModel>(
                 $"api/v1/Patika/Users/{userCreateResponse.Value.Id}", false);

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
            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

            var userInputModel = Builder<UserInputModel>.CreateNew()
                    .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray).With(p => p.Id = 1)
                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var loginInputModel = Builder<LoginInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            loginInputModel.UserId = 1;

            //Act
            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var loginCreateResponse = await HttpClient.FiPostTestAsync<LoginInputModel, LoginOutputModel>(
                $"{basePath}", loginInputModel);

            var checkUserResponse = await HttpClient.FiGetTestAsync<UserOutputModel>(
                 $"api/v1/Patika/Users/{userCreateResponse.Value.Id}", false);

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
            //Arrange
            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

            var userInputModel = Builder<UserInputModel>.CreateNew()
                    .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray).With(p => p.Id = 1)
                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var loginInputModel = Builder<LoginInputModel>.CreateNew()
                     .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
            loginInputModel.UserId = 1;

            //Act
            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var loginCreateResponse = await HttpClient.FiPostTestAsync<LoginInputModel, LoginOutputModel>(
                $"{basePath}", loginInputModel);

            // Act
            var response = await HttpClient.FiDeleteTestAsync(
                                            $"{basePath}/{loginCreateResponse.Value.Id}");

            // Assert
            response.FiShouldBeSuccessStatus();
            TestDbContext.Set<Login>().FirstOrDefault(p => p.Id == loginCreateResponse.Value.Id).ShouldBeNull();
        }
    }
}
