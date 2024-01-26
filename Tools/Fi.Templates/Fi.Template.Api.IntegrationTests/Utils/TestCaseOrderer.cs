using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Fi.TemplateUniqueName.Api.IntegrationTests
{
    public class TemplateUniqueNameTestCaseOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            return testCases.OrderBy(tc => tc.DisplayName);
        }
    }
}