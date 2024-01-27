﻿using Fi.Infra.Tools.DefinitionGenerator;
using Fi.Patika.Api.Domain.Entity;
using Fi.Patika.Api.IntegrationTests.Initialization;
using Fi.Patika.Schema.Model;
using Fi.Test.Extensions;
using FizzWare.NBuilder;
using Newtonsoft.Json;
using OpenTelemetry.Trace;
using Should;
using System.Security.Cryptography;
using Xunit;
using Xunit.Abstractions;

namespace Fi.Patika.Api.IntegrationTests.Controllers
{
    public class UsersControllerTests : PatikaScenariosBase
    {
        private const string basePath = "api/v1/Patika/Users";
        private HelperMethodsForTests helperMethodsForTests;

        private readonly ITestOutputHelper output;

        public UsersControllerTests(ITestOutputHelper output, PatikaApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
        {
            this.output = output;
            helperMethodsForTests = new HelperMethodsForTests(fiTestApplicationFactory);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task CreateUser_IfNotExists_ReturnsSuccess_WithCreatedUser()
        {
            // Arrange
            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

            var inputModel = Builder<UserInputModel>.CreateNew()
                .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray)
                                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            // Act
            var responsePost = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                                            $"{basePath}", inputModel);
            
            var response = await HttpClient.FiPostTestAsync(
                                            $"{basePath}/{responsePost.Value.Id}");

            // Assert
            responsePost.FiShouldBeSuccessStatus();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task UpdateUser_WhenCalled_ReturnsSuccess_WithUpdatedUser()
        {
            // Arrange
            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

            var inputModel = Builder<UserInputModel>.CreateNew()
                .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray)
                                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var responsePost = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                                            $"{basePath}", inputModel);
            inputModel.RoleType = RoleType.Auditor;

            // Act
            var response = await HttpClient.FiPutTestAsync<UserInputModel?, UserOutputModel>(
                                            $"{basePath}/{responsePost.Value.Id}", inputModel, false);

            // Assert
            response.FiShouldBeSuccessStatus();
            response.Value.ShouldNotBeNull();
            response.Value.RoleType.ShouldEqual(RoleType.Auditor);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task DeleteUserByKey_WhenCalled_ReturnsSuccess()
        {
            // Arrange
            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

            var inputModel = Builder<UserInputModel>.CreateNew()
                .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray)
                                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var responsePost = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                                            $"{basePath}", inputModel);

            // Act
            var response = await HttpClient.FiDeleteTestAsync(
                                            $"{basePath}/{responsePost.Value.Id}");

            // Assert
            response.FiShouldBeSuccessStatus();
            TestDbContext.Set<User>().FirstOrDefault(p => p.Id == responsePost.Value.Id).ShouldBeNull();
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetUserByKey_IfRequestedItemExists_ReturnsSuccess_WithItem()
        {
            // Arrange
            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

            var inputModel = Builder<UserInputModel>.CreateNew()
                .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray)
                                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var responsePost = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                                            $"{basePath}", inputModel);

            // Act
            var response = await HttpClient.FiGetTestAsync<UserOutputModel>(
                                            $"{basePath}/{responsePost.Value.Id}", false);

            // Assert
            response.FiShouldBeSuccessStatus();
            response.Value.ShouldNotBeNull();
            response.Value.Id.ShouldEqual(responsePost.Value.Id);
        }

        [Fact, Trait("Category", "Integration")]
        public async Task GetUsersByParameters_IfItemsExist_ReturnsSuccess_WithList()
        {
            // Arrange
            byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();
            var inputModel = Builder<UserInputModel>.CreateNew()
                .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray)
                                    .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

            var responsePost = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
                                            $"{basePath}", inputModel);

            // Act
            var response = await HttpClient.FiGetTestAsync<List<UserOutputModel>>(
                                            $"{basePath}/ByParameters", false);
            
            // Assert
            response.FiShouldBeSuccessStatus();
            response.Value.ShouldNotBeNull();
            response.Value.Count.ShouldBeGreaterThan(0);
        }
    }
}
