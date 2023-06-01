using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Identity;
using Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Identity;
using Infrastructure.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, IApplicationDbContext
    {
        private readonly IMediator _mediator;
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

        public ApplicationDbContext(DbContextOptions options,
        IMediator mediator,
            AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
            : base(options)
        {
            _mediator = mediator;
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }
        public DbSet<Lab> Lab => Set<Lab>();

        public DbSet<Category> Category => Set<Category>();

        public DbSet<LabTest> LabTest => Set<LabTest>();

        public DbSet<ServiceCharge> ServiceCharge => Set<ServiceCharge>();
        public DbSet<TubeType> TubeType => Set<TubeType>();

        public DbSet<SampleType> SampleType => Set<SampleType>();

        public DbSet<TestPrice> TestPrice => Set<TestPrice>();
        public DbSet<LabTestSampleTypeDetail> LabTestSampleTypeDetail  => Set<LabTestSampleTypeDetail>();
        public DbSet<LabTestTubeTypeDetail> LabTestTubeTypeDetail => Set<LabTestTubeTypeDetail>();
        public DbSet<SelectedTestDetail> SelectedTestDetail => Set<SelectedTestDetail>();
        public DbSet<SelectedTestStatus> SelectedTestStatus => Set<SelectedTestStatus>();
        public DbSet<TestResult> TestResult => Set<TestResult>();
        public DbSet<UserAssign> UserAssign => Set<UserAssign>();
        public DbSet<UserBranch> UserBranch => Set<UserBranch>();
        public DbSet<PatientFile> PatientFile => Set<PatientFile>();
        public DbSet<Organization> Organization => Set<Organization>();
        public DbSet<BankAccount> BankAccount => Set<BankAccount>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //builder.Entity<ApplicationRole>(entity =>
            //{
            //    entity.Property(e => e.UpdatedDate)
            //     .HasColumnType("datetime2");
            //    entity.Property(e => e.CreatedDate)
            //   .HasColumnType("datetime2");
            //});
            //builder.Entity<ApplicationUser>(entity =>
            //{
            //    entity.Property(e => e.UpdatedDate)
            //     .HasColumnType("datetime2");
            //    entity.Property(e => e.CreatedDate)
            //   .HasColumnType("datetime2");
            //    entity.Property(e => e.RefreshTokenExpiryTime)
            //    .HasColumnType("datetime2");
            //});

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEvents(this);

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
