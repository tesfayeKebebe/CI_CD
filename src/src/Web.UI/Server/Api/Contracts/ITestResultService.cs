using Web.UI.Server.Models.LabTestResults;

namespace Web.UI.Server.Api.Contracts;

public interface ITestResultService
{
    Task<TestResultDetail> Get(string parentId);
    Task<string> Create(TestResult command);
    Task<string> Update(TestResultDetail command);

}