using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Fi.Patika.Api.IntegrationTests
{
    public class PatikaTestCaseOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            return testCases.OrderBy(tc => tc.DisplayName);
        }
    }
}