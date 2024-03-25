using deftq.BuildingBlocks.Validation.Webapi.Errors;
using Microsoft.AspNetCore.Mvc;

namespace deftq.Akkordplus.WebApplication.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
    public class ApiControllerBase : ControllerBase
    {
    }
}
