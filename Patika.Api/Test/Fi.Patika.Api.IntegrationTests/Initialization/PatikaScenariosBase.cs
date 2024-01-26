using Fi.Test.IntegrationTests;

namespace Fi.Patika.Api.IntegrationTests.Initialization;

public class PatikaScenariosBase : FiScenariosBase<PatikaApplicationFactory, Startup>
{
    protected PatikaScenariosBase(PatikaApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
    {
    }
}