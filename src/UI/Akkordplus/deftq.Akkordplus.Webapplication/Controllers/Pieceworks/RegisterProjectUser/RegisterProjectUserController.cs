using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectParticipant;
using deftq.BuildingBlocks.Application.Commands;
using deftq.Pieceworks.Application.RegisterProjectUser;
using deftq.Pieceworks.Domain.project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectUser
{
    [Authorize]
    public class RegisterProjectUserController : ApiControllerBase
    {
        private readonly ICommandBus _commandBus;

        public RegisterProjectUserController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        [HttpPost]
        [Route("/api/projects/{projectId}/users/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Tags = new string[] { "Users" },
            Summary = "Register a new participant on the project")]
        public async Task<ActionResult> RegisterProjectUser(Guid projectId, RegisterProjectUserRequest request)
        {
            ICommand<ICommandResponse> command;

            if (request.Role == UserRole.ProjectManager)
            {
                command = CreateManagerCommand(projectId, request);
            }
            else
            {
                command = CreateParticipantCommand(projectId, request);
            }

            await _commandBus.Send(command, HttpContext.RequestAborted);
            return base.Ok();
        }

        private ICommand<ICommandResponse> CreateParticipantCommand(Guid projectId, RegisterProjectUserRequest request)
        {
            var email = String.IsNullOrWhiteSpace(request.Email) ? ProjectEmail.Empty() : ProjectEmail.Create(request.Email);
            var participant = ProjectParticipant.Create(ProjectParticipantId.Create(Guid.NewGuid()), ProjectParticipantName.Create(request.Name),
                email, ProjectParticipantAddress.Create(request.Address ?? string.Empty), ProjectParticipantPhoneNumber.Create(request.Phone ?? string.Empty) );
            return RegisterProjectUserCommand.Create(projectId, participant);
        }

        private ICommand<ICommandResponse> CreateManagerCommand(Guid projectId, RegisterProjectUserRequest request)
        {
            var email = String.IsNullOrWhiteSpace(request.Email) ? ProjectEmail.Empty() : ProjectEmail.Create(request.Email);
            var manager = ProjectManager.Create(Guid.NewGuid(), request.Name, email, request.Address, request.Phone);
            return RegisterProjectUserCommand.Create(projectId, manager);
        }
    }
}
