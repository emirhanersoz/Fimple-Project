using Fi.Patika.Api.Domain.Entity;
using Fi.Patika.Api.IntegrationTests.Initialization;
using Fi.Patika.Api.Persistence;
using Fi.Patika.Schema.Model;
using Fi.Infra.Exceptions;
using Fi.Infra.Schema.Const;
using Fi.Infra.Schema.Model;
using Fi.Mediator.Message;
using Fi.Persistence.Relational.Interfaces;
using Fi.Test.Extensions;
using Fi.Test.IntegrationTests.IntegrationTestHelper;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using Should;
using Xunit;
using Xunit.Abstractions;
using System.Collections;
using Newtonsoft.Json;

namespace Fi.Patika.Api.IntegrationTests.Controllers;

//[TestCaseOrderer("Fi.Patika.Api.IntegrationTests.PatikaTestCaseOrderer", "Fi.Patika.Api.IntegrationTests")]
public class CustomersControllerTests : PatikaScenariosBase
{
    private const string basePath = "api/v1/Patika/Customers";

    //Sonradan eklendi silenecek
    private readonly ITestOutputHelper output;
    private HelperMethodsForTests helperMethodsForTests;


    public CustomersControllerTests(ITestOutputHelper output, PatikaApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
    {
        this.output = output;
        helperMethodsForTests = new HelperMethodsForTests(fiTestApplicationFactory);
    }

    [Fact, Trait("Category", "Integration")]
    public async Task CreateCustomer_RequestedNotExist_ReturnsSuccess_WithItem()
    {
        //Arrange
        byte[] byteArray = helperMethodsForTests.GeneratorByteCodes();

        var userInputModel = Builder<UserInputModel>.CreateNew()
                .With(p => p.PasswordHash = byteArray).With(p => p.PasswordSalt = byteArray).With(p => p.Id = 1)
                .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();

        var customerInputModel = Builder<CustomerInputModel>.CreateNew()
                 .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        customerInputModel.UserId = 1;

        //Act
        var userCreateResponse = await HttpClient.FiPostTestAsync<UserInputModel, UserOutputModel>(
            "api/v1/Patika/Users", userInputModel);

        var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
            $"{basePath}", customerInputModel);

        var checkUserResponse = await HttpClient.FiGetTestAsync<UserOutputModel>(
             $"api/v1/Patika/Users/{userCreateResponse.Value.Id}", false);
        
        var checkCustomerResponse = await HttpClient.FiGetTestAsync<CustomerOutputModel>(
            $"{basePath}/{customerCreateResponse.Value.Id}", false);

        //Assert
        checkCustomerResponse.FiShouldBeSuccessStatus();
        checkCustomerResponse.Value.ShouldNotBeNull();
    }
}