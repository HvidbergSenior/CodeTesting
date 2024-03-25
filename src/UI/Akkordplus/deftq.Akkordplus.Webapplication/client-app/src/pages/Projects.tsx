import { useMsal } from "@azure/msal-react";
import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import Typography from "@mui/material/Typography";
import { useEffect, useMemo, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";
import TableSortLabel from "@mui/material/TableSortLabel";
import { ProjectResponse, useGetApiProjectsQuery } from "api/generatedApi";
import { OfflineDialogButtonless } from "components/shared/offline-dialog/offline-dialog-buttonless";
import { useOrder } from "shared/sorting/use-order";
import { useToast } from "shared/toast/hooks/use-toast";
import { useOfflineStorage } from "utils/offline-storage/use-offline-storage";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { sortCompareString } from "utils/compares";
import { useOnlineStatus } from "utils/connectivity/hook/use-online-status";
import { ProjectTableRow } from "./project/row/project-row";

interface Props {
  selectLastUsedProject: boolean;
}

export interface TableHeaderProjects {
  id: ProjectSortableId;
  title: string;
  sortable: boolean;
  testid: string;
  align: "center" | "left";
}

export type ProjectSortableId = "Id" | "Name" | "PieceWorkType" | "Description" | "Delete";

export const ProjectsPage = (props: Props) => {
  const { selectLastUsedProject } = props;
  const { t } = useTranslation();
  const tRef = useRef(t);
  const toast = useToast();
  const toastRef = useRef(toast);
  const { instance } = useMsal();
  const account = instance.getActiveAccount();
  const [skip, setSkip] = useState(true);
  const { data, error: projectError } = useGetApiProjectsQuery(undefined, { skip: skip });
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();
  const navigateRef = useRef(navigate);
  const { getOfflineProjectId, setOfflineProjectId } = useOfflineStorage();
  const { direction, orderBy, getLabelProps } = useOrder<ProjectSortableId>("Name");
  const { screenSize } = useScreenSize();
  const padHorizontal = screenSize === ScreenSizeEnum.Mobile ? 2 : 8;
  const padVertial = screenSize === ScreenSizeEnum.Mobile ? 4 : 8;

  const isOnline = useOnlineStatus();

  useEffect(() => {
    if (account) {
      setSkip(false);
      setLoading(false);
      if (!selectLastUsedProject) {
        return;
      }
      const activeProjectId = getOfflineProjectId();
      if (data?.projects && data.projects.length > 0) {
        let projectId = "";
        if (data.projects.length === 1) {
          projectId = data.projects[0].projectId ?? "";
        } else if (activeProjectId && data.projects.some((project) => project.projectId === activeProjectId)) {
          projectId = activeProjectId;
        } else if (data.projects.length > 1) {
          projectId = data.projects[0].projectId ?? "";
        }

        if (projectId !== "") {
          setOfflineProjectId(projectId);
          return navigateRef.current("/projects/" + projectId);
        }
      }
    }
    if (projectError) {
      toastRef.current.error(tRef.current("project.getProjects.error"));
    }
  }, [account, navigateRef, data, selectLastUsedProject, projectError, toastRef, tRef, getOfflineProjectId, setOfflineProjectId]);

  const headerConfigDesktop: TableHeaderProjects[] = [
    {
      id: "Name",
      title: "Navn",
      sortable: true,
      testid: "TestId",
      align: "left",
    },

    {
      id: "PieceWorkType",
      title: "Akkord type",
      sortable: true,
      testid: "TestId",
      align: "center",
    },
    {
      id: "Description",
      title: "Beskrivelse",
      sortable: false,
      testid: "TestId",
      align: "left",
    },
    {
      id: "Delete",
      title: "Slet projekt",
      sortable: false,
      testid: "TestId",
      align: "center",
    },
  ];

  const headerConfigMobile: TableHeaderProjects[] = [
    {
      id: "Name",
      title: "Navn",
      sortable: true,
      testid: "TestId",
      align: "left",
    },

    {
      id: "PieceWorkType",
      title: "Akkord type",
      sortable: true,
      testid: "TestId",
      align: "left",
    },
    {
      id: "Delete",
      title: "Slet projekt",
      sortable: false,
      testid: "TestId",
      align: "center",
    },
  ];

  const headerConfig = screenSize === ScreenSizeEnum.Mobile ? headerConfigMobile : headerConfigDesktop;

  const sortedData = useMemo(() => {
    let sortedList: ProjectResponse[] = [];
    if (data) {
      sortedList = data.projects ?? [];
    }

    switch (orderBy) {
      case "Name":
        sortedList = [...sortedList].sort((a, b) => sortCompareString(direction, a?.projectName, b?.projectName));
        break;
      case "Id":
        sortedList = [...sortedList].sort((a, b) => sortCompareString(direction, a?.projectId, b?.projectId));
        break;
      case "PieceWorkType":
        sortedList = [...sortedList].sort((a, b) => sortCompareString(direction, a?.pieceworkType, b?.pieceworkType));
        break;
    }

    return sortedList;
  }, [data, orderBy, direction]);

  return (
    <Box>
      <OfflineDialogButtonless isOpen={!isOnline} />
      {loading && (
        <Box sx={{ width: "100%", height: "100%", display: "flex", justifyContent: "center", alignItems: "center" }}>
          <CircularProgress size={150} />
        </Box>
      )}
      {!loading && (
        <Box sx={{ height: "100%", display: "flex", flexDirection: "column", justifyContent: "start" }}>
          <Typography pt={padVertial} pl={padHorizontal} color={"primary.main"} variant="h5">
            {t("project.title")}
          </Typography>
          <Typography pt={2} pl={padHorizontal} color={"grey.50"} variant="body2">
            {t("project.description")}
          </Typography>

          <Box pt={4} pl={padHorizontal} pr={padHorizontal}>
            <TableContainer sx={{ maxHeight: "700px", overflow: "hide", height: "calc(100vh - 300px)" }} component={Paper}>
              <Table stickyHeader>
                <TableHead sx={{ backgroundColor: "primary.light" }}>
                  <TableRow>
                    {headerConfig.map((cell, index) => {
                      if (!cell.sortable) {
                        return (
                          <TableCell
                            data-testid={cell.testid}
                            key={cell.id}
                            sx={{ color: "primary.main", backgroundColor: "primary.light", textAlign: cell.align }}
                          >
                            {cell.title}
                          </TableCell>
                        );
                      }
                      if (index === 0) {
                        return (
                          <TableCell data-testid={cell.testid} key={cell.id} sx={{ color: "primary.main", backgroundColor: "primary.light" }}>
                            <TableSortLabel {...getLabelProps(cell.id)}>{cell.title}</TableSortLabel>
                          </TableCell>
                        );
                      } else {
                        return (
                          <TableCell
                            data-testid={cell.testid}
                            key={cell.id}
                            sx={{ color: "primary.main", backgroundColor: "primary.light", textAlign: cell.align }}
                          >
                            <TableSortLabel {...getLabelProps(cell.id)}>{cell.title}</TableSortLabel>
                          </TableCell>
                        );
                      }
                    })}
                  </TableRow>
                </TableHead>
                <TableBody>
                  {sortedData.map((project) => {
                    return <ProjectTableRow key={project.projectId} project={project} />;
                  })}
                </TableBody>
              </Table>
            </TableContainer>
          </Box>
        </Box>
      )}
    </Box>
  );
};
