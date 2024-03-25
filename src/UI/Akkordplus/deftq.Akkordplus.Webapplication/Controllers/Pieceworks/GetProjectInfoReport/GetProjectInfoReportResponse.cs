using deftq.Pieceworks.Application.GetExtraWorkAgreementRates;
using deftq.Pieceworks.Application.GetGroupedWorkItems;
using deftq.Pieceworks.Application.GetProject;
using deftq.Pieceworks.Application.GetProjectFolderRoot;
using deftq.Pieceworks.Application.GetProjectSummation;
using deftq.Pieceworks.Application.GetProjectUsers;

namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetProjectInfoReport
{
    public class GetProjectInfoReportResponse
    {
        public GetProjectResponse Project { get; }
        public ProjectFolderResponse RootFolder { get; }
        public GetExtraWorkAgreementRatesQueryResponse ExtraWorkAgreementsRates { get; }
        public GetProjectSummationQueryResponse ProjectSummation { get; }
        public IList<ProjectUserResponse> Users { get; }
        public IEnumerable<GroupedWorkItemsResponse> GroupedWorkitems { get; }

        public GetProjectInfoReportResponse(GetProjectResponse project, ProjectFolderResponse rootFolder,
            GetExtraWorkAgreementRatesQueryResponse extraWorkAgreementsRates, GetProjectSummationQueryResponse projectSummation,
            IList<ProjectUserResponse> users, IEnumerable<GroupedWorkItemsResponse> groupedWorkitems)
        {
            Project = project;
            RootFolder = rootFolder;
            ExtraWorkAgreementsRates = extraWorkAgreementsRates;
            ProjectSummation = projectSummation;
            Users = users;
            GroupedWorkitems = groupedWorkitems;
        }
    }
}
