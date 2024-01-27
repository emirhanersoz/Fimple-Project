/*using Fi.Patika.Api.Domain.Entity;
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

namespace Fi.Patika.Api.IntegrationTests.Controllers;

//[TestCaseOrderer("Fi.Patika.Api.IntegrationTests.PatikaTestCaseOrderer", "Fi.Patika.Api.IntegrationTests")]
public class SamplesControllerTests : PatikaScenariosBase
{
    private const string basePath = "api/v1/Patika/Samples";

    public SamplesControllerTests(PatikaApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
    {
    }

    [Fact, Trait("Category", "Integration")]
    public async Task GetByKey_IfRequestedItemExist_ReturnsSuccess_WithItem()
    {
        // Arrange
        // await EnsureEntityIsEmpty<Sample>();
        var inputModel = Builder<SampleInputModel>.CreateNew()
                                //Set your props here like '.With(p => p.CountryCode = ISOCountryCodes.TUR)' according to the logic in this API ...
                                .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        var responsePost = await HttpClient.FiPostTestAsync<SampleInputModel, SampleOutputModel>(
                                        $"{basePath}", inputModel);
        // Act
        var response = await HttpClient.FiGetTestAsync<SampleOutputModel>(
                                        $"{basePath}/{responsePost.Value.Id}", false);
        // Assert
        response.FiShouldBeSuccessStatus();
        response.Value.ShouldNotBeNull();
        response.Value.Id.ShouldEqual(responsePost.Value.Id);
    }

    [Fact, Trait("Category", "Integration")]
    public async Task GetAllList_IfItemsExist_ReturnSuccess_WithList()
    {
        // Arrange
        // await EnsureEntityIsEmpty<Sample>();
        var inputModel = Builder<SampleInputModel>.CreateNew()
                                //Set your props here like '.With(p => p.CountryCode = ISOCountryCodes.TUR)' according to the logic in this API ...
                                .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        var responsePost = await HttpClient.FiPostTestAsync<SampleInputModel, SampleOutputModel>(
                                        $"{basePath}", inputModel);
        // Act
        var response = await HttpClient.FiGetTestAsync<List<SampleOutputModel>>(
                                        $"{basePath}/", false);
        // Assert
        response.FiShouldBeSuccessStatus();
        response.Value.ShouldNotBeNull();
        response.Value.Count.ShouldBeGreaterThan(0);
    }


    [Fact, Trait("Category", "Integration")]
    public async Task GetByKey_IfRequestedItemDoesNotExist_ReturnsBadRequest_WithItemDoesNotExist()
    {
        // Arrange
        // await EnsureEntityIsEmpty<Sample>();
        var notExistValue = "999";
        // Act && Assert
        await HttpClient.FiGetShouldBeBadRequestStatus<SampleOutputModel>(
                                        $"{basePath}/{notExistValue}",
                                        BaseErrorCodes.ItemDoNotExists.Code, notExistValue);
    }

    [Fact, Trait("Category", "Integration")]
    public async Task GetBySampleCode_IfItemsExist_ReturnSuccess_WithList()
    {
        // Arrange
        // await EnsureEntityIsEmpty<Sample>();
        var inputModel = Builder<SampleInputModel>.CreateNew()
                                //Set your props here like '.With(p => p.CountryCode = ISOCountryCodes.TUR)' according to the logic in this API ...
                                .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        var responsePost = await HttpClient.FiPostTestAsync<SampleInputModel, SampleOutputModel>(
                                        $"{basePath}", inputModel);
        // Act
        var response = await HttpClient.FiGetTestAsync<List<SampleOutputModel>>(
                                        $"{basePath}/BySampleCode/{responsePost.Value.Code}", false);
        // Assert
        response.FiShouldBeSuccessStatus();
        response.Value.ShouldNotBeNull();
        response.Value.Count.ShouldBeGreaterThan(0);
    }

    [Fact, Trait("Category", "Integration")]
    public async Task Create_IfRequestedNotExist_ReturnsSuccess_WithItem()
    {
        // Arrange
        // await EnsureEntityIsEmpty<Sample>();
        var inputModel = Builder<SampleInputModel>.CreateNew()
                                //Set your props here like '.With(p => p.CountryCode = ISOCountryCodes.TUR)' according to the logic in this API ...
                                .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        // Act
        var response = await HttpClient.FiPostTestAsync<SampleInputModel, SampleOutputModel>(
                                        $"{basePath}/", inputModel, false);
        // Assert
        response.FiShouldBeSuccessStatus();
        response.Value.ShouldNotBeNull();
        response.Value.Code.ShouldEqual(inputModel.Code);
    }

    [Fact, Trait("Category", "Integration")]
    public async Task Update_WhenCalled_ReturnsSuccess_WithUpdatedItem()
    {
        // Arrange
        // await EnsureEntityIsEmpty<Sample>();
        var inputModel = Builder<SampleInputModel>.CreateNew()
                                //Set your props here like '.With(p => p.CountryCode = ISOCountryCodes.TUR)' according to the logic in this API ...
                                .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        var responsePost = await HttpClient.FiPostTestAsync<SampleInputModel, SampleOutputModel>(
                                        $"{basePath}", inputModel);
        inputModel.Description = "TEST";
        // Act
        var response = await HttpClient.FiPutTestAsync<SampleInputModel?, SampleOutputModel>(
                                        $"{basePath}/{responsePost.Value.Id}", inputModel, false);
        // Assert
        response.FiShouldBeSuccessStatus();
        response.Value.ShouldNotBeNull();
        response.Value.Description.ShouldEqual("TEST");
    }

    [Fact, Trait("Category", "Integration")]
    public async Task DeleteByKey_WhenCalled_ReturnsSuccess()
    {
        // Arrange
        // await EnsureEntityIsEmpty<Sample>();
        var inputModel = Builder<SampleInputModel>.CreateNew()
                                //Set your props here like '.With(p => p.CountryCode = ISOCountryCodes.TUR)' according to the logic in this API ...
                                .Build().AddFiDefaults().AddFiSmartEnums().AddFiML().AddSchemaDefaults();
        var responsePost = await HttpClient.FiPostTestAsync<SampleInputModel, SampleOutputModel>(
                                        $"{basePath}", inputModel);
        // Act
        var response = await HttpClient.FiDeleteTestAsync(
                                        $"{basePath}/{responsePost.Value.Id}");
        // Assert
        response.FiShouldBeSuccessStatus();
        TestDbContext.Set<Sample>().FirstOrDefault(p => p.Id == responsePost.Value.Id).ShouldBeNull();
    }
}*/