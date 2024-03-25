using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CreateProject;
using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Application.Generators;
using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetDocument;
using deftq.Pieceworks.Domain.projectDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetDocument
{
    [Authorize]
    public class GetDocumentController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;

        public GetDocumentController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }

        [HttpGet]
        [Route("/api/documents/{documentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Tags = new string[] {"Documents"},
            Summary = "Get document")]
        public async Task<ActionResult> GetDocument(Guid documentId)
        {
            var query = GetDocumentQuery.Create(ProjectDocumentId.Create(documentId));
            var resp = await _queryBus.Send<GetDocumentQuery, GetDocumentResponse>(query);
            return base.File(resp.GetBuffer(), "application/octet-stream", resp.Filename);
        }
    }
}
