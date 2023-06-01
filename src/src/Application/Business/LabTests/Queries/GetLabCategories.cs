using System.Text;
using Application.Business.Categories.Queries;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Application.Business.LabTests.Queries;
public record GetLabCategories     : IRequest<IEnumerable<LabCategoryDetail2>>;

public class GetLabCategoriesHandler : IRequestHandler<GetLabCategories, IEnumerable<LabCategoryDetail2>>
{
    private readonly IApplicationDbContext _context;

    public GetLabCategoriesHandler(IApplicationDbContext application)
    {
        _context = application;
    }

    public async Task<IEnumerable<LabCategoryDetail2>> Handle(GetLabCategories request, CancellationToken cancellationToken)
    {
        var tests = await (from test in _context.LabTest
                           join testPrice in _context.TestPrice on test.Id equals testPrice.LabTestId
                           join cat in _context.Category on test.CategoryId equals cat.Id
                           where testPrice.IsActive && test.IsActive
                           select new  LabCategoryDetail2{
                               Name = cat.Name,
                               Id = cat.Id,
                               LabTest=new LabTestPriceDetail2
                                   {
                                       Name = test.Name,
                                       Id = test.Id,
                                       CategoryId = test.CategoryId,
                                       Price = testPrice.Price
                                   }
                           }).AsNoTracking()
                           .ToListAsync(cancellationToken);
     
    
        return tests;
    }
}
