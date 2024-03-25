using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.UpdateProjectCompany;
using deftq.Pieceworks.Domain.project.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectCompany
{
    [Authorize]
    public class UpdateProjectCompanyController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;

        public UpdateProjectCompanyController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/setup/projectcompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Projects" },
            Summary = "Update piecework Company, Workplace, CVR and P number")]
        [SwaggerResponse(200, "The piecework company, workplace, CVR and P Number was updated")]
        [SwaggerResponse(403, "User not project owner")]
        [SwaggerResponse(404, "Project not found")]
        public async Task<ActionResult> UpdateProjectTypeType(Guid projectId, UpdateProjectCompanyRequest request)
        {
            var companyName = request.Company ?? "";
            var companyAddress = request.WorkplaceAdr ?? "";
            var cvrNumber = request.CvrNumber ?? "";
            var pNumber = request.PNumber ?? "";
            var command = UpdateProjectCompanyCommand.Create(projectId, companyName, companyAddress, cvrNumber, pNumber);
            await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
