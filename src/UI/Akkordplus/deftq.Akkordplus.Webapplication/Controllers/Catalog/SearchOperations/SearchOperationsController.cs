using deftq.BuildingBlocks.Application.Queries;
using deftq.Catalog.Application.SearchOperation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Catalog.SearchOperations
{
    [Authorize]
    public class SearchOperationsController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;
        private readonly IServiceProvider _serviceProvider;

        public SearchOperationsController(IQueryBus queryBus, IServiceProvider serviceProvider)
        {
            _queryBus = queryBus;
            _serviceProvider = serviceProvider;
        }

        [HttpPost]
        [Route("/api/catalog/operations/search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Tags = new string[] { "Catalog" },
            Summary = "Search operations catalog")]
        [ProducesResponseTypeAttribute(typeof(SearchOperationResponse), 200)]
        public async Task<ActionResult> SearchOperationsCatalog(SearchOperationsRequest request)
        {
            var query = SearchOperationQuery.Create(request.SearchString, request.MaxHits);
            var response = await _queryBus.Send<SearchOperationQuery, SearchOperationResponse>(query);
            return base.Ok(response);
        }
    }
    
    public class SearchOperationsRequest
    {
        public string SearchString { get; private set; }
        public uint MaxHits { get; private set; }

        public SearchOperationsRequest(string searchString, uint maxHits)
        {
            SearchString = searchString;
            MaxHits = maxHits;
        }
    }
}
