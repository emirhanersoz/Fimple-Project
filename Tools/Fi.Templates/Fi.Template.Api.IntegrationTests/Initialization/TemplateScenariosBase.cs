using Fi.Test.IntegrationTests;

namespace Fi.TemplateUniqueName.Api.IntegrationTests.Initialization;

public class TemplateUniqueNameScenariosBase : FiScenariosBase<TemplateUniqueNameApplicationFactory, Startup>
{
    protected TemplateUniqueNameScenariosBase(TemplateUniqueNameApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
    {
    }
}