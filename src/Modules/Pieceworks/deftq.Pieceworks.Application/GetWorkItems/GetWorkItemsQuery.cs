using deftq.BuildingBlocks.Application;
using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.Exceptions;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.FolderWork;

namespace deftq.Pieceworks.Application.GetWorkItems
{
    public sealed class GetWorkItemsQuery : IQuery<GetWorkItemsQueryResponse>
    {
        public ProjectId ProjectId { get; private set; }
        public ProjectFolderId ProjectFolderId { get; private set; }

        private GetWorkItemsQuery()
        {
            ProjectId = ProjectId.Empty();
            ProjectFolderId = ProjectFolderId.Empty();
        }

        private GetWorkItemsQuery(ProjectId projectId, ProjectFolderId projectFolderId)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
        }

        public static GetWorkItemsQuery Create(Guid projectId, Guid projectFolderId)
        {
            return new GetWorkItemsQuery(ProjectId.Create(projectId), ProjectFolderId.Create(projectFolderId));
        }
    }

    internal class GetWorkItemsQueryHandler : IQueryHandler<GetWorkItemsQuery, GetWorkItemsQueryResponse>
    {
        private readonly IProjectFolderWorkRepository _projectFolderWorkRepository;

        public GetWorkItemsQueryHandler(IProjectFolderWorkRepository projectFolderWorkRepository)
        {
            _projectFolderWorkRepository = projectFolderWorkRepository;
        }

        public async Task<GetWorkItemsQueryResponse> Handle(GetWorkItemsQuery query, CancellationToken cancellationToken)
        {
            var projectFolderWork =
                await _projectFolderWorkRepository.GetByProjectAndFolderId(query.ProjectId.Value, query.ProjectFolderId.Value, cancellationToken);

            if (projectFolderWork is null)
            {
                throw new NotFoundException(
                    $"Project with id {query.ProjectId.Value} does not contain a folder with id {query.ProjectFolderId.Value}");
            }

            return MapResponse(query.ProjectId, query.ProjectFolderId, projectFolderWork);
        }

        private static GetWorkItemsQueryResponse MapResponse(ProjectId projectId, ProjectFolderId projectFolderId,
            ProjectFolderWork projectFolderWork)
        {
            var workItemsResponse = MapWorkItems(projectFolderWork);
            return new GetWorkItemsQueryResponse(projectId.Value, projectFolderId.Value, workItemsResponse);
        }

        private static IList<WorkItemResponse> MapWorkItems(ProjectFolderWork folderWork)
        {
            var result = new List<WorkItemResponse>();
            foreach (var workItem in folderWork.WorkItems)
            {
                var supplements = CreateSupplementResponse(workItem);

                if (workItem.IsMaterial())
                {
                    var supplementOperations = CreateSupplementOperationResponse(workItem);
                    var workItemMaterial = workItem.WorkItemMaterial!;
                    var workItemMaterialResponse = new WorkItemMaterialResponse(workItemMaterial.EanNumber.Value,
                        workItemMaterial.MountingCode.MountingCode,
                        workItemMaterial.MountingCode.Text, supplementOperations);

                    result.Add(new WorkItemResponse(workItem.WorkItemId.Value, WorkItemType.Material, workItem.Text.Value, workItem.Date.Value,
                        workItem.Amount.Value, workItem.TotalWorkTime.Value, workItem.OperationTime.Value,
                        workItem.TotalPayment.Value, supplements, workItemMaterialResponse, null));
                }
                else
                {
                    var workItemOperation = workItem.WorkItemOperation!;
                    var workItemOperationResponse = new WorkItemOperationResponse(workItemOperation.WorkItemOperationNumber.Value);
                    result.Add(new WorkItemResponse(workItem.WorkItemId.Value, WorkItemType.Operation, workItem.Text.Value, workItem.Date.Value,
                        workItem.Amount.Value,
                        workItem.TotalWorkTime.Value, workItem.OperationTime.Value,
                        workItem.TotalPayment.Value, supplements, null, workItemOperationResponse));
                }
            }

            return result;
        }

        private static IList<WorkItemSupplementResponse> CreateSupplementResponse(WorkItem workItem)
        {
            var supplements = new List<WorkItemSupplementResponse>();
            foreach (var supplement in workItem.Supplements)
            {
                supplements.Add(new WorkItemSupplementResponse(supplement.SupplementId.Value, supplement.SupplementNumber.Value,
                    supplement.SupplementText.Value));
            }

            return supplements;
        }

        private static List<WorkItemSupplementOperationResponse> CreateSupplementOperationResponse(WorkItem workItem)
        {
            var supplementOperations = new List<WorkItemSupplementOperationResponse>();
            foreach (var operation in workItem.WorkItemMaterial!.SupplementOperations)
            {
                var operationType = operation.IsAmountRelated()
                    ? WorkItemSupplementOperationResponse.WorkItemSupplementOperationType.AmountRelated
                    : WorkItemSupplementOperationResponse.WorkItemSupplementOperationType.UnitRelated;

                supplementOperations.Add(new WorkItemSupplementOperationResponse(operation.SupplementOperationId.Value, operation.Text.Value,
                    operationType,
                    operation.OperationTime.Value, operation.OperationAmount.Value));
            }

            return supplementOperations;
        }
    }

    internal class GetWorkItemsQueryAuthorizer : IAuthorizer<GetWorkItemsQuery>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IExecutionContext _executionContext;

        public GetWorkItemsQueryAuthorizer(IProjectRepository projectRepository, IExecutionContext executionContext)
        {
            _projectRepository = projectRepository;
            _executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetWorkItemsQuery query, CancellationToken cancellation)
        {
            var project = await _projectRepository.GetById(query.ProjectId.Value, cancellation);
            if (project.IsOwner(_executionContext.UserId) || project.IsParticipant(_executionContext.UserId) || project.IsProjectManager(_executionContext.UserId))
            {
                return AuthorizationResult.Succeed();
            }

            return AuthorizationResult.Fail();
        }
    }
}
