import { generatedApi } from "./generatedApi";

export const api = generatedApi.enhanceEndpoints({
  addTagTypes: [
    "Projects",
    "ThisProject",
    "Favorites",
    "Folders",
    "Logbook",
    "LogbookWeek",
    "FolderWorkItems",
    "WorkItemsGrouped",
    "WorkItemsSummation",
    "ExtraWorkAgreement",
    "DashboardSummary",
    "ExtraWorkAgreementRates",
    "ProjectUsers",
    "CompensationPayment",
    "ProjectSpecificOperation"
  ],
  endpoints: {
    // Projects
    getApiProjects: {
      providesTags: ["Projects"],
    },
    postApiProjects: {
      invalidatesTags: ["Projects"],
    },
    deleteApiProjectsByProjectId: {
      invalidatesTags: ["Projects"],
    },
    getApiProjectsByProjectId: {
      providesTags: ["ThisProject"]
    },
    // Favorites
    getApiProjectsByProjectIdFavorites: {
      providesTags: ["Favorites"],
    },
    postApiProjectsByProjectIdFavorites: {
      invalidatesTags: ["Favorites"],
    },
    deleteApiProjectsByProjectIdFavorites: {
      invalidatesTags: ["Favorites"],
    },
    // Folders
    getApiProjectsByProjectIdFolders: {
      providesTags: ["Folders"],
    },
    postApiProjectsByProjectIdFolders: {
      invalidatesTags: ["Folders"],
    },
    putApiProjectsByProjectIdFoldersMove: {
      invalidatesTags: ["Folders"],
    },
    putApiProjectsByProjectIdFoldersAndSourceFolderIdCopy: {
      invalidatesTags: ["Folders", "WorkItemsGrouped", "WorkItemsSummation", "DashboardSummary"],
    },
    deleteApiProjectsByProjectIdFoldersAndFolderId: {
      invalidatesTags: ["Folders", "WorkItemsGrouped", "WorkItemsSummation", "DashboardSummary"],
    },
    putApiProjectsByProjectIdFoldersAndFolderIdDescription: {
      invalidatesTags: ["Folders"],
    },
    putApiProjectsByProjectIdFoldersAndFolderIdName: {
      invalidatesTags: ["Folders"],
    },
    deleteApiProjectsByProjectIdFoldersAndFolderIdDocumentsDocumentId: {
      invalidatesTags: ["Folders"],
    },
    postApiProjectsByProjectIdFoldersAndFolderIdLock: {
      invalidatesTags: ["Folders"],
    },
    postApiProjectsByProjectIdFoldersAndFolderIdSupplements: {
      invalidatesTags: ["Folders", "WorkItemsGrouped", "WorkItemsSummation", "DashboardSummary"],
    },
    // Logbook
    getApiProjectsByProjectIdLogbook: {
      providesTags: ["Logbook"],
    },
    getApiProjectsByProjectIdLogbookAndUserIdWeeksYearMonthDay: {
      providesTags: ["LogbookWeek"],
    },
    getApiProjectsByProjectIdLogbookAndUserIdWeeksYearWeek: {
      providesTags: ["LogbookWeek"],
    },
    postApiProjectsByProjectIdLogbookWeeks: {
      invalidatesTags: ["LogbookWeek"],
    },
    postApiProjectsByProjectIdLogbookWeeksClose: {
      invalidatesTags: ["LogbookWeek", "DashboardSummary"],
    },
    postApiProjectsByProjectIdLogbookWeeksOpen: {
      invalidatesTags: ["LogbookWeek", "DashboardSummary"],
    },
    postApiProjectsByProjectIdLogbookSalaryadvance: {
      invalidatesTags: ["LogbookWeek"],
    },
    // Folder workitems
    getApiProjectsByProjectIdFoldersAndFolderIdWorkitems: {
      providesTags: ["FolderWorkItems"],
    },
    postApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMaterial: {
      invalidatesTags: ["FolderWorkItems", "WorkItemsGrouped", "WorkItemsSummation", "DashboardSummary"],
    },
    postApiProjectsByProjectIdFoldersAndFolderIdWorkitemsOperation: {
      invalidatesTags: ["FolderWorkItems", "WorkItemsGrouped", "WorkItemsSummation", "DashboardSummary"],
    },
    deleteApiProjectsByProjectIdFoldersAndFolderIdWorkitems: {
      invalidatesTags: ["FolderWorkItems", "WorkItemsGrouped", "WorkItemsSummation", "DashboardSummary"],
    },
    postApiProjectsByProjectIdFoldersAndSourceFolderIdWorkitemsCopy: {
      invalidatesTags: ["FolderWorkItems", "WorkItemsGrouped", "WorkItemsSummation", "DashboardSummary"],
    },
    putApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMove: {
      invalidatesTags: ["FolderWorkItems", "WorkItemsGrouped", "WorkItemsSummation"],
    },
    putApiProjectsByProjectIdFoldersAndFolderIdWorkItemsWorkItemId: {
      invalidatesTags: ["FolderWorkItems", "WorkItemsGrouped", "WorkItemsSummation"],
    },
    // Extra work
    postApiProjectsByProjectIdFoldersAndFolderIdExtrawork: {
      invalidatesTags: ["Folders", "WorkItemsSummation", "DashboardSummary"],
    },
    // Base rate and supplements
    putApiProjectsByProjectIdFoldersAndFolderIdBasesupplements: {
      invalidatesTags: ["Folders", "WorkItemsSummation", "DashboardSummary"],
    },
    putApiProjectsByProjectIdFoldersAndFolderIdBaserate: {
      invalidatesTags: ["Folders", "DashboardSummary"],
    },
    // WorkItemsGrouped
    getApiProjectsByProjectIdFoldersAndFolderIdWorkitemsGrouped: {
      providesTags: ["WorkItemsGrouped"],
    },
    // Summations Work Items
    getApiProjectsByProjectIdFoldersAndFolderIdSummation: {
      providesTags: ["WorkItemsSummation"],
    },
    //Extra Work Agreeemnts
    getApiProjectsByProjectIdExtraworkgreements: {
      providesTags: ["ExtraWorkAgreement"],
    },
    postApiProjectsByProjectIdExtraworkagreements: {
      invalidatesTags: ["ExtraWorkAgreement", "DashboardSummary"],
    },
    putApiProjectsByProjectIdExtraworkagreementsAndExtraWorkAgreementId: {
      invalidatesTags: ["ExtraWorkAgreement", "DashboardSummary"],
    },
    deleteApiProjectsByProjectIdExtraworkagreements: {
      invalidatesTags: ["ExtraWorkAgreement", "DashboardSummary"],
    },
    // Dashboard Summary
    getApiProjectsByProjectIdSummation: {
      providesTags: ["DashboardSummary"],
    },
    putApiProjectsByProjectIdProjectlumpsum: {
      invalidatesTags: ["DashboardSummary"]
    },
    // Extra Work Agreement Rates
    getApiProjectsByProjectIdExtraworkagreementsRates: {
      providesTags: ["ExtraWorkAgreementRates"],
    },
    putApiProjectsByProjectIdExtraworkagreementsRates: {
      invalidatesTags: ["ExtraWorkAgreement", "ExtraWorkAgreementRates"],
    },
    // Dashboard Project Info
    postApiProjectsByProjectIdProjectinformation: {
      invalidatesTags: ["ThisProject"]
    },
    postApiProjectsByProjectIdSetupProjecttype: {
      invalidatesTags: ["ThisProject"]
    },
    postApiProjectsByProjectIdSetupProjectcompany: {
      invalidatesTags: ["ThisProject"]
    },
    // Dashboard users
    getApiProjectsByProjectIdUsers: {
      providesTags: ["ProjectUsers"]
    },
    postApiProjectsByProjectIdUsers: {
      invalidatesTags: ["ProjectUsers"]
    },
    // Compensation Payment
    getApiProjectsByProjectIdCompensations: {
      providesTags: ["CompensationPayment"]
    },
    postApiProjectsByProjectIdCompensations: {
      invalidatesTags: ["CompensationPayment"]
    },
    deleteApiProjectsByProjectIdCompensations: {
      invalidatesTags: ["CompensationPayment"]
    },
    // Project Specific Operations
    getApiProjectsByProjectIdProjectspecificoperation: {
      providesTags: ["ProjectSpecificOperation"]
    },
    postApiProjectsByProjectIdProjectspecificoperation: {
      invalidatesTags: ["ProjectSpecificOperation"]
    },
    postApiProjectsByProjectIdProjectspecificoperationAndProjectSpecificOperationId: {
      invalidatesTags: ["ProjectSpecificOperation"]
    },
    deleteApiProjectsByProjectIdProjectspecificoperation: {
      invalidatesTags: ["ProjectSpecificOperation"]
    }
  },
});
