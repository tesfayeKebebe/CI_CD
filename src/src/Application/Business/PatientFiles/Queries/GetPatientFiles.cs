using Application.Business.UserBranches.Queries;
using Application.Interfaces;
using Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.PatientFiles.Queries
{
    public record GetPatientFile : IRequest<IEnumerable<PatientFileDetail>>;
    public class GetPatientFileQueryHandler : IRequestHandler<GetPatientFile, IEnumerable<PatientFileDetail>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public GetPatientFileQueryHandler(IApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IEnumerable<PatientFileDetail>> Handle(GetPatientFile request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.ToListAsync(cancellationToken: cancellationToken);
            var entities = await  _context.PatientFile.Where(x=>!x.IsDeleted).AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
            return entities.Select(item => 
                new PatientFileDetail 
                {
                    Id = item.Id, 
                    ContentType = item.ContentType,
                    StoredFileName = item.StoredFileName,
                    FileName = item.FileName,
                    PatientId  = users.FirstOrDefault(x=>x.Id==item.CreatedBy)?.Id,
                    Latitude = Convert.ToDouble(item.Latitude),
                    Longitude = Convert.ToDouble(item.Longitude),
                    PhoneNumber =  users.FirstOrDefault(x=>x.Id==item.CreatedBy)?.PhoneNumber,
                    PatientName = users.FirstOrDefault(x=>x.Id==item.CreatedBy)?.FullName}).ToList();
                  
        }
    }
}
