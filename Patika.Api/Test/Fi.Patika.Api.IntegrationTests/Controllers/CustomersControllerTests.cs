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
    public async Task CreateCustomer_IfUserAndCustomerNotExist_ReturnsSuccess_WithItem()
    {
        //Arrange
        await TestDbContext.EnsureEntityIsEmpty<Customer>();

        var customerInputModel = Builder<CustomerInputModel>.CreateNew()
                 .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        customerInputModel.UserId = 1;

        //Act
        var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
            $"{basePath}", customerInputModel);
        
        var checkCustomerResponse = await HttpClient.FiGetTestAsync<CustomerOutputModel>(
            $"{basePath}/{customerCreateResponse.Value.Id}", false);

        //Assert
        customerCreateResponse.FiShouldBeSuccessStatus();
        checkCustomerResponse.Value.ShouldNotBeNull();
    }

    [Fact, Trait("Category", "Integration")]
    public async Task UpdateCustomer_WhenCalled_ReturnsSuccess_WithUpdatedCustomer()
    {
        //Arrange
        await TestDbContext.EnsureEntityIsEmpty<Customer>();

        var customerInputModel = Builder<CustomerInputModel>.CreateNew()
                 .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        customerInputModel.UserId = 1;

        var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
            $"{basePath}", customerInputModel);

        customerInputModel.City = "CityCity";

        // Act
        var response = await HttpClient.FiPutTestAsync<CustomerInputModel?, CustomerOutputModel>(
                                        $"{basePath}/{customerCreateResponse.Value.Id}", customerInputModel, false);

        // Assert
        response.FiShouldBeSuccessStatus();
        response.Value.ShouldNotBeNull();
        response.Value.City.ShouldEqual("CityCity");
    }

    [Fact, Trait("Category", "Integration")]
    public async Task DeleteCustomerByKey_WhenCalled_ReturnsSuccess()
    {
        //Arrange
        await TestDbContext.EnsureEntityIsEmpty<Customer>();

        var customerInputModel = Builder<CustomerInputModel>.CreateNew()
                 .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        customerInputModel.UserId = 1;

        var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
            $"{basePath}", customerInputModel);

        // Act
        var response = await HttpClient.FiDeleteTestAsync(
                                        $"{basePath}/{customerCreateResponse.Value.Id}");

        // Assert
        response.FiShouldBeSuccessStatus();
        TestDbContext.Set<Customer>().FirstOrDefault(p => p.Id == customerCreateResponse.Value.Id).ShouldBeNull();
    }

    [Fact, Trait("Category", "Integration")]
    public async Task GetCustomerByKey_IfRequestedItemExists_ReturnsSuccess_WithItem()
    {
        //Arrange
        await TestDbContext.EnsureEntityIsEmpty<Customer>();

        var customerInputModel = Builder<CustomerInputModel>.CreateNew()
                 .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        customerInputModel.UserId = 1;

        var customerCreateResponse = await HttpClient.FiPostTestAsync<CustomerInputModel, CustomerOutputModel>(
            $"{basePath}", customerInputModel);

        // Act
        var response = await HttpClient.FiGetTestAsync<CustomerOutputModel>(
                                        $"{basePath}/{customerCreateResponse.Value.Id}", false);

        // Assert
        response.FiShouldBeSuccessStatus();
        response.Value.ShouldNotBeNull();
        response.Value.Id.ShouldEqual(customerCreateResponse.Value.Id);
    }
}