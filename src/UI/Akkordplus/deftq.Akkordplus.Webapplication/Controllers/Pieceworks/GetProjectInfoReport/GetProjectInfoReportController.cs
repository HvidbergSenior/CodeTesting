using deftq.BuildingBlocks.Application.Queries;
using deftq.Pieceworks.Application.GetExtraWorkAgreementRates;
using deftq.Pieceworks.Application.GetGroupedWorkItems;
using deftq.Pieceworks.Application.GetProject;
using deftq.Pieceworks.Application.GetProjectFolderRoot;
using deftq.Pieceworks.Application.GetProjectLogBook;
using deftq.Pieceworks.Application.GetProjectParticipants;
using deftq.Pieceworks.Application.GetProjectSummation;
using deftq.Pieceworks.Application.GetProjectUsers;
using deftq.Pieceworks.Domain.project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetProjectInfoReport
{
    [Authorize]
    public class GetProjectInfoReportController : ApiControllerBase
    {
        private readonly IQueryBus _queryBus;

        public GetProjectInfoReportController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }

        [HttpGet]
        [Route("/api/projects/{projectId}/reports/projectinfo")]
        [SwaggerOperation(
            Tags = new string[] { "Reports" },
            Summary = "Get data for the project info report")]
        [ProducesResponseTypeAttribute(typeof(GetProjectInfoReportResponse), 200)]
        public async Task<ActionResult> GetProjectInfoReport(Guid projectId)
        {
            var projectIdObj = ProjectId.Create(projectId);
            var projectQuery = GetProjectQuery.Create(projectIdObj);
            var projectResp = await _queryBus.Send<GetProjectQuery, GetProjectResponse>(projectQuery);
            
            var rootFolderQuery = GetProjectFolderRootQuery.Create(projectIdObj);
            var rootFolderResp = await _queryBus.Send<GetProjectFolderRootQuery, GetProjectFolderRootQueryResponse>(rootFolderQuery);
            
            var extraWorkAgreementRatesQuery = GetExtraWorkAgreementRatesQuery.Create(projectIdObj);
            var extraWorkAgreementRatesResp = await _queryBus.Send<GetExtraWorkAgreementRatesQuery, GetExtraWorkAgreementRatesQueryResponse>(extraWorkAgreementRatesQuery);
            
            var projectSummaryQuery = GetProjectSummationQuery.Create(projectIdObj);
            var projectSummaryResp = await _queryBus.Send<GetProjectSummationQuery, GetProjectSummationQueryResponse>(projectSummaryQuery);

            var usersQuery = GetProjectUsersQuery.Create(projectIdObj);
            var usersResp = await _queryBus.Send<GetProjectUsersQuery, GetProjectUsersQueryResponse>(usersQuery);
            var users = usersResp.Users;
            
            var groupedWorkitemsQuery = GetGroupedWorkItemsQuery.Create(projectId, Guid.Parse("11111111-1111-1111-1111-111111111111"), 10000);
            var groupedWorkitemsResp = await _queryBus.Send<GetGroupedWorkItemsQuery, GetGroupedWorkItemsQueryResponse>(groupedWorkitemsQuery);
            
            var resp = new GetProjectInfoReportResponse(projectResp, rootFolderResp.RootFolder, extraWorkAgreementRatesResp, projectSummaryResp, users, groupedWorkitemsResp.GroupedWorkItems);
            return base.Ok(resp);
        }
    }
}
