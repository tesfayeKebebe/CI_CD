using System.Text;
using Application.Business.Categories.Queries;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.LabTests.Queries;

public record GetLabTestDropDown : IRequest<IEnumerable<LabTestPriceDetail2>>;

public class GetLabTestDropDownHandler : IRequestHandler<GetLabTestDropDown, IEnumerable<LabTestPriceDetail2>>
{
    private readonly IApplicationDbContext _context;

    public GetLabTestDropDownHandler(IApplicationDbContext application)
    {
        _context = application;
    }

    public async Task<IEnumerable<LabTestPriceDetail2>> Handle(GetLabTestDropDown request, CancellationToken cancellationToken)
    {
        var tests = await (from test in _context.LabTest
             join testPrice in _context.TestPrice on test.Id equals testPrice.LabTestId
             where  testPrice.IsActive && test.IsActive
                 select new LabTestPriceDetail2 {
                 Name = test.Name,
                 Id = test.Id,
                 CategoryId = test.CategoryId,
                 Price = testPrice.Price
                 }).AsNoTracking()
             .ToListAsync(cancellationToken);

        return tests;
    }
    
}