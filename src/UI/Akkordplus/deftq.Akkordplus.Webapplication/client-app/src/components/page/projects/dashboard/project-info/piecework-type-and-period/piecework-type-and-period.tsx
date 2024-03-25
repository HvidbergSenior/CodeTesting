import Grid from "@mui/material/Grid";
import { GetProjectResponse } from "api/generatedApi";
import { useEditProjectTypeAndPeriod } from "components/page/projects/project-setup/edit-project-type-dialog/hooks/use-edit-project-type-and-period";
import { CardWithHeaderAndFooter } from "components/shared/card/card-header-footer";
import { useTranslation } from "react-i18next";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { PieceworkTypeAndPeriodContent } from "./piecework-type-and-period-content";

interface Props {
  project: GetProjectResponse;
}

export const PieceworkTypeAndPeriod = ({ project }: Props) => {
  const { t } = useTranslation();
  const editProjectType = useEditProjectTypeAndPeriod({ project });
  const { canEditProjectSetupProjectType } = useDashboardRestrictions(project);

  return (
    <Grid item xs={12} lg={4} xl={4} display={"flex"} justifyContent={"center"}>
      <CardWithHeaderAndFooter
        titleNamespace={t("dashboard.projectInfo.pieceworkTypeAndPeriod.title")}
        description={""}
        height={"400px"}
        headerActionIcon={undefined}
        showHeaderAction={canEditProjectSetupProjectType()}
        showBottomAction={"none"}
        headerActionClickedProps={editProjectType}
        bottomActionClickedProps={() => {}}
        showContent={true}
        noContentText={""}
        showDescription={false}
        hasChildPadding={false}
      >
        <PieceworkTypeAndPeriodContent project={project} usePrintlayOut={false} />
      </CardWithHeaderAndFooter>
    </Grid>
  );
};
