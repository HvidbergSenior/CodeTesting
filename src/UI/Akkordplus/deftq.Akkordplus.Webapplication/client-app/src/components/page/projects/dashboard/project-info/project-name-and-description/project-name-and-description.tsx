import Grid from "@mui/material/Grid";
import { GetProjectResponse } from "api/generatedApi";
import { useEditProjectName } from "components/page/projects/project-setup/edit-project-name-dialog/hooks/use-edit-project-name";
import { CardWithHeaderAndFooter } from "components/shared/card/card-header-footer";
import { ShowStringDialog } from "components/shared/show-string-dialog/show-string-dialog";
import { useTranslation } from "react-i18next";
import { useDialog } from "shared/dialog/use-dialog";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { ProjectNameAndDescriptionContent } from "./project-name-and-description-content";

interface Props {
  project: GetProjectResponse;
}

export const ProjectNameAndDescription = ({ project }: Props) => {
  const { t } = useTranslation();
  const editProjectName = useEditProjectName({ project });
  const { canEditProjectSetupProjectName } = useDashboardRestrictions(project);
  const [openDescriptionDialog] = useDialog(ShowStringDialog);

  const hasDescription = (): boolean => {
    return (project.description ?? "").length > 0;
  };

  const viewDescription = () => {
    if (hasDescription() && project.description) {
      openDescriptionDialog({ stringValue: project.description, header: "common.description" });
    }
  };

  return (
    <Grid item xs={12} lg={4} xl={4} display={"flex"} justifyContent={"center"}>
      <CardWithHeaderAndFooter
        titleNamespace={t("dashboard.projectInfo.projectNameAndDescription.title")}
        description={""}
        height={"400px"}
        headerActionIcon={undefined}
        showHeaderAction={canEditProjectSetupProjectName()}
        showBottomAction={hasDescription() ? "showMore" : "none"}
        headerActionClickedProps={editProjectName}
        bottomActionClickedProps={viewDescription}
        showContent={true}
        noContentText={""}
        showDescription={false}
        hasChildPadding={true}
      >
        <ProjectNameAndDescriptionContent project={project} />
      </CardWithHeaderAndFooter>
    </Grid>
  );
};
