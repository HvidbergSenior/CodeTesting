using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.Catalog.Application.SearchMaterial;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Catalog.SearchMaterials
{
    [Authorize]
    public class SearchMaterialsController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;
        private readonly IServiceProvider _serviceProvider;

        public SearchMaterialsController(IQueryBus queryBus, IServiceProvider serviceProvider)
        {
            _queryBus = queryBus;
            _serviceProvider = serviceProvider;
        }

        [HttpPost]
        [Route("/api/catalog/materials/search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Catalog" },
            Summary = "Search materials catalog")]
        [ProducesResponseTypeAttribute(typeof(SearchMaterialResponse), 200)]
        public async Task<ActionResult> SearchMaterialsCatalog(SearchMaterialsRequest request)
        {
            var query = SearchMaterialQuery.Create(request.SearchString, request.MaxHits);
            var response = await _queryBus.Send<SearchMaterialQuery, SearchMaterialResponse>(query);
            return base.Ok(response);
        }
    }
    
    public class SearchMaterialsRequest
    {
        public string SearchString { get; private set; }
        public uint MaxHits { get; private set; }

        public SearchMaterialsRequest(string searchString, uint maxHits)
        {
            SearchString = searchString;
            MaxHits = maxHits;
        }
    }
}
