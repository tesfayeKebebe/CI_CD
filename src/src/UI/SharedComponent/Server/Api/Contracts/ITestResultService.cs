using SharedComponent.Server.Models.LabTestResults;

namespace SharedComponent.Server.Api.Contracts;

public interface ITestResultService
{
    Task<TestResultDetail> Get(string parentId);
    Task<string> Create(TestResult command);
    Task<string> Update(TestResultDetail command);
    Task<string> Approval(LabTestResultApproval model);

}