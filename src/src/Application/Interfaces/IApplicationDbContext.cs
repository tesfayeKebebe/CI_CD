using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Lab> Lab { get; }

    DbSet<Category> Category { get; }
    DbSet<LabTest> LabTest { get; }
    DbSet<ServiceCharge> ServiceCharge { get; }
    DbSet<TestPrice> TestPrice { get; }
    DbSet<SampleType> SampleType { get; }
    DbSet<TubeType> TubeType { get; }
    DbSet<LabTestSampleTypeDetail> LabTestSampleTypeDetail { get; }
    DbSet<LabTestTubeTypeDetail> LabTestTubeTypeDetail { get; }
    DbSet<SelectedTestDetail> SelectedTestDetail { get; }
    DbSet<SelectedTestStatus> SelectedTestStatus { get; }
    DbSet<TestResult> TestResult { get; }
    DbSet<UserAssign> UserAssign { get; }
    DbSet<UserBranch> UserBranch { get; }
    DbSet<PatientFile> PatientFile { get; }
    DbSet<Organization> Organization { get; }
    DbSet<BankAccount> BankAccount { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
