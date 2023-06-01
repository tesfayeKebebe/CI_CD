using Application.Business.Categories.Commands.CreateCategory;
using Application.Business.Categories.Commands.DeleteLab;
using Application.Business.Categories.Commands.UpdateLab;
using Application.Business.Categories.Queries;
using Application.Common.Security;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles ="administrator, supervisor" )]
    public class CategoryController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<CategoryDetail>> GetCategories()
        {
            return await Mediator.Send(new GetCategory());
        }
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CreateCategoryCommand command)
        {
            return await Mediator.Send(command);

        }
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateCategoryCommand command)
        {
            if (command == null)
            {
                return BadRequest();
            }
            await Mediator.Send(command);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var command = new DeleteCategoryCommand
            {
                Id = id
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
