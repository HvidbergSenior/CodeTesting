using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.UpdateProjectType;
using deftq.Pieceworks.Domain.project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectType.UpdateProjectTypeRequest;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectType
{
    [Authorize]
    public class UpdateProjectTypeController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly ILogger<UpdateProjectTypeController> _logger;

        public UpdateProjectTypeController(ICommandBus commandBus, ILogger<UpdateProjectTypeController> logger)
        {
            _commandBus = commandBus;
            _logger = logger;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/setup/projecttype")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Projects" },
            Summary = "Update piecework type")]
        [SwaggerResponse(200, "The piecework type was updated")]
        [SwaggerResponse(403, "User not project owner")]
        [SwaggerResponse(404, "Project not found")]
        public async Task<ActionResult> UpdateProjectType(Guid projectId, UpdateProjectTypeRequest request)
        {
            var requestPieceWorkType = request.PieceworkType switch
            { 
                UpdateProjectPieceworkType.TwelveTwo => PieceworkType.TwelveTwo,
                UpdateProjectPieceworkType.TwelveOneA => PieceworkType.TwelveOneA,
                UpdateProjectPieceworkType.TwelveOneB => PieceworkType.TwelveOneB,
                UpdateProjectPieceworkType.TwelveOneC => PieceworkType.TwelveOneC,
                _ => throw new ArgumentException("Invalid Piecework Type", nameof(request))
            };

            var projectStartDate = request.StartDate is null ? ProjectStartDate.Empty() : ProjectStartDate.Create(request.StartDate!.Value);
            var projectEndDate = request.EndDate is null ? ProjectEndDate.Empty() : ProjectEndDate.Create(request.EndDate!.Value);
            var projectLumpSum = request.PieceWorkSum is null
                ? ProjectLumpSumPayment.Empty()
                : ProjectLumpSumPayment.Create(request.PieceWorkSum!.Value);
            
            var command = UpdateProjectTypeCommand.Create(projectId, requestPieceWorkType, projectStartDate, projectEndDate, projectLumpSum);
            await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }
    }
}
