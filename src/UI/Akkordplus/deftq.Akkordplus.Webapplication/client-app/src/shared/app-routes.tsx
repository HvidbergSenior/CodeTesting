import { DefaultLayout } from "layouts/default";
import { Fragment } from "react";
import { useRoutes } from "react-router-dom";
import type { BreadcrumbsRoute } from "use-react-router-breadcrumbs";
import { DefaultProjectPage } from "pages/project/default";
import { ProjectsPage } from "pages/Projects";
import { PrintLayout } from "layouts/print-layout";
import { DefaultProjectPrintReportPage } from "components/page/projects/dashboard/reports/print-reports/default";

export const routes: BreadcrumbsRoute[] = [
  {
    path: "/projectlist",
    element: <DefaultLayout />,

    children: [
      {
        path: "",
        element: <ProjectsPage selectLastUsedProject={false} />,
      },
    ],
  },
  {
    path: "/reports/projects",
    element: <PrintLayout />,
    children: [{ path: ":projectId/projectinfo/", element: <DefaultProjectPrintReportPage subPage="projekt-info" /> }],
  },
  {
    path: "/projects",
    element: <DefaultLayout />,
    children: [
      {
        path: "",
        element: <ProjectsPage selectLastUsedProject={true} />,
      },
      {
        path: ":projectId",
        element: <DefaultProjectPage subPage="folders" />,
      },
      {
        path: ":projectId/dashboard/",
        element: <DefaultProjectPage subPage="dashboard" />,
      },
      {
        path: ":projectId/dashboard/favorites/",
        element: <DefaultProjectPage subPage="dashboard-favorites" />,
      },
      {
        path: ":projectId/dashboard/compensation-payment/",
        element: <DefaultProjectPage subPage="dashboard-compensation-payment" />,
      },
      {
        path: ":projectId/dashboard/extraworkagreements/",
        element: <DefaultProjectPage subPage="dashboard-extra-work-agreements" />,
      },
      {
        path: ":projectId/dashboard/project-info/",
        element: <DefaultProjectPage subPage="dashboard-project-info" />,
      },
      {
        path: ":projectId/dashboard/reports/",
        element: <DefaultProjectPage subPage="dashboard-reports" />,
      },
      {
        path: ":projectId/dashboard/users/",
        element: <DefaultProjectPage subPage="dashboard-users" />,
      },
      {
        path: ":projectId/dashboard/projectspecificoperations",
        element: <DefaultProjectPage subPage="dashboard-project-specific-operations" />,
      },
      {
        path: ":projectId/logbook/",
        element: <DefaultProjectPage subPage="logbook" />,
      },
      {
        path: ":projectId/draft/",
        element: <DefaultProjectPage subPage="draft" />,
      },
      {
        path: ":projectId/folders/:folderId",
        element: <DefaultProjectPage subPage="folders" />,
      },
      {
        path: ":projectId/folders/:folderId/foldercontent",
        element: <DefaultProjectPage subPage="foldercontent" />,
      },
    ],
  },
  {
    path: "*",
    element: <DefaultLayout />,

    children: [
      {
        path: "*",
        element: <ProjectsPage selectLastUsedProject={true} />,
      },
    ],
  },
];

export function AppRoutes() {
  let routeElement = useRoutes(routes);

  return <Fragment>{routeElement}</Fragment>;
}
