using Application.Business.UserBranches.Queries;
using Application.Interfaces;
using Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.PatientFiles.Queries
{
    public record GetPatientFilesByPatientId : IRequest<IEnumerable<PatientFileDetail>>
    {
        public string? PatientId { get; set; }
    }
    public class GetPatientFileByPatientIdQueryHandler : IRequestHandler<GetPatientFilesByPatientId, IEnumerable<PatientFileDetail>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public GetPatientFileByPatientIdQueryHandler(IApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IEnumerable<PatientFileDetail>> Handle(GetPatientFilesByPatientId request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.FirstOrDefaultAsync(x=>x.Id==request.PatientId, cancellationToken: cancellationToken);
            var entities = await  _context.PatientFile.Where(x=>x.CreatedBy==request.PatientId).AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
            return entities.Select(item => 
                new PatientFileDetail 
                {
                    Id = item.Id, 
                    ContentType = item.ContentType,
                    StoredFileName = item.StoredFileName,
                    FileName = item.FileName,
                    PatientId  = users?.Id,
                    PatientName = users?.FullName}).ToList();
        }
    }
}
