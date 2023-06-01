using Application.Interfaces;
using Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.LabTestResults.Queries
{
    public record GetLabTestResults : IRequest<TestResultDetail>
    {
        public string TransactionNumber { get; set; } = null!;
    }
    public class GetTestResultDetailQueries : IRequestHandler<GetLabTestResults, TestResultDetail>
    {
        private readonly IApplicationDbContext _context;
        public GetTestResultDetailQueries(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<TestResultDetail> Handle(GetLabTestResults request, CancellationToken cancellationToken)
        {
            var data = await _context.TestResult.AsNoTracking()
                .FirstOrDefaultAsync(x => !x.IsDeleted && x.TransactionNumber == request.TransactionNumber,
                    cancellationToken: cancellationToken);
            if (data == null)
            {
                return new TestResultDetail();
            }
            var result =new  TestResultDetail
            {
                Id = data.Id,
                Description = data.Description,
                Reason = data.Reason,
                IsCompleted = data.IsCompleted,
                StoredFileName = data.StoredFileName,
                FileName = data.FileName,
                ContentType = data.ContentType
            };
            return result;

        }
    }
}
