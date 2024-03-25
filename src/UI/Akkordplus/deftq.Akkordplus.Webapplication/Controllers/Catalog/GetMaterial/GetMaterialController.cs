using deftq.BuildingBlocks.Application.Queries;
using deftq.Catalog.Application.GetMaterial;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Catalog.GetMaterial
{
    [Authorize]
    public class GetMaterialController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;
        private readonly IServiceProvider _serviceProvider;

        public GetMaterialController(IQueryBus queryBus, IServiceProvider serviceProvider)
        {
            _queryBus = queryBus;
            _serviceProvider = serviceProvider;
        }
    
        [HttpGet]
        [Route("/api/catalog/materials/{materialId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Catalog" },
            Summary = "Get detailed material information")]
        [ProducesResponseTypeAttribute(typeof(GetMaterialResponse), 200)]
        public async Task<ActionResult> GetMaterial(Guid materialId)
        {
            var query = GetMaterialQuery.Create(materialId);
            var response = await _queryBus.Send<GetMaterialQuery, GetMaterialResponse>(query);
            return base.Ok(response);
        }
    }
}
