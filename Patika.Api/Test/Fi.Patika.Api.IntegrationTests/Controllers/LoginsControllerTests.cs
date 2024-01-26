using Fi.Patika.Api.IntegrationTests.Initialization;
using Fi.Patika.Schema.Model;
using Fi.Test.Extensions;
using FizzWare.NBuilder;
using Xunit.Abstractions;
using Xunit;
using Should;
using Newtonsoft.Json;

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
        public async Task CreateLogin_IfRequestedNotExist_ReturnsSuccess_WithItem()
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
                "api/v1/Patika/Logins", loginInputModel);

            var checkUserResponse = await HttpClient.FiGetTestAsync<UserOutputModel>(
                 $"api/v1/Patika/Users/{userCreateResponse.Value.Id}", false);

            var checkLoginResponse = await HttpClient.FiGetTestAsync<LoginOutputModel>(
                 $"api/v1/Patika/Logins/{loginCreateResponse.Value.Id}", false);

            //Assert
            checkLoginResponse.FiShouldBeSuccessStatus();
            checkLoginResponse.Value.ShouldNotBeNull();
        }
    }
}
