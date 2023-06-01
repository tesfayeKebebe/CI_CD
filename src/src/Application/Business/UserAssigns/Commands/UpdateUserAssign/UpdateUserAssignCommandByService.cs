using System.Globalization;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.UserAssigns.Commands.UpdateUserAssign;


public record UpdateUserAssignCommandByService   : IRequest
{
    public string UserId { get; set; }= null !;
    public double Longitude { get; set; } 
    public double Latitude { get; set; }
    
}
public class UpdateUserAssignCommandByServiceHandler : IRequestHandler<UpdateUserAssignCommandByService>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserAssignCommandByServiceHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateUserAssignCommandByService request, CancellationToken cancellationToken)
    {
        var entity = await _context.UserAssign.FirstOrDefaultAsync(x=>x.UserId==request.UserId, cancellationToken);
        if (entity == null)
        {
            throw new NotFoundException(nameof(UserAssign), request.UserId);
        }
        entity.UserId = request.UserId;
        entity.Latitude =Convert.ToString(request.Latitude, CultureInfo.InvariantCulture);
        entity.Longitude =Convert.ToString(request.Longitude, CultureInfo.InvariantCulture);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;

    }
}