using Fi.Patika.Schema.Model;
using FizzWare.NBuilder;
using System.Security.Cryptography;
using Fi.Patika.Api.Domain.Entity;
using Fi.Patika.Api.IntegrationTests.Initialization;
using Fi.Test.Extensions;
using Newtonsoft.Json;
using Should;
using Xunit;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using Fi.ApiBase.Controller;

namespace Fi.Patika.Api.IntegrationTests.Controllers
{
    public class HelperMethodsForTests : PatikaScenariosBase
    {
        public HelperMethodsForTests(PatikaApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
        {
        }

        public byte[] GeneratorByteCodes()
        {
            int byteCount = 16;
            byte[] byteArray = new byte[byteCount];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(byteArray);
            }

            return byteArray;
        }

        public async Task<ApiResponse<UserOutputModel>> CreateUser_ReturnsUserOutputModel()
        {
            byte[] byteArray = GeneratorByteCodes();

            var inputModel = Builder<UserInputModel>.CreateNew()
                        .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray)
                        .With(p => p.Id = 1)
                        .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            inputModel.Id = 1;

            var responsePost = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", inputModel);

            return responsePost;
        }

        public async Task<ApiResponse<CustomerOutputModel>> CreateCustomer_ReturnsCustomerOutputModel()
        {
            var inputModel = Builder<CustomerInputModel>.CreateNew()
                .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var responsePost = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
                                                "api/v1/Patika/Customers", inputModel);

            return responsePost;
        }

        public async Task<ApiResponse<AccountOutputModel>> CreateAccount_ReturnsAccountOutputModel()
        {
            const decimal balance = 1000;
            byte[] byteArray = GeneratorByteCodes();

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

            var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                "api/v1/Patika/Users", userInputModel);

            var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
                "api/v1/Patika/Customers", customerInputModel);

            var accountCreateResponse = await HttpClient.FiPostTestAsync<AccountInputModel, AccountOutputModel>(
                "api/v1/Patika/Accounts", accountInputModel);

            var checkUserResponse = await HttpClient.FiGetTestAsync<UserOutputModel>(
                 $"api/v1/Patika/Users/{userCreateResponse.Value.Id}", false);

            var checkCustomerResponse = await HttpClient.FiGetTestAsync<CustomerOutputModel>(
                $"api/v1/Patika/Customers/{customerCreateResponse.Value.Id}", false);

            var checkAccountResponse = await HttpClient.FiGetTestAsync<AccountOutputModel>(
                $"api/v1/Patika/Accounts/{accountCreateResponse.Value.Id}", false);

            return checkAccountResponse;
        }
    }
}
