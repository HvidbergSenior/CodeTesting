using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CreateProjectFolder;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Application.Generators;
using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.CreateProjectFolder;
using deftq.Pieceworks.Application.CreateProjectInvitation;
using deftq.Pieceworks.Application.GetProjectInvitation;
using deftq.Pieceworks.Application.GetProjects;
using deftq.Pieceworks.Domain.InvitationFlow;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CreateProjectInvitation
{
    [Authorize]
    public class CreateProjectInvitationController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IIdGenerator<Guid> _idGenerator;

        public CreateProjectInvitationController(ICommandBus commandBus, IIdGenerator<Guid> idGenerator)
        {
            _commandBus = commandBus;
            _idGenerator = idGenerator;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/invitation")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [SwaggerOperation(
            Tags = new string[] { "Invitation" },
            Summary = "Invite a user to a project")]
        public async Task<ActionResult> CreateInvitation(Guid projectId,
            [FromBody] CreateInvitationRequest request)
        {
            var invitationId = _idGenerator.Generate();
            var command = CreateProjectInvitationCommand.Create(invitationId, projectId, request.Email);
            await _commandBus.Send(command, HttpContext.RequestAborted);
            
            return base.Ok(invitationId);
        }
    }
}
