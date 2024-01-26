using Fi.TemplateUniqueName.Api.Domain.Entity;
using Fi.Template.Api.IntegrationTests.Initialization;
using Fi.TemplateUniqueName.Api.Persistence;
using Fi.TemplateUniqueName.Schema.Model;
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

namespace Fi.TemplateUniqueName.Api.IntegrationTests.Controllers;

//[TestCaseOrderer("Fi.TemplateUniqueName.Api.IntegrationTests.TemplateUniqueNameTestCaseOrderer", "Fi.TemplateUniqueName.Api.IntegrationTests")]
public class SamplesControllerTests : TemplateUniqueNameScenariosBase
{
    private const string basePath = "api/v1/TemplateUniqueName/Samples";

    public SamplesControllerTests(TemplateUniqueNameApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
    {
    }
}