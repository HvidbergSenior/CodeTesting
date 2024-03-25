using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CloseProjectLogBookWeek;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CopyProjectFolder;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CopyWorkItems;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CreateExtraWorkAgreement;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CreateProject;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CreateProjectFolder;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.CreateProjectInvitation;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetCompensationPaymentUsers;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetProjectInfoReport;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetWorkItemMaterialPreview;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetWorkItemOperationPreview;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.MoveProjectFolder;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.MoveWorkItems;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.OpenProjectLogBookWeek;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterLogbookSalaryAdvance;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectCatalogFavorite;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectCompensation;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectLogBookWeek;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectParticipant;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectSpecificOperation;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterWorkItemMaterial;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterWorkItemOperation;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RemoveExtraWorkAgreement;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RemoveProjectFavorites;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RemoveProjectSpecificOperations;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RemoveWorkItem;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateBaseRate;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateBaseSupplements;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateExtraWorkAgreementRates;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateFolderSupplements;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectCompany;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectExtraWorkAgreement;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectFolderDescription;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectFolderExtraWork;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectFolderLock;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectFolderName;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectLumpSum;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectNameAndOrderNumber;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectSpecificOperation;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateProjectType;
using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.UpdateWorkItem;
using deftq.BuildingBlocks.Exceptions;
using deftq.Pieceworks.Application.CreateProject;
using deftq.Pieceworks.Application.GetCompensationPaymentParticipantsInPeriod;
using deftq.Pieceworks.Application.GetExtraWorkAgreementRates;
using deftq.Pieceworks.Application.GetExtraWorkAgreements;
using deftq.Pieceworks.Application.GetFavoriteList;
using deftq.Pieceworks.Application.GetGroupedWorkItems;
using deftq.Pieceworks.Application.GetProject;
using deftq.Pieceworks.Application.GetProjectCompensation;
using deftq.Pieceworks.Application.GetProjectFolderRoot;
using deftq.Pieceworks.Application.GetProjectFolderSummation;
using deftq.Pieceworks.Application.GetProjectInvitation;
using deftq.Pieceworks.Application.GetProjectLogBook;
using deftq.Pieceworks.Application.GetProjectLogBookWeek;
using deftq.Pieceworks.Application.GetProjectSpecificOperations;
using deftq.Pieceworks.Application.GetProjectSummation;
using deftq.Pieceworks.Application.GetProjectUsers;
using deftq.Pieceworks.Application.GetWorkItemMaterialPreview;
using deftq.Pieceworks.Application.GetWorkItemOperationPreview;
using deftq.Pieceworks.Application.GetWorkItems;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectLogBook;
using Xunit;

namespace deftq.Tests.End2End
{
    public class Api
    {
        private readonly WebAppFixture _fixture;

        public WebAppFixture Fixture
        {
            get
            {
                return _fixture;
            }
        }

        public Api(WebAppFixture webAppFixture)
        {
            _fixture = webAppFixture;
        }

        public async Task<Guid> CreateProject(string title = "", string description = "", PieceworkType pieceworkType = PieceworkType.TwelveTwo,
            decimal pieceworkSum = 0)
        {
            if (String.IsNullOrEmpty(title))
            {
                title = Guid.NewGuid().ToString();
            }

            var request = new CreateProjectRequest(title, description, pieceworkType, pieceworkSum);
            var response = await _fixture.Client.PostAsJsonAsync("/api/projects", request);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            return await response.Content.ReadFromJsonAsync<Guid>();
        }

        public async Task<GetProjectResponse> GetProject(Guid projectId)
        {
            var projectResponse =
                await _fixture.Client.GetFromJsonAsync<GetProjectResponse>($"/api/projects/{projectId}", _fixture.JsonSerializerOptions());
            return projectResponse!;
        }

        public async Task<GetProjectInfoReportResponse> GetProjectInfoReport(Guid projectId)
        {
            var reportReponse = await _fixture.Client.GetFromJsonAsync<GetProjectInfoReportResponse>($"/api/projects/{projectId}/reports/projectinfo",
                _fixture.JsonSerializerOptions());
            return reportReponse!;
        }

        public async Task RemoveProject(Guid projectId)
        {
            var removeProjectResponse = await _fixture.Client.DeleteAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.OK, removeProjectResponse.StatusCode);
        }

        public async Task<Guid> CreateFolder(Guid projectId, string name, string description = "", Guid? parentFolderId = null)
        {
            var foldersRequest = new CreateProjectFolderRequest(name, description, parentFolderId);
            var createFoldersResponse = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/folders", foldersRequest);
            Assert.Equal(HttpStatusCode.OK, createFoldersResponse.StatusCode);

            var foldersResponse = await _fixture.Client.GetFromJsonAsync<GetProjectFolderRootQueryResponse>($"/api/projects/{projectId}/folders",
                _fixture.JsonSerializerOptions());

            return FindFolder(foldersResponse!.RootFolder, name, description).Single().ProjectFolderId;
        }

        private IList<ProjectFolderResponse> FindFolder(ProjectFolderResponse response, string name, string description)
        {
            var result = new List<ProjectFolderResponse>();

            if (string.Equals(response.ProjectFolderName, name, StringComparison.Ordinal) &&
                string.Equals(response.ProjectFolderDescription, description, StringComparison.Ordinal))
            {
                result.Add(response);
            }

            foreach (var subFolder in response.SubFolders)
            {
                var subFolderResponse = FindFolder(subFolder, name, description);
                result.AddRange(subFolderResponse);
            }

            return result;
        }

        public async Task UpdateFolderName(Guid projectId, Guid folderId, string newName)
        {
            var updateRequest = new UpdateProjectFolderNameRequest(newName);
            var updateResponse = await _fixture.Client.PutAsJsonAsync($"/api/projects/{projectId}/folders/{folderId}/name", updateRequest);
            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        }

        public async Task UpdateFolderDescription(Guid projectId, Guid folderId, string newDescription)
        {
            var updateRequest = new UpdateProjectFolderDescriptionRequest(newDescription);
            var updateResponse = await _fixture.Client.PutAsJsonAsync($"/api/projects/{projectId}/folders/{folderId}/description", updateRequest);
            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        }

        public async Task UpdateFolderSupplements(Guid projectId, Guid folderId, IList<Guid> supplementIds)
        {
            var updateRequest = new UpdateFolderSupplementsRequest(supplementIds);
            var updateResponse = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/folders/{folderId}/supplements", updateRequest);
            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        }

        public async Task RemoveFolder(Guid projectId, Guid folderId)
        {
            var removeFolderResponse = await _fixture.Client.DeleteAsync($"/api/projects/{projectId}/folders/{folderId}");
            Assert.Equal(HttpStatusCode.OK, removeFolderResponse.StatusCode);
        }

        public async Task CopyFolder(Guid projectId, Guid sourceFolderId, Guid destinationFolderId)
        {
            var copyFolderResponse = await _fixture.Client.PutAsJsonAsync($"/api/projects/{projectId}/folders/{sourceFolderId}/copy",
                new CopyProjectFolderRequest(destinationFolderId));
            Assert.Equal(HttpStatusCode.OK, copyFolderResponse.StatusCode);
        }

        public async Task MoveFolder(Guid projectId, Guid sourceFolderId, Guid destinationFolderId)
        {
            var removeFolderResponse = await _fixture.Client.PutAsJsonAsync($"/api/projects/{projectId}/folders/move",
                new MoveProjectFolderRequest(sourceFolderId, destinationFolderId));
            Assert.Equal(HttpStatusCode.OK, removeFolderResponse.StatusCode);
        }

        public async Task UpdateFolderLock(Guid projectId, Guid folderId, UpdateLockProjectFolderRequest.Lock lockStatus, bool recursive)
        {
            var lockResponse = await _fixture.Client.PostAsJsonAsync(
                $"/api/projects/{projectId}/folders/{folderId}/lock",
                new UpdateLockProjectFolderRequest(lockStatus, recursive));
            Assert.Equal(HttpStatusCode.OK, lockResponse.StatusCode);
        }

        public async Task RegisterFavorite(Guid projectId, Guid catalogId, FavoriteCatalogType catalogType)
        {
            var favoriteRequest = new RegisterProjectCatalogFavoriteRequest(catalogId, catalogType);
            var response = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/favorites", favoriteRequest);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async Task<GetProjectFavoriteListQueryResponse> GetFavorites(Guid projectId)
        {
            var favoriteResponse = await _fixture.Client.GetFromJsonAsync<GetProjectFavoriteListQueryResponse>($"/api/projects/{projectId}/favorites",
                _fixture.JsonSerializerOptions());
            return favoriteResponse!;
        }

        public async Task RemoveFavorite(Guid projectId, Guid favoriteId)
        {
            var favoritesRequest = new RemoveProjectFavoritesRequest(new List<Guid> { favoriteId });
            var favoritesResponse = await _fixture.Client.DeleteAsJsonAsync($"/api/projects/{projectId}/favorites", favoritesRequest);
            Assert.Equal(HttpStatusCode.OK, favoritesResponse.StatusCode);
        }

        public async Task<GetWorkItemsQueryResponse> GetWorkItems(Guid projectId, Guid folderId)
        {
            var getWorkItemsQueryResponse = await _fixture.Client.GetFromJsonAsync<GetWorkItemsQueryResponse>(
                $"/api/projects/{projectId}/folders/{folderId}/workitems",
                _fixture.JsonSerializerOptions());
            return getWorkItemsQueryResponse!;
        }

        public async Task<GetProjectFolderRootQueryResponse> GetFolderRoot(Guid projectId)
        {
            var getProjectFolderRootQueryResponse = await _fixture.Client.GetFromJsonAsync<GetProjectFolderRootQueryResponse>(
                $"/api/projects/{projectId}/folders",
                _fixture.JsonSerializerOptions());
            return getProjectFolderRootQueryResponse!;
        }

        public async Task RegisterExtraWorkAgreementWithPayment(Guid projectId, string number, string name, string? description, decimal? paymentDkr,
            CreateExtraWorkAgreementWorkTime? workTime = null)
        {
            var extraWorkAgreementType = CreateExtraWorkAgreementRequest.ExtraWorkAgreementTypeRequest.AgreedPayment;
            var extraWorkAgreementRequest =
                new CreateExtraWorkAgreementRequest(number, name, description, extraWorkAgreementType, paymentDkr, workTime);
            var extraWorkAgreementResponse = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/extraworkagreements",
                extraWorkAgreementRequest, _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, extraWorkAgreementResponse.StatusCode);
        }

        public async Task RegisterExtraWorkAgreementWithCompanyHoursWorkTime(Guid projectId, string number, string name, string? description,
            decimal? paymentDkr, CreateExtraWorkAgreementWorkTime? workTime)
        {
            var extraWorkAgreementType = CreateExtraWorkAgreementRequest.ExtraWorkAgreementTypeRequest.CompanyHours;
            var extraWorkAgreementRequest =
                new CreateExtraWorkAgreementRequest(number, name, description, extraWorkAgreementType, paymentDkr, workTime);
            var extraWorkAgreementResponse = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/extraworkagreements",
                extraWorkAgreementRequest, _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, extraWorkAgreementResponse.StatusCode);
        }

        public async Task RegisterExtraWorkAgreementWithCustomerHoursWorkTime(Guid projectId, string number, string name, string? description,
            decimal? paymentDkr, CreateExtraWorkAgreementWorkTime? workTime)
        {
            var extraWorkAgreementType = CreateExtraWorkAgreementRequest.ExtraWorkAgreementTypeRequest.CustomerHours;
            var extraWorkAgreementRequest =
                new CreateExtraWorkAgreementRequest(number, name, description, extraWorkAgreementType, paymentDkr, workTime);
            var extraWorkAgreementResponse = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/extraworkagreements",
                extraWorkAgreementRequest, _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, extraWorkAgreementResponse.StatusCode);
        }

        public async Task RegisterExtraWorkAgreementWithTypeOther(Guid projectId, string number, string name, string? description,
            decimal? paymentDkr, CreateExtraWorkAgreementWorkTime? workTime)
        {
            var extraWorkAgreementType = CreateExtraWorkAgreementRequest.ExtraWorkAgreementTypeRequest.Other;
            var extraWorkAgreementRequest =
                new CreateExtraWorkAgreementRequest(number, name, description, extraWorkAgreementType, paymentDkr, workTime);
            var extraWorkAgreementResponse = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/extraworkagreements",
                extraWorkAgreementRequest, _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, extraWorkAgreementResponse.StatusCode);
        }

        public async Task<GetExtraWorkAgreementsQueryResponse> GetExtraWorkAgreements(Guid projectId)
        {
            var extraWorkAgreementsResponse = await _fixture.Client.GetFromJsonAsync<GetExtraWorkAgreementsQueryResponse>(
                $"/api/projects/{projectId}/extraworkgreements", _fixture.JsonSerializerOptions());
            return extraWorkAgreementsResponse!;
        }

        public async Task RemoveExtraWorkAgreement(Guid projectId, Guid extraWorkAgreementId)
        {
            var extraWorkAgreementsRequest = new RemoveExtraWorkAgreementRequest(new List<Guid> { extraWorkAgreementId });
            var extraWorkAgreementsResponse =
                await _fixture.Client.DeleteAsJsonAsync($"/api/projects/{projectId}/extraworkagreements", extraWorkAgreementsRequest);
            Assert.Equal(HttpStatusCode.OK, extraWorkAgreementsResponse.StatusCode);
        }

        public async Task UpdateExtraWorkAgreement(Guid projectId, Guid extraWorkAgreementId, string number, string name, string? description,
            decimal? paymentDkr, UpdateProjectExtraWorkAgreementsRequest.UpdateExtraWorkAgreementType extraWorkAgreementType,
            UpdateExtraWorkAgreementWorkTime workTime)
        {
            var extraWorkAgreementRequest = new UpdateProjectExtraWorkAgreementsRequest(number, name, description,
                extraWorkAgreementType, paymentDkr, workTime);
            var extraWorkAgreementResponse = await _fixture.Client.PutAsJsonAsync(
                $"/api/projects/{projectId}/extraworkagreements/{extraWorkAgreementId}", extraWorkAgreementRequest, _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, extraWorkAgreementResponse.StatusCode);
        }

        public async Task RegisterWorkItemMaterial(Guid projectId, Guid folderId, Guid materialId, int mountingCode, decimal amount,
            IList<MaterialSupplementOperationRequest> materialSupplementOperations, IList<MaterialSupplementRequest> supplements)
        {
            var workItemMountingCode = WorkItemMountingCode.FromCode(mountingCode).MountingCode;
            var workItemRequest =
                new RegisterWorkItemMaterialRequest(materialId, amount, workItemMountingCode, materialSupplementOperations, supplements);
            var workItemResponse = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/folders/{folderId}/workitems/material",
                workItemRequest, _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, workItemResponse.StatusCode);
        }

        public async Task RegisterWorkItemOperation(Guid projectId, Guid folderId, Guid operationId, decimal amount,
            IList<OperationSupplementRequest> supplements)
        {
            var workItemRequest = new RegisterWorkItemOperationRequest(operationId, amount, supplements);
            var workItemResponse = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/folders/{folderId}/workitems/operation",
                workItemRequest,
                _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, workItemResponse.StatusCode);
        }

        public async Task<GetWorkItemMaterialPreviewQueryResponse> PreviewWorkItemMaterial(Guid projectId, Guid folderId, Guid materialId,
            int mountingCode, decimal amount,
            IList<GetWorkItemMaterialPreviewSupplementOperationRequest> materialSupplementOperations,
            IList<GetWorkItemMaterialPreviewSupplementRequest> supplements)
        {
            var workItemRequest = new GetWorkItemMaterialPreviewRequest(materialId, amount, mountingCode, materialSupplementOperations, supplements);
            var workItemPreviewResponse = await _fixture.Client.PostAsJsonAsync(
                $"/api/projects/{projectId}/folders/{folderId}/workitems/material/preview",
                workItemRequest, _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, workItemPreviewResponse.StatusCode);
            await using var stream = await workItemPreviewResponse.Content.ReadAsStreamAsync();
            return JsonSerializer.Deserialize<GetWorkItemMaterialPreviewQueryResponse>(stream, _fixture.JsonSerializerOptions())!;
        }

        public async Task<GetWorkItemOperationPreviewQueryResponse> PreviewWorkItemOperation(Guid projectId, Guid folderId, Guid operationId,
            decimal amount, IList<GetWorkItemOperationPreviewSupplementRequest> supplements)
        {
            var workItemRequest = new GetWorkItemOperationPreviewRequest(operationId, amount, supplements);
            var workItemPreviewResponse = await _fixture.Client.PostAsJsonAsync(
                $"/api/projects/{projectId}/folders/{folderId}/workitems/operation/preview",
                workItemRequest, _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, workItemPreviewResponse.StatusCode);
            await using var stream = await workItemPreviewResponse.Content.ReadAsStreamAsync();
            return JsonSerializer.Deserialize<GetWorkItemOperationPreviewQueryResponse>(stream, _fixture.JsonSerializerOptions())!;
        }

        public async Task CopyWorkItems(Guid projectId, Guid sourceFolderId, IList<Guid> workItemIds, Guid destinationFolderId)
        {
            var copyRequest = new CopyWorkItemsRequest(destinationFolderId, workItemIds);
            var response = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/folders/{sourceFolderId}/workitems/copy",
                copyRequest, _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async Task MoveWorkItems(Guid projectId, Guid sourceFolderId, IList<Guid> workItemIds, Guid destinationFolderId)
        {
            var moveWorkItemsRequest = new MoveWorkItemsRequest(destinationFolderId, workItemIds);
            var moveWorkItemsResponse =
                await _fixture.Client.PutAsJsonAsync($"/api/projects/{projectId}/folders/{sourceFolderId}/workitems/move", moveWorkItemsRequest);
            Assert.Equal(HttpStatusCode.OK, moveWorkItemsResponse.StatusCode);
        }

        public async Task RemoveWorkItem(Guid projectId, Guid folderId, IList<Guid> workItemIds)
        {
            var workItemsRequest = new RemoveWorkItemRequest(workItemIds);
            var workItemDeleteResponse =
                await _fixture.Client.DeleteAsJsonAsync($"/api/projects/{projectId}/folders/{folderId}/workitems", workItemsRequest);
            Assert.Equal(HttpStatusCode.OK, workItemDeleteResponse.StatusCode);
        }

        public async Task UpdateWorkItem(Guid projectId, Guid folderId, Guid workItemId, decimal workItemAmount)
        {
            var workItemRequest = new UpdateWorkItemRequest(workItemAmount);
            var workItemUpdateResponse = await _fixture.Client.PutAsJsonAsync($"/api/projects/{projectId}/folders/{folderId}/workItems/{workItemId}",
                workItemRequest, _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, workItemUpdateResponse.StatusCode);
        }

        public async Task UpdateFolderExtraWork(Guid projectId, Guid folderId, UpdateProjectFolderExtraWorkRequest.ExtraWorkUpdate extraWorkUpdate)
        {
            var workItemsRequest = new UpdateProjectFolderExtraWorkRequest(extraWorkUpdate);
            var workItemDeleteResponse =
                await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/folders/{folderId}/extrawork", workItemsRequest);
            Assert.Equal(HttpStatusCode.OK, workItemDeleteResponse.StatusCode);
        }

        public async Task<GetProjectFolderSummationQueryResponse> GetFolderSummation(Guid projectId, Guid folderId)
        {
            var summationResponse = await _fixture.Client.GetFromJsonAsync<GetProjectFolderSummationQueryResponse>(
                $"/api/projects/{projectId}/folders/{folderId}/summation", _fixture.JsonSerializerOptions());
            return summationResponse!;
        }

        public async Task<GetGroupedWorkItemsQueryResponse> GetGroupedWorkItemsResult(Guid projectId, Guid folderId, int maxHits)
        {
            var groupedWorkItemsResponse = await _fixture.Client.GetFromJsonAsync<GetGroupedWorkItemsQueryResponse>(
                $"/api/projects/{projectId}/folders/{folderId}/workitems/grouped?maxHits={maxHits}",
                _fixture.JsonSerializerOptions());
            return groupedWorkItemsResponse!;
        }

        public async Task UpdateFolderBaseRateRegulation(Guid projectId, Guid folderId, decimal value, BaseRateStatusUpdate status)
        {
            var baseRateRequest = new UpdateBaseRateRequest(new BaseRateUpdate(value, status));
            await UpdateFolderBaseRateRegulation(projectId, folderId, baseRateRequest);
        }

        public async Task UpdateFolderBaseRateRegulation(Guid projectId, Guid folderId, UpdateBaseRateRequest request)
        {
            var response = await _fixture.Client.PutAsJsonAsync(
                $"/api/projects/{projectId}/folders/{folderId}/baserate", request, _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async Task UpdateFolderBaseSupplements(Guid projectId, Guid folderId, decimal indirectTimePercentage,
            BaseSupplementStatusUpdate indirectTimeStatus, decimal siteSpecificTimePercentage, BaseSupplementStatusUpdate siteSpecificTimeStatus)
        {
            var indirectTime = new BaseSupplementUpdate(indirectTimePercentage, indirectTimeStatus);
            var siteSpecificTime = new BaseSupplementUpdate(siteSpecificTimePercentage, siteSpecificTimeStatus);
            var request = new UpdateBaseSupplementsRequest(indirectTime, siteSpecificTime);
            await UpdateFolderBaseSupplements(projectId, folderId, request);
        }

        public async Task UpdateFolderBaseSupplements(Guid projectId, Guid folderId, UpdateBaseSupplementsRequest request)
        {
            var response = await _fixture.Client.PutAsJsonAsync(
                $"/api/projects/{projectId}/folders/{folderId}/basesupplements", request, _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async Task<GetProjectSummationQueryResponse> GetProjectSummation(Guid projectId)
        {
            var response = await _fixture.Client.GetFromJsonAsync<GetProjectSummationQueryResponse>(
                $"/api/projects/{projectId}/summation", _fixture.JsonSerializerOptions());
            return response!;
        }

        public async Task<GetProjectLogBookQueryResponse> GetLogBook(Guid projectId)
        {
            var response = await _fixture.Client.GetFromJsonAsync<GetProjectLogBookQueryResponse>($"/api/projects/{projectId}/logbook");
            return response!;
        }

        public async Task<GetProjectLogBookWeekQueryResponse> GetLogBookWeek(Guid projectId, Guid userId, int year, int week)
        {
            var response = await _fixture.Client.GetFromJsonAsync<GetProjectLogBookWeekQueryResponse>(
                $"/api/projects/{projectId}/logbook/{userId}/weeks/{year}/{week}", _fixture.JsonSerializerOptions());
            return response!;
        }

        public async Task<GetProjectLogBookWeekQueryResponse> GetLogBookWeekFromDate(Guid projectId, Guid userId, int year, int month, int day)
        {
            var response = await _fixture.Client.GetFromJsonAsync<GetProjectLogBookWeekQueryResponse>(
                $"/api/projects/{projectId}/logbook/{userId}/weeks/{year}/{month}/{day}", _fixture.JsonSerializerOptions());
            return response!;
        }

        public async Task RegisterLogBookWeek(Guid projectId, RegisterProjectLogBookWeekRequest request)
        {
            var response = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/logbook/weeks", request,
                _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async Task CloseLogBookWeek(Guid projectId, CloseProjectLogBookWeekRequest request)
        {
            var response = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/logbook/weeks/close", request,
                _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async Task OpenLogBookWeek(Guid projectId, OpenProjectLogBookWeekRequest request)
        {
            var response = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/logbook/weeks/open", request,
                _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async Task RegisterLogBookSalaryAdvance(Guid projectId, RegisterLogbookSalaryAdvanceRequest request)
        {
            var response = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/logbook/salaryadvance", request,
                _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async Task<GetExtraWorkAgreementRatesQueryResponse> GetExtraWorkAgreementRates(Guid projectId)
        {
            var response = await _fixture.Client.GetFromJsonAsync<GetExtraWorkAgreementRatesQueryResponse>(
                $"api/projects/{projectId}/extraworkagreements/rates", _fixture.JsonSerializerOptions());
            return response!;
        }

        public async Task UpdateExtraWorkAgreementRates(Guid projectId, UpdateExtraWorkAgreementRatesRequest request)
        {
            var response = await _fixture.Client.PutAsJsonAsync($"api/projects/{projectId}/extraworkagreements/rates", request,
                _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async Task UpdateProjectLumpSum(Guid projectId, decimal projectLumpSum)
        {
            var request = new UpdateProjectLumpSumRequest(projectLumpSum);
            var response = await _fixture.Client.PutAsJsonAsync($"/api/projects/{projectId}/projectlumpsum", request,
                _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async Task UpdateProjectType(Guid projectId, UpdateProjectTypeRequest request)
        {
            var response = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/setup/projecttype", request,
                _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async Task UpdateProjectInformation(Guid projectId, UpdateProjectInformationRequest request)
        {
            var response = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/projectinformation", request,
                _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async Task UpdateProjectCompany(Guid projectId, string name, string address, string cvrNo, string pNo)
        {
            var request = new UpdateProjectCompanyRequest(name, address, cvrNo, pNo);
            var response = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/setup/projectcompany", request,
                _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async Task RegisterUser(Guid projectId, string name, UserRole role, string email, string? adr, string? phone)
        {
            var request = new RegisterProjectUserRequest(name, role, email, adr, phone);
            var response = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/users", request,
                _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async Task<GetProjectUsersQueryResponse> GetProjectUsers(Guid projectId)
        {
            var response = await _fixture.Client.GetFromJsonAsync<GetProjectUsersQueryResponse>($"/api/projects/{projectId}/users",
                _fixture.JsonSerializerOptions());
            return response!;
        }

        public async Task<GetCompensationPaymentResponse> GetCompensationPayment(Guid projectId, DateOnly start, DateOnly end, decimal amount)
        {
            var request = new GetCompensationPaymentParticipantsInPeriodRequest(start, end, amount);
            var response = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/compensations/participants", request,
                _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            await using var stream = await response.Content.ReadAsStreamAsync();
            return JsonSerializer.Deserialize<GetCompensationPaymentResponse>(stream, _fixture.JsonSerializerOptions())!;
        }

        public async Task RegisterProjectSpecificOperation(Guid projectId, string extraWorkAgreementNumber, string name, string? description,
            decimal operationTimeMs, decimal workingTimeMs)
        {
            var request = new RegisterProjectSpecificOperationRequest(extraWorkAgreementNumber, name, description, operationTimeMs, workingTimeMs);
            var response = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/projectspecificoperation", request,
                _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        public async Task UpdateProjectSpecificOperation(Guid projectId, Guid projectSpecificOperationId, string extraWorkAgreementNumber, string name, string? description,
            decimal operationTimeMs, decimal workingTimeMs)
        {
            var request = new UpdateProjectSpecificOperationRequest(extraWorkAgreementNumber, name, description, operationTimeMs, workingTimeMs);
            var response = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/projectspecificoperation/{projectSpecificOperationId}", request,
                _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        public async Task DeleteProjectSpecificOperation(Guid projectId, Guid projectSpecificOperationId)
        {
            var request = new RemoveProjectSpecificOperationsRequest(new List<Guid> {projectSpecificOperationId});
            var response = await _fixture.Client.DeleteAsJsonAsync($"/api/projects/{projectId}/projectspecificoperation/", request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        public async Task<GetProjectSpecificOperationsListResponse> GetProjectSpecificOperationsList(Guid projectId)
        {
            var response = await _fixture.Client.GetFromJsonAsync<GetProjectSpecificOperationsListResponse>(
                $"/api/projects/{projectId}/projectspecificoperation", _fixture.JsonSerializerOptions());
            return response!;
        }

        public async Task<GetProjectCompensationListQueryResponse> GetProjectCompensationList(Guid projectId)
        {
            var response = await _fixture.Client.GetFromJsonAsync<GetProjectCompensationListQueryResponse>($"/api/projects/{projectId}/compensations",
                _fixture.JsonSerializerOptions());
            return response!;
        }

        public async Task RegisterProjectCompensation(Guid projectId, RegisterCompensationRequest request)
        {
            var response = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/compensations", request,
                _fixture.JsonSerializerOptions());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async Task<HttpResponseMessage> CreateProjectInvitation(Guid projectId, string email)
        {
            var param = new CreateInvitationRequest(email);
            var response = await _fixture.Client.PostAsJsonAsync($"/api/projects/{projectId}/invitation", param,
                _fixture.JsonSerializerOptions());
            return response;
        }
    }
}
