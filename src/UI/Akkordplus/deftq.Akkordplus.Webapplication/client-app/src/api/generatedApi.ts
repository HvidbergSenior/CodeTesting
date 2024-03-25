import { baseApi as api } from "./baseApi";
const injectedRtkApi = api.injectEndpoints({
  endpoints: (build) => ({
    postApiProjectsByProjectIdLogbookWeeksClose: build.mutation<
      PostApiProjectsByProjectIdLogbookWeeksCloseApiResponse,
      PostApiProjectsByProjectIdLogbookWeeksCloseApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/logbook/weeks/close`,
        method: "POST",
        body: queryArg.closeProjectLogBookWeekRequest,
      }),
    }),
    getApiConfig: build.query<GetApiConfigApiResponse, GetApiConfigApiArg>({
      query: () => ({ url: `/api/config` }),
    }),
    putApiProjectsByProjectIdFoldersAndSourceFolderIdCopy: build.mutation<
      PutApiProjectsByProjectIdFoldersAndSourceFolderIdCopyApiResponse,
      PutApiProjectsByProjectIdFoldersAndSourceFolderIdCopyApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/folders/${queryArg.sourceFolderId}/copy`,
        method: "PUT",
        body: queryArg.copyProjectFolderRequest,
      }),
    }),
    postApiProjectsByProjectIdFoldersAndSourceFolderIdWorkitemsCopy: build.mutation<
      PostApiProjectsByProjectIdFoldersAndSourceFolderIdWorkitemsCopyApiResponse,
      PostApiProjectsByProjectIdFoldersAndSourceFolderIdWorkitemsCopyApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/folders/${queryArg.sourceFolderId}/workitems/copy`,
        method: "POST",
        body: queryArg.copyWorkItemsRequest,
      }),
    }),
    postApiProjectsByProjectIdExtraworkagreements: build.mutation<
      PostApiProjectsByProjectIdExtraworkagreementsApiResponse,
      PostApiProjectsByProjectIdExtraworkagreementsApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/extraworkagreements`,
        method: "POST",
        body: queryArg.createExtraWorkAgreementRequest,
      }),
    }),
    deleteApiProjectsByProjectIdExtraworkagreements: build.mutation<
      DeleteApiProjectsByProjectIdExtraworkagreementsApiResponse,
      DeleteApiProjectsByProjectIdExtraworkagreementsApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/extraworkagreements`,
        method: "DELETE",
        body: queryArg.removeExtraWorkAgreementRequest,
      }),
    }),
    postApiProjects: build.mutation<PostApiProjectsApiResponse, PostApiProjectsApiArg>({
      query: (queryArg) => ({ url: `/api/projects`, method: "POST", body: queryArg.createProjectRequest }),
    }),
    getApiProjects: build.query<GetApiProjectsApiResponse, GetApiProjectsApiArg>({
      query: () => ({ url: `/api/projects` }),
    }),
    postApiProjectsByProjectIdFolders: build.mutation<PostApiProjectsByProjectIdFoldersApiResponse, PostApiProjectsByProjectIdFoldersApiArg>({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/folders`, method: "POST", body: queryArg.createProjectFolderRequest }),
    }),
    getApiProjectsByProjectIdFolders: build.query<GetApiProjectsByProjectIdFoldersApiResponse, GetApiProjectsByProjectIdFoldersApiArg>({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/folders` }),
    }),
    postApiProjectsByProjectIdCompensationsParticipants: build.mutation<
      PostApiProjectsByProjectIdCompensationsParticipantsApiResponse,
      PostApiProjectsByProjectIdCompensationsParticipantsApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/compensations/participants`,
        method: "POST",
        body: queryArg.getCompensationPaymentParticipantsInPeriodRequest,
      }),
    }),
    getApiDocumentsByDocumentId: build.query<GetApiDocumentsByDocumentIdApiResponse, GetApiDocumentsByDocumentIdApiArg>({
      query: (queryArg) => ({ url: `/api/documents/${queryArg.documentId}` }),
    }),
    getApiProjectsByProjectIdExtraworkagreementsRates: build.query<
      GetApiProjectsByProjectIdExtraworkagreementsRatesApiResponse,
      GetApiProjectsByProjectIdExtraworkagreementsRatesApiArg
    >({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/extraworkagreements/rates` }),
    }),
    putApiProjectsByProjectIdExtraworkagreementsRates: build.mutation<
      PutApiProjectsByProjectIdExtraworkagreementsRatesApiResponse,
      PutApiProjectsByProjectIdExtraworkagreementsRatesApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/extraworkagreements/rates`,
        method: "PUT",
        body: queryArg.updateExtraWorkAgreementRatesRequest,
      }),
    }),
    getApiProjectsByProjectIdExtraworkgreements: build.query<
      GetApiProjectsByProjectIdExtraworkgreementsApiResponse,
      GetApiProjectsByProjectIdExtraworkgreementsApiArg
    >({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/extraworkgreements` }),
    }),
    getApiProjectsByProjectIdFoldersAndFolderIdWorkitemsGrouped: build.query<
      GetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsGroupedApiResponse,
      GetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsGroupedApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}/workitems/grouped`,
        params: { maxHits: queryArg.maxHits },
      }),
    }),
    getApiProjectsByProjectIdReportsLogbookspreadsheet: build.query<
      GetApiProjectsByProjectIdReportsLogbookspreadsheetApiResponse,
      GetApiProjectsByProjectIdReportsLogbookspreadsheetApiArg
    >({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/reports/logbookspreadsheet` }),
    }),
    getApiCatalogMaterialsByMaterialId: build.query<GetApiCatalogMaterialsByMaterialIdApiResponse, GetApiCatalogMaterialsByMaterialIdApiArg>({
      query: (queryArg) => ({ url: `/api/catalog/materials/${queryArg.materialId}` }),
    }),
    getApiProjectsByProjectId: build.query<GetApiProjectsByProjectIdApiResponse, GetApiProjectsByProjectIdApiArg>({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}` }),
    }),
    deleteApiProjectsByProjectId: build.mutation<DeleteApiProjectsByProjectIdApiResponse, DeleteApiProjectsByProjectIdApiArg>({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}`, method: "DELETE" }),
    }),
    getApiProjectsByProjectIdFavorites: build.query<GetApiProjectsByProjectIdFavoritesApiResponse, GetApiProjectsByProjectIdFavoritesApiArg>({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/favorites` }),
    }),
    postApiProjectsByProjectIdFavorites: build.mutation<PostApiProjectsByProjectIdFavoritesApiResponse, PostApiProjectsByProjectIdFavoritesApiArg>({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/favorites`,
        method: "POST",
        body: queryArg.registerProjectCatalogFavoriteRequest,
      }),
    }),
    deleteApiProjectsByProjectIdFavorites: build.mutation<
      DeleteApiProjectsByProjectIdFavoritesApiResponse,
      DeleteApiProjectsByProjectIdFavoritesApiArg
    >({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/favorites`, method: "DELETE", body: queryArg.removeProjectFavoritesRequest }),
    }),
    getApiProjectsByProjectIdCompensations: build.query<
      GetApiProjectsByProjectIdCompensationsApiResponse,
      GetApiProjectsByProjectIdCompensationsApiArg
    >({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/compensations` }),
    }),
    postApiProjectsByProjectIdCompensations: build.mutation<
      PostApiProjectsByProjectIdCompensationsApiResponse,
      PostApiProjectsByProjectIdCompensationsApiArg
    >({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/compensations`, method: "POST", body: queryArg.registerCompensationRequest }),
    }),
    deleteApiProjectsByProjectIdCompensations: build.mutation<
      DeleteApiProjectsByProjectIdCompensationsApiResponse,
      DeleteApiProjectsByProjectIdCompensationsApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/compensations`,
        method: "DELETE",
        body: queryArg.removeCompensationPaymentsRequest,
      }),
    }),
    getApiProjectsByProjectIdFoldersAndFolderIdSummation: build.query<
      GetApiProjectsByProjectIdFoldersAndFolderIdSummationApiResponse,
      GetApiProjectsByProjectIdFoldersAndFolderIdSummationApiArg
    >({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}/summation` }),
    }),
    getApiProjectsByProjectIdReportsProjectinfo: build.query<
      GetApiProjectsByProjectIdReportsProjectinfoApiResponse,
      GetApiProjectsByProjectIdReportsProjectinfoApiArg
    >({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/reports/projectinfo` }),
    }),
    getApiProjectsByProjectIdLogbook: build.query<GetApiProjectsByProjectIdLogbookApiResponse, GetApiProjectsByProjectIdLogbookApiArg>({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/logbook` }),
    }),
    getApiProjectsByProjectIdLogbookAndUserIdWeeksYearWeek: build.query<
      GetApiProjectsByProjectIdLogbookAndUserIdWeeksYearWeekApiResponse,
      GetApiProjectsByProjectIdLogbookAndUserIdWeeksYearWeekApiArg
    >({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/logbook/${queryArg.userId}/weeks/${queryArg.year}/${queryArg.week}` }),
    }),
    getApiProjectsByProjectIdLogbookAndUserIdWeeksYearMonthDay: build.query<
      GetApiProjectsByProjectIdLogbookAndUserIdWeeksYearMonthDayApiResponse,
      GetApiProjectsByProjectIdLogbookAndUserIdWeeksYearMonthDayApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/logbook/${queryArg.userId}/weeks/${queryArg.year}/${queryArg.month}/${queryArg.day}`,
      }),
    }),
    getApiProjectsByProjectIdProjectspecificoperation: build.query<
      GetApiProjectsByProjectIdProjectspecificoperationApiResponse,
      GetApiProjectsByProjectIdProjectspecificoperationApiArg
    >({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/projectspecificoperation` }),
    }),
    postApiProjectsByProjectIdProjectspecificoperation: build.mutation<
      PostApiProjectsByProjectIdProjectspecificoperationApiResponse,
      PostApiProjectsByProjectIdProjectspecificoperationApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/projectspecificoperation`,
        method: "POST",
        body: queryArg.registerProjectSpecificOperationRequest,
      }),
    }),
    deleteApiProjectsByProjectIdProjectspecificoperation: build.mutation<
      DeleteApiProjectsByProjectIdProjectspecificoperationApiResponse,
      DeleteApiProjectsByProjectIdProjectspecificoperationApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/projectspecificoperation`,
        method: "DELETE",
        body: queryArg.removeProjectSpecificOperationsRequest,
      }),
    }),
    getApiProjectsByProjectIdSummation: build.query<GetApiProjectsByProjectIdSummationApiResponse, GetApiProjectsByProjectIdSummationApiArg>({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/summation` }),
    }),
    getApiProjectsByProjectIdUsers: build.query<GetApiProjectsByProjectIdUsersApiResponse, GetApiProjectsByProjectIdUsersApiArg>({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/users` }),
    }),
    postApiProjectsByProjectIdUsers: build.mutation<PostApiProjectsByProjectIdUsersApiResponse, PostApiProjectsByProjectIdUsersApiArg>({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/users`, method: "POST", body: queryArg.registerProjectUserRequest }),
    }),
    getApiProjectsByProjectIdReportsStatusreportspreatsheet: build.query<
      GetApiProjectsByProjectIdReportsStatusreportspreatsheetApiResponse,
      GetApiProjectsByProjectIdReportsStatusreportspreatsheetApiArg
    >({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/reports/statusreportspreatsheet` }),
    }),
    getApiCatalogSupplements: build.query<GetApiCatalogSupplementsApiResponse, GetApiCatalogSupplementsApiArg>({
      query: () => ({ url: `/api/catalog/supplements` }),
    }),
    postApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMaterialPreview: build.mutation<
      PostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMaterialPreviewApiResponse,
      PostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMaterialPreviewApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}/workitems/material/preview`,
        method: "POST",
        body: queryArg.getWorkItemMaterialPreviewRequest,
      }),
    }),
    postApiProjectsByProjectIdFoldersAndFolderIdWorkitemsOperationPreview: build.mutation<
      PostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsOperationPreviewApiResponse,
      PostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsOperationPreviewApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}/workitems/operation/preview`,
        method: "POST",
        body: queryArg.getWorkItemOperationPreviewRequest,
      }),
    }),
    getApiProjectsByProjectIdFoldersAndFolderIdWorkitems: build.query<
      GetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsApiResponse,
      GetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsApiArg
    >({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}/workitems` }),
    }),
    deleteApiProjectsByProjectIdFoldersAndFolderIdWorkitems: build.mutation<
      DeleteApiProjectsByProjectIdFoldersAndFolderIdWorkitemsApiResponse,
      DeleteApiProjectsByProjectIdFoldersAndFolderIdWorkitemsApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}/workitems`,
        method: "DELETE",
        body: queryArg.removeWorkItemRequest,
      }),
    }),
    getApiProjectsByProjectIdReportsWorkitemsspreadsheet: build.query<
      GetApiProjectsByProjectIdReportsWorkitemsspreadsheetApiResponse,
      GetApiProjectsByProjectIdReportsWorkitemsspreadsheetApiArg
    >({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/reports/workitemsspreadsheet` }),
    }),
    putApiProjectsByProjectIdFoldersMove: build.mutation<PutApiProjectsByProjectIdFoldersMoveApiResponse, PutApiProjectsByProjectIdFoldersMoveApiArg>(
      {
        query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/folders/move`, method: "PUT", body: queryArg.moveProjectFolderRequest }),
      }
    ),
    putApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMove: build.mutation<
      PutApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMoveApiResponse,
      PutApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMoveApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}/workitems/move`,
        method: "PUT",
        body: queryArg.moveWorkItemsRequest,
      }),
    }),
    postApiProjectsByProjectIdLogbookWeeksOpen: build.mutation<
      PostApiProjectsByProjectIdLogbookWeeksOpenApiResponse,
      PostApiProjectsByProjectIdLogbookWeeksOpenApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/logbook/weeks/open`,
        method: "POST",
        body: queryArg.openProjectLogBookWeekRequest,
      }),
    }),
    getApiPing: build.query<GetApiPingApiResponse, GetApiPingApiArg>({
      query: () => ({ url: `/api/ping` }),
    }),
    postApiProjectsByProjectIdLogbookSalaryadvance: build.mutation<
      PostApiProjectsByProjectIdLogbookSalaryadvanceApiResponse,
      PostApiProjectsByProjectIdLogbookSalaryadvanceApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/logbook/salaryadvance`,
        method: "POST",
        body: queryArg.registerLogbookSalaryAdvanceRequest,
      }),
    }),
    postApiProjectsByProjectIdLogbookWeeks: build.mutation<
      PostApiProjectsByProjectIdLogbookWeeksApiResponse,
      PostApiProjectsByProjectIdLogbookWeeksApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/logbook/weeks`,
        method: "POST",
        body: queryArg.registerProjectLogBookWeekRequest,
      }),
    }),
    postApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMaterial: build.mutation<
      PostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMaterialApiResponse,
      PostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMaterialApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}/workitems/material`,
        method: "POST",
        body: queryArg.registerWorkItemMaterialRequest,
      }),
    }),
    postApiProjectsByProjectIdFoldersAndFolderIdWorkitemsOperation: build.mutation<
      PostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsOperationApiResponse,
      PostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsOperationApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}/workitems/operation`,
        method: "POST",
        body: queryArg.registerWorkItemOperationRequest,
      }),
    }),
    deleteApiProjectsByProjectIdFoldersAndFolderIdDocumentsDocumentId: build.mutation<
      DeleteApiProjectsByProjectIdFoldersAndFolderIdDocumentsDocumentIdApiResponse,
      DeleteApiProjectsByProjectIdFoldersAndFolderIdDocumentsDocumentIdApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}/documents/${queryArg.documentId}`,
        method: "DELETE",
      }),
    }),
    deleteApiProjectsByProjectIdFoldersAndFolderId: build.mutation<
      DeleteApiProjectsByProjectIdFoldersAndFolderIdApiResponse,
      DeleteApiProjectsByProjectIdFoldersAndFolderIdApiArg
    >({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}`, method: "DELETE" }),
    }),
    postApiCatalogMaterialsSearch: build.mutation<PostApiCatalogMaterialsSearchApiResponse, PostApiCatalogMaterialsSearchApiArg>({
      query: (queryArg) => ({ url: `/api/catalog/materials/search`, method: "POST", body: queryArg.searchMaterialsRequest }),
    }),
    postApiCatalogOperationsSearch: build.mutation<PostApiCatalogOperationsSearchApiResponse, PostApiCatalogOperationsSearchApiArg>({
      query: (queryArg) => ({ url: `/api/catalog/operations/search`, method: "POST", body: queryArg.searchOperationsRequest }),
    }),
    putApiProjectsByProjectIdFoldersAndFolderIdBaserate: build.mutation<
      PutApiProjectsByProjectIdFoldersAndFolderIdBaserateApiResponse,
      PutApiProjectsByProjectIdFoldersAndFolderIdBaserateApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}/baserate`,
        method: "PUT",
        body: queryArg.updateBaseRateRequest,
      }),
    }),
    putApiProjectsByProjectIdFoldersAndFolderIdBasesupplements: build.mutation<
      PutApiProjectsByProjectIdFoldersAndFolderIdBasesupplementsApiResponse,
      PutApiProjectsByProjectIdFoldersAndFolderIdBasesupplementsApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}/basesupplements`,
        method: "PUT",
        body: queryArg.updateBaseSupplementsRequest,
      }),
    }),
    postApiProjectsByProjectIdFoldersAndFolderIdSupplements: build.mutation<
      PostApiProjectsByProjectIdFoldersAndFolderIdSupplementsApiResponse,
      PostApiProjectsByProjectIdFoldersAndFolderIdSupplementsApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}/supplements`,
        method: "POST",
        body: queryArg.updateFolderSupplementsRequest,
      }),
    }),
    postApiProjectsByProjectIdFoldersAndFolderIdLock: build.mutation<
      PostApiProjectsByProjectIdFoldersAndFolderIdLockApiResponse,
      PostApiProjectsByProjectIdFoldersAndFolderIdLockApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}/lock`,
        method: "POST",
        body: queryArg.updateLockProjectFolderRequest,
      }),
    }),
    postApiProjectsByProjectIdSetupProjectcompany: build.mutation<
      PostApiProjectsByProjectIdSetupProjectcompanyApiResponse,
      PostApiProjectsByProjectIdSetupProjectcompanyApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/setup/projectcompany`,
        method: "POST",
        body: queryArg.updateProjectCompanyRequest,
      }),
    }),
    putApiProjectsByProjectIdExtraworkagreementsAndExtraWorkAgreementId: build.mutation<
      PutApiProjectsByProjectIdExtraworkagreementsAndExtraWorkAgreementIdApiResponse,
      PutApiProjectsByProjectIdExtraworkagreementsAndExtraWorkAgreementIdApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/extraworkagreements/${queryArg.extraWorkAgreementId}`,
        method: "PUT",
        body: queryArg.updateProjectExtraWorkAgreementsRequest,
      }),
    }),
    putApiProjectsByProjectIdFoldersAndFolderIdDescription: build.mutation<
      PutApiProjectsByProjectIdFoldersAndFolderIdDescriptionApiResponse,
      PutApiProjectsByProjectIdFoldersAndFolderIdDescriptionApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}/description`,
        method: "PUT",
        body: queryArg.updateProjectFolderDescriptionRequest,
      }),
    }),
    postApiProjectsByProjectIdFoldersAndFolderIdExtrawork: build.mutation<
      PostApiProjectsByProjectIdFoldersAndFolderIdExtraworkApiResponse,
      PostApiProjectsByProjectIdFoldersAndFolderIdExtraworkApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}/extrawork`,
        method: "POST",
        body: queryArg.updateProjectFolderExtraWorkRequest,
      }),
    }),
    putApiProjectsByProjectIdFoldersAndFolderIdName: build.mutation<
      PutApiProjectsByProjectIdFoldersAndFolderIdNameApiResponse,
      PutApiProjectsByProjectIdFoldersAndFolderIdNameApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}/name`,
        method: "PUT",
        body: queryArg.updateProjectFolderNameRequest,
      }),
    }),
    postApiProjectsByProjectIdProjectinformation: build.mutation<
      PostApiProjectsByProjectIdProjectinformationApiResponse,
      PostApiProjectsByProjectIdProjectinformationApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/projectinformation`,
        method: "POST",
        body: queryArg.updateProjectInformationRequest,
      }),
    }),
    putApiProjectsByProjectIdProjectlumpsum: build.mutation<
      PutApiProjectsByProjectIdProjectlumpsumApiResponse,
      PutApiProjectsByProjectIdProjectlumpsumApiArg
    >({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/projectlumpsum`, method: "PUT", body: queryArg.updateProjectLumpSumRequest }),
    }),
    postApiProjectsByProjectIdProjectspecificoperationAndProjectSpecificOperationId: build.mutation<
      PostApiProjectsByProjectIdProjectspecificoperationAndProjectSpecificOperationIdApiResponse,
      PostApiProjectsByProjectIdProjectspecificoperationAndProjectSpecificOperationIdApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/projectspecificoperation/${queryArg.projectSpecificOperationId}`,
        method: "POST",
        body: queryArg.updateProjectSpecificOperationRequest,
      }),
    }),
    postApiProjectsByProjectIdSetupProjecttype: build.mutation<
      PostApiProjectsByProjectIdSetupProjecttypeApiResponse,
      PostApiProjectsByProjectIdSetupProjecttypeApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/setup/projecttype`,
        method: "POST",
        body: queryArg.updateProjectTypeRequest,
      }),
    }),
    putApiProjectsByProjectIdFoldersAndFolderIdWorkItemsWorkItemId: build.mutation<
      PutApiProjectsByProjectIdFoldersAndFolderIdWorkItemsWorkItemIdApiResponse,
      PutApiProjectsByProjectIdFoldersAndFolderIdWorkItemsWorkItemIdApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}/workItems/${queryArg.workItemId}`,
        method: "PUT",
        body: queryArg.updateWorkItemRequest,
      }),
    }),
    postApiProjectsByProjectIdDocuments: build.mutation<PostApiProjectsByProjectIdDocumentsApiResponse, PostApiProjectsByProjectIdDocumentsApiArg>({
      query: (queryArg) => ({ url: `/api/projects/${queryArg.projectId}/documents`, method: "POST", body: queryArg.body }),
    }),
    postApiProjectsByProjectIdFoldersAndFolderIdDocuments: build.mutation<
      PostApiProjectsByProjectIdFoldersAndFolderIdDocumentsApiResponse,
      PostApiProjectsByProjectIdFoldersAndFolderIdDocumentsApiArg
    >({
      query: (queryArg) => ({
        url: `/api/projects/${queryArg.projectId}/folders/${queryArg.folderId}/documents`,
        method: "POST",
        body: queryArg.body,
      }),
    }),
  }),
  overrideExisting: false,
});
export { injectedRtkApi as generatedApi };
export type PostApiProjectsByProjectIdLogbookWeeksCloseApiResponse = /** status 200 Success */ undefined;
export type PostApiProjectsByProjectIdLogbookWeeksCloseApiArg = {
  projectId: string;
  closeProjectLogBookWeekRequest: CloseProjectLogBookWeekRequest;
};
export type GetApiConfigApiResponse = /** status 200 Success */ ConfigResponse;
export type GetApiConfigApiArg = void;
export type PutApiProjectsByProjectIdFoldersAndSourceFolderIdCopyApiResponse = /** status 200 The folder was copied */ undefined;
export type PutApiProjectsByProjectIdFoldersAndSourceFolderIdCopyApiArg = {
  projectId: string;
  sourceFolderId: string;
  copyProjectFolderRequest: CopyProjectFolderRequest;
};
export type PostApiProjectsByProjectIdFoldersAndSourceFolderIdWorkitemsCopyApiResponse = /** status 200 Success */ undefined;
export type PostApiProjectsByProjectIdFoldersAndSourceFolderIdWorkitemsCopyApiArg = {
  projectId: string;
  sourceFolderId: string;
  copyWorkItemsRequest: CopyWorkItemsRequest;
};
export type PostApiProjectsByProjectIdExtraworkagreementsApiResponse = /** status 201 Created */ undefined;
export type PostApiProjectsByProjectIdExtraworkagreementsApiArg = {
  projectId: string;
  createExtraWorkAgreementRequest: CreateExtraWorkAgreementRequest;
};
export type DeleteApiProjectsByProjectIdExtraworkagreementsApiResponse = /** status 200 The extra work agreement was removed */ undefined;
export type DeleteApiProjectsByProjectIdExtraworkagreementsApiArg = {
  projectId: string;
  removeExtraWorkAgreementRequest: RemoveExtraWorkAgreementRequest;
};
export type PostApiProjectsApiResponse = /** status 201 Created */ undefined;
export type PostApiProjectsApiArg = {
  createProjectRequest: CreateProjectRequest;
};
export type GetApiProjectsApiResponse = /** status 200 Success */ GetProjectsResponse;
export type GetApiProjectsApiArg = void;
export type PostApiProjectsByProjectIdFoldersApiResponse = /** status 200 Success */ undefined;
export type PostApiProjectsByProjectIdFoldersApiArg = {
  projectId: string;
  createProjectFolderRequest: CreateProjectFolderRequest;
};
export type GetApiProjectsByProjectIdFoldersApiResponse = /** status 200 Success */ GetProjectFolderRootQueryResponse;
export type GetApiProjectsByProjectIdFoldersApiArg = {
  projectId: string;
};
export type PostApiProjectsByProjectIdCompensationsParticipantsApiResponse = /** status 200 Success */ GetCompensationPaymentResponse;
export type PostApiProjectsByProjectIdCompensationsParticipantsApiArg = {
  projectId: string;
  getCompensationPaymentParticipantsInPeriodRequest: GetCompensationPaymentParticipantsInPeriodRequest;
};
export type GetApiDocumentsByDocumentIdApiResponse = /** status 200 Success */ undefined;
export type GetApiDocumentsByDocumentIdApiArg = {
  documentId: string;
};
export type GetApiProjectsByProjectIdExtraworkagreementsRatesApiResponse = /** status 200 Success */ GetExtraWorkAgreementRatesQueryResponse;
export type GetApiProjectsByProjectIdExtraworkagreementsRatesApiArg = {
  projectId: string;
};
export type PutApiProjectsByProjectIdExtraworkagreementsRatesApiResponse =
  /** status 200 The extra work agreement rates have been updated */ undefined;
export type PutApiProjectsByProjectIdExtraworkagreementsRatesApiArg = {
  projectId: string;
  updateExtraWorkAgreementRatesRequest: UpdateExtraWorkAgreementRatesRequest;
};
export type GetApiProjectsByProjectIdExtraworkgreementsApiResponse = /** status 200 Success */ ExtraWorkAgreementsResponse;
export type GetApiProjectsByProjectIdExtraworkgreementsApiArg = {
  projectId: string;
};
export type GetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsGroupedApiResponse = /** status 200 Success */ GetGroupedWorkItemsQueryResponse;
export type GetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsGroupedApiArg = {
  projectId: string;
  folderId: string;
  maxHits?: number;
};
export type GetApiProjectsByProjectIdReportsLogbookspreadsheetApiResponse = /** status 200 Success */ undefined;
export type GetApiProjectsByProjectIdReportsLogbookspreadsheetApiArg = {
  projectId: string;
};
export type GetApiCatalogMaterialsByMaterialIdApiResponse = /** status 200 Success */ GetMaterialResponse;
export type GetApiCatalogMaterialsByMaterialIdApiArg = {
  materialId: string;
};
export type GetApiProjectsByProjectIdApiResponse = /** status 200 Success */ GetProjectResponse;
export type GetApiProjectsByProjectIdApiArg = {
  projectId: string;
};
export type DeleteApiProjectsByProjectIdApiResponse = /** status 200 The project was removed */ undefined;
export type DeleteApiProjectsByProjectIdApiArg = {
  projectId: string;
};
export type GetApiProjectsByProjectIdFavoritesApiResponse = /** status 200 Success */ GetProjectFavoriteListQueryResponse;
export type GetApiProjectsByProjectIdFavoritesApiArg = {
  projectId: string;
};
export type PostApiProjectsByProjectIdFavoritesApiResponse = /** status 200 Success */ undefined;
export type PostApiProjectsByProjectIdFavoritesApiArg = {
  projectId: string;
  registerProjectCatalogFavoriteRequest: RegisterProjectCatalogFavoriteRequest;
};
export type DeleteApiProjectsByProjectIdFavoritesApiResponse = /** status 200 The favorite was removed */ undefined;
export type DeleteApiProjectsByProjectIdFavoritesApiArg = {
  projectId: string;
  removeProjectFavoritesRequest: RemoveProjectFavoritesRequest;
};
export type GetApiProjectsByProjectIdCompensationsApiResponse = /** status 200 Success */ GetProjectCompensationListQueryResponse;
export type GetApiProjectsByProjectIdCompensationsApiArg = {
  projectId: string;
};
export type PostApiProjectsByProjectIdCompensationsApiResponse = /** status 200 Success */ undefined;
export type PostApiProjectsByProjectIdCompensationsApiArg = {
  projectId: string;
  registerCompensationRequest: RegisterCompensationRequest;
};
export type DeleteApiProjectsByProjectIdCompensationsApiResponse = /** status 200 The compensations were removed */ undefined;
export type DeleteApiProjectsByProjectIdCompensationsApiArg = {
  projectId: string;
  removeCompensationPaymentsRequest: RemoveCompensationPaymentsRequest;
};
export type GetApiProjectsByProjectIdFoldersAndFolderIdSummationApiResponse = /** status 200 Success */ GetProjectFolderSummationQueryResponse;
export type GetApiProjectsByProjectIdFoldersAndFolderIdSummationApiArg = {
  projectId: string;
  folderId: string;
};
export type GetApiProjectsByProjectIdReportsProjectinfoApiResponse = /** status 200 Success */ GetProjectInfoReportResponse;
export type GetApiProjectsByProjectIdReportsProjectinfoApiArg = {
  projectId: string;
};
export type GetApiProjectsByProjectIdLogbookApiResponse = /** status 200 Success */ GetProjectLogBookQueryResponse;
export type GetApiProjectsByProjectIdLogbookApiArg = {
  projectId: string;
};
export type GetApiProjectsByProjectIdLogbookAndUserIdWeeksYearWeekApiResponse = /** status 200 Success */ GetProjectLogBookWeekQueryResponse;
export type GetApiProjectsByProjectIdLogbookAndUserIdWeeksYearWeekApiArg = {
  projectId: string;
  userId: string;
  year: number;
  week: number;
};
export type GetApiProjectsByProjectIdLogbookAndUserIdWeeksYearMonthDayApiResponse = /** status 200 Success */ GetProjectLogBookWeekQueryResponse;
export type GetApiProjectsByProjectIdLogbookAndUserIdWeeksYearMonthDayApiArg = {
  projectId: string;
  userId: string;
  year: number;
  month: number;
  day: number;
};
export type GetApiProjectsByProjectIdProjectspecificoperationApiResponse = /** status 200 Success */ GetProjectSpecificOperationsListResponse;
export type GetApiProjectsByProjectIdProjectspecificoperationApiArg = {
  projectId: string;
};
export type PostApiProjectsByProjectIdProjectspecificoperationApiResponse = /** status 201 Created */ undefined;
export type PostApiProjectsByProjectIdProjectspecificoperationApiArg = {
  projectId: string;
  registerProjectSpecificOperationRequest: RegisterProjectSpecificOperationRequest;
};
export type DeleteApiProjectsByProjectIdProjectspecificoperationApiResponse = /** status 200 The project specific operations was removed */ undefined;
export type DeleteApiProjectsByProjectIdProjectspecificoperationApiArg = {
  projectId: string;
  removeProjectSpecificOperationsRequest: RemoveProjectSpecificOperationsRequest;
};
export type GetApiProjectsByProjectIdSummationApiResponse = /** status 200 Success */ GetProjectSummationQueryResponse;
export type GetApiProjectsByProjectIdSummationApiArg = {
  projectId: string;
};
export type GetApiProjectsByProjectIdUsersApiResponse = /** status 200 Success */ GetProjectUsersQueryResponse;
export type GetApiProjectsByProjectIdUsersApiArg = {
  projectId: string;
};
export type PostApiProjectsByProjectIdUsersApiResponse = /** status 200 Success */ undefined;
export type PostApiProjectsByProjectIdUsersApiArg = {
  projectId: string;
  registerProjectUserRequest: RegisterProjectUserRequest;
};
export type GetApiProjectsByProjectIdReportsStatusreportspreatsheetApiResponse = /** status 200 Success */ undefined;
export type GetApiProjectsByProjectIdReportsStatusreportspreatsheetApiArg = {
  projectId: string;
};
export type GetApiCatalogSupplementsApiResponse = /** status 200 Success */ GetSupplementsResponse;
export type GetApiCatalogSupplementsApiArg = void;
export type PostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMaterialPreviewApiResponse =
  /** status 200 Success */ GetWorkItemMaterialPreviewQueryResponse;
export type PostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMaterialPreviewApiArg = {
  projectId: string;
  folderId: string;
  getWorkItemMaterialPreviewRequest: GetWorkItemMaterialPreviewRequest;
};
export type PostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsOperationPreviewApiResponse =
  /** status 200 Success */ GetWorkItemOperationPreviewQueryResponse;
export type PostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsOperationPreviewApiArg = {
  projectId: string;
  folderId: string;
  getWorkItemOperationPreviewRequest: GetWorkItemOperationPreviewRequest;
};
export type GetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsApiResponse = /** status 200 Success */ GetWorkItemsQueryResponse;
export type GetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsApiArg = {
  projectId: string;
  folderId: string;
};
export type DeleteApiProjectsByProjectIdFoldersAndFolderIdWorkitemsApiResponse = /** status 200 The work item was removed */ undefined;
export type DeleteApiProjectsByProjectIdFoldersAndFolderIdWorkitemsApiArg = {
  projectId: string;
  folderId: string;
  removeWorkItemRequest: RemoveWorkItemRequest;
};
export type GetApiProjectsByProjectIdReportsWorkitemsspreadsheetApiResponse = /** status 200 Success */ undefined;
export type GetApiProjectsByProjectIdReportsWorkitemsspreadsheetApiArg = {
  projectId: string;
};
export type PutApiProjectsByProjectIdFoldersMoveApiResponse = /** status 200 The folder was moved */ undefined;
export type PutApiProjectsByProjectIdFoldersMoveApiArg = {
  projectId: string;
  moveProjectFolderRequest: MoveProjectFolderRequest;
};
export type PutApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMoveApiResponse = /** status 200 The work items are moved */ undefined;
export type PutApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMoveApiArg = {
  projectId: string;
  folderId: string;
  moveWorkItemsRequest: MoveWorkItemsRequest;
};
export type PostApiProjectsByProjectIdLogbookWeeksOpenApiResponse = /** status 200 Success */ undefined;
export type PostApiProjectsByProjectIdLogbookWeeksOpenApiArg = {
  projectId: string;
  openProjectLogBookWeekRequest: OpenProjectLogBookWeekRequest;
};
export type GetApiPingApiResponse = unknown;
export type GetApiPingApiArg = void;
export type PostApiProjectsByProjectIdLogbookSalaryadvanceApiResponse = /** status 200 Success */ undefined;
export type PostApiProjectsByProjectIdLogbookSalaryadvanceApiArg = {
  projectId: string;
  registerLogbookSalaryAdvanceRequest: RegisterLogbookSalaryAdvanceRequest;
};
export type PostApiProjectsByProjectIdLogbookWeeksApiResponse = /** status 200 Success */ undefined;
export type PostApiProjectsByProjectIdLogbookWeeksApiArg = {
  projectId: string;
  registerProjectLogBookWeekRequest: RegisterProjectLogBookWeekRequest;
};
export type PostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMaterialApiResponse = /** status 200 Success */ undefined;
export type PostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMaterialApiArg = {
  projectId: string;
  folderId: string;
  registerWorkItemMaterialRequest: RegisterWorkItemMaterialRequest;
};
export type PostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsOperationApiResponse = /** status 200 Success */ undefined;
export type PostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsOperationApiArg = {
  projectId: string;
  folderId: string;
  registerWorkItemOperationRequest: RegisterWorkItemOperationRequest;
};
export type DeleteApiProjectsByProjectIdFoldersAndFolderIdDocumentsDocumentIdApiResponse = /** status 200 Success */ undefined;
export type DeleteApiProjectsByProjectIdFoldersAndFolderIdDocumentsDocumentIdApiArg = {
  projectId: string;
  folderId: string;
  documentId: string;
};
export type DeleteApiProjectsByProjectIdFoldersAndFolderIdApiResponse = /** status 200 The folder was removed */ undefined;
export type DeleteApiProjectsByProjectIdFoldersAndFolderIdApiArg = {
  projectId: string;
  folderId: string;
};
export type PostApiCatalogMaterialsSearchApiResponse = /** status 200 Success */ SearchMaterialResponse;
export type PostApiCatalogMaterialsSearchApiArg = {
  searchMaterialsRequest: SearchMaterialsRequest;
};
export type PostApiCatalogOperationsSearchApiResponse = /** status 200 Success */ SearchOperationResponse;
export type PostApiCatalogOperationsSearchApiArg = {
  searchOperationsRequest: SearchOperationsRequest;
};
export type PutApiProjectsByProjectIdFoldersAndFolderIdBaserateApiResponse = /** status 200 The folder base rate was updated */ undefined;
export type PutApiProjectsByProjectIdFoldersAndFolderIdBaserateApiArg = {
  projectId: string;
  folderId: string;
  updateBaseRateRequest: UpdateBaseRateRequest;
};
export type PutApiProjectsByProjectIdFoldersAndFolderIdBasesupplementsApiResponse =
  /** status 200 The folder base supplements were updated */ undefined;
export type PutApiProjectsByProjectIdFoldersAndFolderIdBasesupplementsApiArg = {
  projectId: string;
  folderId: string;
  updateBaseSupplementsRequest: UpdateBaseSupplementsRequest;
};
export type PostApiProjectsByProjectIdFoldersAndFolderIdSupplementsApiResponse = /** status 200 The folder supplements was updated */ undefined;
export type PostApiProjectsByProjectIdFoldersAndFolderIdSupplementsApiArg = {
  projectId: string;
  folderId: string;
  updateFolderSupplementsRequest: UpdateFolderSupplementsRequest;
};
export type PostApiProjectsByProjectIdFoldersAndFolderIdLockApiResponse = /** status 200 Success */ undefined;
export type PostApiProjectsByProjectIdFoldersAndFolderIdLockApiArg = {
  projectId: string;
  folderId: string;
  updateLockProjectFolderRequest: UpdateLockProjectFolderRequest;
};
export type PostApiProjectsByProjectIdSetupProjectcompanyApiResponse =
  /** status 200 The piecework company, workplace, CVR and P Number was updated */ undefined;
export type PostApiProjectsByProjectIdSetupProjectcompanyApiArg = {
  projectId: string;
  updateProjectCompanyRequest: UpdateProjectCompanyRequest;
};
export type PutApiProjectsByProjectIdExtraworkagreementsAndExtraWorkAgreementIdApiResponse = /** status 201 Created */ undefined;
export type PutApiProjectsByProjectIdExtraworkagreementsAndExtraWorkAgreementIdApiArg = {
  projectId: string;
  extraWorkAgreementId: string;
  updateProjectExtraWorkAgreementsRequest: UpdateProjectExtraWorkAgreementsRequest;
};
export type PutApiProjectsByProjectIdFoldersAndFolderIdDescriptionApiResponse = /** status 200 The folder description was edited */ undefined;
export type PutApiProjectsByProjectIdFoldersAndFolderIdDescriptionApiArg = {
  projectId: string;
  folderId: string;
  updateProjectFolderDescriptionRequest: UpdateProjectFolderDescriptionRequest;
};
export type PostApiProjectsByProjectIdFoldersAndFolderIdExtraworkApiResponse = /** status 200 Folder extra work is updated */ undefined;
export type PostApiProjectsByProjectIdFoldersAndFolderIdExtraworkApiArg = {
  projectId: string;
  folderId: string;
  updateProjectFolderExtraWorkRequest: UpdateProjectFolderExtraWorkRequest;
};
export type PutApiProjectsByProjectIdFoldersAndFolderIdNameApiResponse = /** status 200 The folder name was edited */ undefined;
export type PutApiProjectsByProjectIdFoldersAndFolderIdNameApiArg = {
  projectId: string;
  folderId: string;
  updateProjectFolderNameRequest: UpdateProjectFolderNameRequest;
};
export type PostApiProjectsByProjectIdProjectinformationApiResponse = /** status 200 The piecework name was updated */ undefined;
export type PostApiProjectsByProjectIdProjectinformationApiArg = {
  projectId: string;
  updateProjectInformationRequest: UpdateProjectInformationRequest;
};
export type PutApiProjectsByProjectIdProjectlumpsumApiResponse = /** status 200 The project lump sum has been updated */ undefined;
export type PutApiProjectsByProjectIdProjectlumpsumApiArg = {
  projectId: string;
  updateProjectLumpSumRequest: UpdateProjectLumpSumRequest;
};
export type PostApiProjectsByProjectIdProjectspecificoperationAndProjectSpecificOperationIdApiResponse = /** status 201 Created */ undefined;
export type PostApiProjectsByProjectIdProjectspecificoperationAndProjectSpecificOperationIdApiArg = {
  projectId: string;
  projectSpecificOperationId: string;
  updateProjectSpecificOperationRequest: UpdateProjectSpecificOperationRequest;
};
export type PostApiProjectsByProjectIdSetupProjecttypeApiResponse = /** status 200 The piecework type was updated */ undefined;
export type PostApiProjectsByProjectIdSetupProjecttypeApiArg = {
  projectId: string;
  updateProjectTypeRequest: UpdateProjectTypeRequest;
};
export type PutApiProjectsByProjectIdFoldersAndFolderIdWorkItemsWorkItemIdApiResponse = /** status 200 The work item was updated */ undefined;
export type PutApiProjectsByProjectIdFoldersAndFolderIdWorkItemsWorkItemIdApiArg = {
  projectId: string;
  folderId: string;
  workItemId: string;
  updateWorkItemRequest: UpdateWorkItemRequest;
};
export type PostApiProjectsByProjectIdDocumentsApiResponse = /** status 201 Created */ undefined;
export type PostApiProjectsByProjectIdDocumentsApiArg = {
  projectId: string;
  body: {
    ContentType?: string;
    ContentDisposition?: string;
    Headers?: {
      [key: string]: string[];
    };
    Length?: number;
    Name?: string;
    FileName?: string;
  };
};
export type PostApiProjectsByProjectIdFoldersAndFolderIdDocumentsApiResponse = /** status 201 Created */ undefined;
export type PostApiProjectsByProjectIdFoldersAndFolderIdDocumentsApiArg = {
  projectId: string;
  folderId: string;
  body: {
    ContentType?: string;
    ContentDisposition?: string;
    Headers?: {
      [key: string]: string[];
    };
    Length?: number;
    Name?: string;
    FileName?: string;
  };
};
export type ErrorDetail = {
  code?: string | null;
  field?: string | null;
  attemptedValue?: any | null;
  message?: string | null;
};
export type ValidationError = {
  type?: string | null;
  title?: string | null;
  detail?: string | null;
  instance?: string | null;
  status?: number;
  traceId?: string | null;
  errors?: ErrorDetail[] | null;
};
export type Error = {
  type?: string | null;
  title?: string | null;
  detail?: string | null;
  instance?: string | null;
  status?: number;
  traceId?: string | null;
};
export type CloseProjectLogBookWeekRequest = {
  userId?: string;
  year?: number;
  week?: number;
};
export type AzureAdB2C = {
  clientId?: string | null;
  authority?: string | null;
  knownAuthority?: string | null;
};
export type FeatureFlags = object;
export type ConfigResponse = {
  azureAdB2C?: AzureAdB2C;
  featureFlags?: FeatureFlags;
  maxUploadFileSizeMB?: number;
};
export type CopyProjectFolderRequest = {
  destinationFolderId?: string;
};
export type ProblemDetails = {
  type?: string | null;
  title?: string | null;
  status?: number | null;
  detail?: string | null;
  instance?: string | null;
  [key: string]: any;
};
export type CopyWorkItemsRequest = {
  destinationFolderId?: string;
  workItemIds?: string[] | null;
};
export type ExtraWorkAgreementTypeRequest = "CustomerHours" | "CompanyHours" | "AgreedPayment" | "Other";
export type CreateExtraWorkAgreementWorkTime = {
  hours?: number;
  minutes?: number;
};
export type CreateExtraWorkAgreementRequest = {
  extraWorkAgreementNumber?: string | null;
  name?: string | null;
  description?: string | null;
  extraWorkAgreementType?: ExtraWorkAgreementTypeRequest;
  paymentDkr?: number | null;
  workTime?: CreateExtraWorkAgreementWorkTime;
};
export type RemoveExtraWorkAgreementRequest = {
  extraWorkAgreementIds?: string[] | null;
};
export type PieceworkType = "TwelveOneA" | "TwelveOneB" | "TwelveOneC" | "TwelveTwo";
export type CreateProjectRequest = {
  title?: string | null;
  description?: string | null;
  pieceworkType?: PieceworkType;
  pieceworkSum?: number | null;
};
export type ProjectResponse = {
  projectId?: string;
  projectName?: string | null;
  description?: string | null;
  pieceworkType?: PieceworkType;
};
export type GetProjectsResponse = {
  projects?: ProjectResponse[] | null;
};
export type CreateProjectFolderRequest = {
  folderName?: string | null;
  folderDescription?: string | null;
  parentFolderId?: string | null;
};
export type DocumentReferenceResponse = {
  documentId?: string;
  name?: string | null;
  uploadedTimestamp?: string;
};
export type ProjectFolderLock = "Locked" | "Unlocked";
export type ExtraWork = "ExtraWork" | "NormalWork";
export type BaseRateAndSupplementsValueStatus = "Inherit" | "Overwrite";
export type BaseRateAndSupplementsValueResponse = {
  valueStatus?: BaseRateAndSupplementsValueStatus;
  value?: number;
};
export type BaseRateAndSupplementsResponse = {
  indirectTimeSupplementPercentage?: BaseRateAndSupplementsValueResponse;
  siteSpecificTimeSupplementPercentage?: BaseRateAndSupplementsValueResponse;
  baseRateRegulationPercentage?: BaseRateAndSupplementsValueResponse;
  combinedSupplementPercentage?: number;
  baseRatePerMinDkr?: number;
  personalTimeSupplementPercentage?: number;
};
export type FolderSupplementResponse = {
  supplementId?: string;
  supplementNumber?: string | null;
  supplementText?: string | null;
};
export type ProjectFolderResponse = {
  projectFolderId?: string;
  projectFolderName?: string | null;
  projectFolderDescription?: string | null;
  createdBy?: string | null;
  createdTime?: string;
  subFolders?: ProjectFolderResponse[] | null;
  documents?: DocumentReferenceResponse[] | null;
  projectFolderLocked?: ProjectFolderLock;
  folderExtraWork?: ExtraWork;
  baseRateAndSupplements?: BaseRateAndSupplementsResponse;
  folderSupplements?: FolderSupplementResponse[] | null;
};
export type GetProjectFolderRootQueryResponse = {
  projectId?: string;
  rootFolder?: ProjectFolderResponse;
};
export type GetCompensationPaymentParticipantResponse = {
  projectParticipantId?: string;
  name?: string | null;
  email?: string | null;
  hours?: number;
  payment?: number;
};
export type GetCompensationPaymentResponse = {
  startDate?: string;
  endDate?: string;
  participants?: GetCompensationPaymentParticipantResponse[] | null;
};
export type GetCompensationPaymentParticipantsInPeriodRequest = {
  startDate?: string;
  endDate?: string;
  amount?: number;
};
export type GetExtraWorkAgreementRatesQueryResponse = {
  customerRatePerHourDkr?: number;
  companyRatePerHourDkr?: number;
};
export type UpdateExtraWorkAgreementRatesRequest = {
  customerRatePrHour?: number;
  companyRatePrHour?: number;
};
export type ExtraWorkAgreementTypeResponse = "CustomerHours" | "CompanyHours" | "AgreedPayment" | "Other";
export type ExtraWorkAgreementWorkTime = {
  hours?: number;
  minutes?: number;
};
export type ExtraWorkAgreementResponse = {
  extraWorkAgreementId?: string;
  extraWorkAgreementNumber?: string | null;
  name?: string | null;
  description?: string | null;
  extraWorkAgreementType?: ExtraWorkAgreementTypeResponse;
  paymentDkr?: number | null;
  workTime?: ExtraWorkAgreementWorkTime;
};
export type ExtraWorkAgreementsResponse = {
  totalPaymentDkr?: number;
  extraWorkAgreements?: ExtraWorkAgreementResponse[] | null;
};
export type GroupedWorkItemsResponse = {
  id?: string | null;
  text?: string | null;
  amount?: number;
  paymentDkr?: number;
};
export type GetGroupedWorkItemsQueryResponse = {
  groupedWorkItems?: GroupedWorkItemsResponse[] | null;
};
export type SupplementOperationType = "AmountRelated" | "UnitRelated";
export type SupplementOperationResponse = {
  supplementOperationId?: string;
  text?: string | null;
  type?: SupplementOperationType;
  operationTimeMilliseconds?: number;
};
export type MaterialMountingResponse = {
  mountingCode?: number;
  text?: string | null;
  operationTimeMilliseconds?: number;
  supplementOperations?: SupplementOperationResponse[] | null;
};
export type GetMaterialResponse = {
  id?: string;
  eanNumber?: string | null;
  name?: string | null;
  unit?: string | null;
  mountings?: MaterialMountingResponse[] | null;
};
export type ProjectParticipant = {
  userId?: string;
  name?: string | null;
};
export type ProjectRole = "ProjectManager" | "ProjectOwner" | "ProjectParticipant" | "Undefined";
export type GetProjectResponse = {
  id?: string;
  title?: string | null;
  projectNumber?: number;
  pieceWorkNumber?: string | null;
  orderNumber?: string | null;
  description?: string | null;
  pieceworkType?: PieceworkType;
  lumpSumPaymentDkr?: number | null;
  startDate?: string | null;
  endDate?: string | null;
  projectCreatedTime?: string;
  companyName?: string | null;
  companyAddress?: string | null;
  companyCvrNo?: string | null;
  companyPNo?: string | null;
  participants?: ProjectParticipant[] | null;
  currentUserRole?: ProjectRole;
};
export type CatalogItemType = "Material" | "Operation";
export type FavoritesResponse = {
  favoriteItemId?: string;
  catalogId?: string;
  text?: string | null;
  number?: string | null;
  unit?: string | null;
  catalogType?: CatalogItemType;
};
export type GetProjectFavoriteListQueryResponse = {
  favorites?: FavoritesResponse[] | null;
};
export type FavoriteCatalogType = "Material" | "Operation";
export type RegisterProjectCatalogFavoriteRequest = {
  catalogId?: string;
  catalogType?: FavoriteCatalogType;
};
export type RemoveProjectFavoritesRequest = {
  favoriteIds?: string[] | null;
};
export type ProjectCompensationParticipant = {
  compensationParticipantId?: string;
  participantName?: string | null;
  participantEmail?: string | null;
  closedHoursInPeriod?: number;
  compensationAmountDkr?: number;
};
export type CompensationResponse = {
  projectCompensationId?: string;
  startDate?: string;
  endDate?: string;
  compensationPaymentDkr?: number;
  compensationParticipant?: ProjectCompensationParticipant[] | null;
};
export type GetProjectCompensationListQueryResponse = {
  compensations?: CompensationResponse[] | null;
};
export type RegisterCompensationRequest = {
  compensationPayment?: number;
  compensationStartDate?: string;
  compensationEndDate?: string;
  compensationParticipantIds?: string[] | null;
};
export type RemoveCompensationPaymentsRequest = {
  compensationPaymentIds?: string[] | null;
};
export type GetProjectFolderSummationQueryResponse = {
  totalWorkTimeMilliseconds?: number;
  totalPaymentDkr?: number;
  totalExtraWorkTimeMilliseconds?: number;
  totalExtraPaymentDkr?: number;
};
export type GetProjectSummationQueryResponse = {
  totalWorkItemPaymentDkr?: number;
  totalWorkItemExtraWorkPaymentDkr?: number;
  totalExtraWorkAgreementDkr?: number;
  totalLogBookHours?: number;
  totalPaymentDkr?: number;
  totalLumpSumDkr?: number;
  totalCalculationSumDkr?: number;
};
export type ProjectUserRole = "Owner" | "Participant" | "Manager";
export type ProjectUserResponse = {
  id?: string;
  name?: string | null;
  role?: ProjectUserRole;
  email?: string | null;
  phone?: string | null;
  address?: string | null;
};
export type GetProjectInfoReportResponse = {
  project?: GetProjectResponse;
  rootFolder?: ProjectFolderResponse;
  extraWorkAgreementsRates?: GetExtraWorkAgreementRatesQueryResponse;
  projectSummation?: GetProjectSummationQueryResponse;
  users?: ProjectUserResponse[] | null;
  groupedWorkitems?: GroupedWorkItemsResponse[] | null;
};
export type LogBookUserResponse = {
  name?: string | null;
  userId?: string;
};
export type GetProjectLogBookQueryResponse = {
  projectId?: string;
  users?: LogBookUserResponse[] | null;
};
export type LogBookTimeResponse = {
  hours?: number;
  minutes?: number;
};
export type GetProjectLogBookDayResponse = {
  date?: string;
  time?: LogBookTimeResponse;
};
export type LogBookSalaryAdvanceTimeResponse = {
  year?: number;
  week?: number;
};
export type LogBookSalaryAdvanceRoleResponse = "Participant" | "Apprentice" | "Undefined";
export type LogBookSalaryAdvanceResponse = {
  start?: LogBookSalaryAdvanceTimeResponse;
  end?: LogBookSalaryAdvanceTimeResponse;
  amount?: number;
  role?: LogBookSalaryAdvanceRoleResponse;
};
export type GetProjectLogBookWeekQueryResponse = {
  year?: number;
  week?: number;
  userName?: string | null;
  note?: string | null;
  closed?: boolean;
  weekSummation?: LogBookTimeResponse;
  closedWeeksSummation?: LogBookTimeResponse;
  days?: GetProjectLogBookDayResponse[] | null;
  salaryAdvance?: LogBookSalaryAdvanceResponse;
};
export type GetProjectSpecificOperationResponse = {
  projectSpecificOperationId?: string;
  extraWorkAgreementNumber?: string | null;
  name?: string | null;
  description?: string | null;
  operationTimeMs?: number;
  workingTimeMs?: number;
  payment?: number;
};
export type GetProjectSpecificOperationsListResponse = {
  projectSpecificOperations?: GetProjectSpecificOperationResponse[] | null;
};
export type RegisterProjectSpecificOperationRequest = {
  extraWorkAgreementNumber?: string | null;
  name?: string | null;
  description?: string | null;
  operationTimeMs?: number;
  workingTimeMs?: number;
};
export type RemoveProjectSpecificOperationsRequest = {
  projectSpecificOperationIds?: string[] | null;
};
export type GetProjectUsersQueryResponse = {
  users?: ProjectUserResponse[] | null;
};
export type UserRole = "ProjectManager" | "ProjectParticipant";
export type RegisterProjectUserRequest = {
  name?: string | null;
  role?: UserRole;
  email?: string | null;
  address?: string | null;
  phone?: string | null;
};
export type SupplementResponse = {
  supplementId?: string;
  supplementNumber?: string | null;
  supplementText?: string | null;
  supplementPercentage?: number;
};
export type GetSupplementsResponse = {
  supplements?: SupplementResponse[] | null;
};
export type GetWorkItemMaterialPreviewQueryResponse = {
  operationTimeMilliseconds?: number;
  totalWorkTimeMilliseconds?: number;
  workItemTotalPaymentDkr?: number;
};
export type GetWorkItemMaterialPreviewSupplementOperationRequest = {
  supplementOperationId?: string;
  amount?: number;
};
export type GetWorkItemMaterialPreviewSupplementRequest = {
  supplementId?: string;
};
export type GetWorkItemMaterialPreviewRequest = {
  materialId?: string;
  workItemAmount?: number;
  workItemMountingCode?: number;
  supplementOperations?: GetWorkItemMaterialPreviewSupplementOperationRequest[] | null;
  supplements?: GetWorkItemMaterialPreviewSupplementRequest[] | null;
};
export type GetWorkItemOperationPreviewQueryResponse = {
  operationTimeMilliseconds?: number;
  totalWorkTimeMilliseconds?: number;
  workItemTotalPaymentDkr?: number;
};
export type GetWorkItemOperationPreviewSupplementRequest = {
  supplementId?: string;
};
export type GetWorkItemOperationPreviewRequest = {
  operationId?: string;
  workItemAmount?: number;
  supplements?: GetWorkItemOperationPreviewSupplementRequest[] | null;
};
export type WorkItemSupplementResponse = {
  supplementId?: string;
  supplementNumber?: string | null;
  supplementText?: string | null;
};
export type WorkItemType = "Material" | "Operation";
export type WorkItemSupplementOperationType = "AmountRelated" | "UnitRelated";
export type WorkItemSupplementOperationResponse = {
  supplementOperationId?: string;
  text?: string | null;
  operationType?: WorkItemSupplementOperationType;
  operationTimeMilliseconds?: number;
  amount?: number;
};
export type WorkItemMaterialResponse = {
  workItemEanNumber?: string | null;
  workItemMountingCode?: number;
  workItemMountingCodeText?: string | null;
  supplementOperations?: WorkItemSupplementOperationResponse[] | null;
};
export type WorkItemOperationResponse = {
  operationNumber?: string | null;
};
export type WorkItemResponse = {
  workItemId?: string;
  workItemText?: string | null;
  workItemDate?: string;
  workItemAmount?: number;
  workItemOperationTimeMilliseconds?: number;
  workItemTotalOperationTimeMilliseconds?: number;
  workItemTotalPaymentDkr?: number;
  supplements?: WorkItemSupplementResponse[] | null;
  workItemType?: WorkItemType;
  workItemMaterial?: WorkItemMaterialResponse;
  workItemOperation?: WorkItemOperationResponse;
};
export type GetWorkItemsQueryResponse = {
  projectId?: string;
  projectFolderId?: string;
  workItems?: WorkItemResponse[] | null;
};
export type RemoveWorkItemRequest = {
  workItemIds?: string[] | null;
};
export type MoveProjectFolderRequest = {
  folderId?: string;
  destinationFolderId?: string;
};
export type MoveWorkItemsRequest = {
  destinationFolderId?: string;
  workItemIds?: string[] | null;
};
export type OpenProjectLogBookWeekRequest = {
  userId?: string;
  year?: number;
  week?: number;
};
export type LogBookSalaryAdvanceRoleRequest = "Participant" | "Apprentice";
export type RegisterLogbookSalaryAdvanceRequest = {
  userId?: string;
  year?: number;
  week?: number;
  type?: LogBookSalaryAdvanceRoleRequest;
  amount?: number;
};
export type RegisterProjectLogBookDay = {
  date?: string;
  hours?: number;
  minutes?: number;
};
export type RegisterProjectLogBookWeekRequest = {
  userId?: string;
  year?: number;
  week?: number;
  note?: string | null;
  days?: RegisterProjectLogBookDay[] | null;
};
export type MaterialSupplementOperationRequest = {
  supplementOperationId?: string;
  amount?: number;
};
export type MaterialSupplementRequest = {
  supplementId?: string;
};
export type RegisterWorkItemMaterialRequest = {
  materialId?: string;
  workItemAmount?: number;
  workItemMountingCode?: number;
  supplementOperations?: MaterialSupplementOperationRequest[] | null;
  supplements?: MaterialSupplementRequest[] | null;
};
export type OperationSupplementRequest = {
  supplementId?: string;
};
export type RegisterWorkItemOperationRequest = {
  operationId?: string;
  workItemAmount?: number;
  supplements?: OperationSupplementRequest[] | null;
};
export type FoundMaterial = {
  id?: string;
  eanNumber?: string | null;
  name?: string | null;
  unit?: string | null;
};
export type SearchMaterialResponse = {
  foundMaterials?: FoundMaterial[] | null;
};
export type SearchMaterialsRequest = {
  searchString?: string | null;
  maxHits?: number;
};
export type FoundOperation = {
  operationId?: string;
  operationNumber?: string | null;
  operationText?: string | null;
};
export type SearchOperationResponse = {
  foundOperations?: FoundOperation[] | null;
};
export type SearchOperationsRequest = {
  searchString?: string | null;
  maxHits?: number;
};
export type BaseRateStatusUpdate = "Inherit" | "Overwrite";
export type BaseRateUpdate = {
  value?: number;
  status?: BaseRateStatusUpdate;
};
export type UpdateBaseRateRequest = {
  baseRateRegulationPercentage?: BaseRateUpdate;
};
export type BaseSupplementStatusUpdate = "Inherit" | "Overwrite";
export type BaseSupplementUpdate = {
  value?: number;
  status?: BaseSupplementStatusUpdate;
};
export type UpdateBaseSupplementsRequest = {
  indirectTimePercentage?: BaseSupplementUpdate;
  siteSpecificTimePercentage?: BaseSupplementUpdate;
};
export type UpdateFolderSupplementsRequest = {
  folderSupplements?: string[] | null;
};
export type Lock = "Locked" | "Unlocked";
export type UpdateLockProjectFolderRequest = {
  folderLock?: Lock;
  recursive?: boolean;
};
export type UpdateProjectCompanyRequest = {
  company?: string | null;
  workplaceAdr?: string | null;
  cvrNumber?: string | null;
  pNumber?: string | null;
};
export type UpdateExtraWorkAgreementType = "CustomerHours" | "CompanyHours" | "AgreedPayment" | "Other";
export type UpdateExtraWorkAgreementWorkTime = {
  hours?: number;
  minutes?: number;
};
export type UpdateProjectExtraWorkAgreementsRequest = {
  extraWorkAgreementNumber?: string | null;
  name?: string | null;
  description?: string | null;
  extraWorkAgreementType?: UpdateExtraWorkAgreementType;
  paymentDkr?: number | null;
  workTime?: UpdateExtraWorkAgreementWorkTime;
};
export type UpdateProjectFolderDescriptionRequest = {
  projectFolderDescription?: string | null;
};
export type ExtraWorkUpdate = "ExtraWork" | "NormalWork";
export type UpdateProjectFolderExtraWorkRequest = {
  folderExtraWorkUpdate?: ExtraWorkUpdate;
};
export type UpdateProjectFolderNameRequest = {
  projectFolderName?: string | null;
};
export type UpdateProjectInformationRequest = {
  name?: string | null;
  description?: string | null;
  orderNumber?: string | null;
  pieceworkNumber?: string | null;
};
export type UpdateProjectLumpSumRequest = {
  lumpSumDkr?: number;
};
export type UpdateProjectSpecificOperationRequest = {
  extraWorkAgreementNumber?: string | null;
  name?: string | null;
  description?: string | null;
  operationTimeMs?: number;
  workingTimeMs?: number;
};
export type UpdateProjectPieceworkType = "TwelveOneA" | "TwelveOneB" | "TwelveOneC" | "TwelveTwo";
export type UpdateProjectTypeRequest = {
  pieceworkType?: UpdateProjectPieceworkType;
  pieceWorkSum?: number | null;
  startDate?: string | null;
  endDate?: string | null;
};
export type UpdateWorkItemRequest = {
  workItemAmount?: number;
};
export const {
  usePostApiProjectsByProjectIdLogbookWeeksCloseMutation,
  useGetApiConfigQuery,
  usePutApiProjectsByProjectIdFoldersAndSourceFolderIdCopyMutation,
  usePostApiProjectsByProjectIdFoldersAndSourceFolderIdWorkitemsCopyMutation,
  usePostApiProjectsByProjectIdExtraworkagreementsMutation,
  useDeleteApiProjectsByProjectIdExtraworkagreementsMutation,
  usePostApiProjectsMutation,
  useGetApiProjectsQuery,
  usePostApiProjectsByProjectIdFoldersMutation,
  useGetApiProjectsByProjectIdFoldersQuery,
  usePostApiProjectsByProjectIdCompensationsParticipantsMutation,
  useGetApiDocumentsByDocumentIdQuery,
  useGetApiProjectsByProjectIdExtraworkagreementsRatesQuery,
  usePutApiProjectsByProjectIdExtraworkagreementsRatesMutation,
  useGetApiProjectsByProjectIdExtraworkgreementsQuery,
  useGetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsGroupedQuery,
  useGetApiProjectsByProjectIdReportsLogbookspreadsheetQuery,
  useGetApiCatalogMaterialsByMaterialIdQuery,
  useGetApiProjectsByProjectIdQuery,
  useDeleteApiProjectsByProjectIdMutation,
  useGetApiProjectsByProjectIdFavoritesQuery,
  usePostApiProjectsByProjectIdFavoritesMutation,
  useDeleteApiProjectsByProjectIdFavoritesMutation,
  useGetApiProjectsByProjectIdCompensationsQuery,
  usePostApiProjectsByProjectIdCompensationsMutation,
  useDeleteApiProjectsByProjectIdCompensationsMutation,
  useGetApiProjectsByProjectIdFoldersAndFolderIdSummationQuery,
  useGetApiProjectsByProjectIdReportsProjectinfoQuery,
  useGetApiProjectsByProjectIdLogbookQuery,
  useGetApiProjectsByProjectIdLogbookAndUserIdWeeksYearWeekQuery,
  useGetApiProjectsByProjectIdLogbookAndUserIdWeeksYearMonthDayQuery,
  useGetApiProjectsByProjectIdProjectspecificoperationQuery,
  usePostApiProjectsByProjectIdProjectspecificoperationMutation,
  useDeleteApiProjectsByProjectIdProjectspecificoperationMutation,
  useGetApiProjectsByProjectIdSummationQuery,
  useGetApiProjectsByProjectIdUsersQuery,
  usePostApiProjectsByProjectIdUsersMutation,
  useGetApiProjectsByProjectIdReportsStatusreportspreatsheetQuery,
  useGetApiCatalogSupplementsQuery,
  usePostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMaterialPreviewMutation,
  usePostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsOperationPreviewMutation,
  useGetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsQuery,
  useDeleteApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMutation,
  useGetApiProjectsByProjectIdReportsWorkitemsspreadsheetQuery,
  usePutApiProjectsByProjectIdFoldersMoveMutation,
  usePutApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMoveMutation,
  usePostApiProjectsByProjectIdLogbookWeeksOpenMutation,
  useGetApiPingQuery,
  usePostApiProjectsByProjectIdLogbookSalaryadvanceMutation,
  usePostApiProjectsByProjectIdLogbookWeeksMutation,
  usePostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMaterialMutation,
  usePostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsOperationMutation,
  useDeleteApiProjectsByProjectIdFoldersAndFolderIdDocumentsDocumentIdMutation,
  useDeleteApiProjectsByProjectIdFoldersAndFolderIdMutation,
  usePostApiCatalogMaterialsSearchMutation,
  usePostApiCatalogOperationsSearchMutation,
  usePutApiProjectsByProjectIdFoldersAndFolderIdBaserateMutation,
  usePutApiProjectsByProjectIdFoldersAndFolderIdBasesupplementsMutation,
  usePostApiProjectsByProjectIdFoldersAndFolderIdSupplementsMutation,
  usePostApiProjectsByProjectIdFoldersAndFolderIdLockMutation,
  usePostApiProjectsByProjectIdSetupProjectcompanyMutation,
  usePutApiProjectsByProjectIdExtraworkagreementsAndExtraWorkAgreementIdMutation,
  usePutApiProjectsByProjectIdFoldersAndFolderIdDescriptionMutation,
  usePostApiProjectsByProjectIdFoldersAndFolderIdExtraworkMutation,
  usePutApiProjectsByProjectIdFoldersAndFolderIdNameMutation,
  usePostApiProjectsByProjectIdProjectinformationMutation,
  usePutApiProjectsByProjectIdProjectlumpsumMutation,
  usePostApiProjectsByProjectIdProjectspecificoperationAndProjectSpecificOperationIdMutation,
  usePostApiProjectsByProjectIdSetupProjecttypeMutation,
  usePutApiProjectsByProjectIdFoldersAndFolderIdWorkItemsWorkItemIdMutation,
  usePostApiProjectsByProjectIdDocumentsMutation,
  usePostApiProjectsByProjectIdFoldersAndFolderIdDocumentsMutation,
} = injectedRtkApi;
