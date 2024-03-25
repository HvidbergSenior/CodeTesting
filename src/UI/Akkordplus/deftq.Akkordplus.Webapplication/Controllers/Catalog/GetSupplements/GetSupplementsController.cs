using deftq.BuildingBlocks.Application.Queries;
using deftq.Catalog.Application.GetSupplements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Catalog.GetSupplements
{
    [Authorize]
    public class GetSupplementsController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;
        private readonly IServiceProvider _serviceProvider;

        public GetSupplementsController(IQueryBus queryBus, IServiceProvider serviceProvider)
        {
            _queryBus = queryBus;
            _serviceProvider = serviceProvider;
        }
    
        [HttpGet]
        [Route("/api/catalog/supplements")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Catalog" },
            Summary = "Get list of available supplements")]
        [ProducesResponseTypeAttribute(typeof(GetSupplementsResponse), 200)]
        public async Task<ActionResult> GetSupplements()
        {
            var query = GetSupplementsQuery.Create();
            var response = await _queryBus.Send<GetSupplementsQuery, GetSupplementsResponse>(query);
            return base.Ok(response);
        }
    }
}
