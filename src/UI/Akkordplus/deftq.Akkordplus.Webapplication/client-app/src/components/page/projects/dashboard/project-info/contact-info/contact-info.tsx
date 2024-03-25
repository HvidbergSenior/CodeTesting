import Grid from "@mui/material/Grid";
import { GetProjectResponse } from "api/generatedApi";
import { useEditProjectCompanyAndWorkplace } from "components/page/projects/project-setup/edit-project-company-dialog/hooks/use-edit-project-company-and-workplace";
import { CardWithHeaderAndFooter } from "components/shared/card/card-header-footer";
import { useTranslation } from "react-i18next";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { ContactInfoContent } from "./contact-info-content";

interface Props {
  project: GetProjectResponse;
}

export const ContactInfo = ({ project }: Props) => {
  const { t } = useTranslation();
  const editProjectCompany = useEditProjectCompanyAndWorkplace({ project });
  const { canEditProjectSetupProjectCompany } = useDashboardRestrictions(project);

  return (
    <Grid item xs={12} lg={4} xl={4} display={"flex"} justifyContent={"center"}>
      <CardWithHeaderAndFooter
        titleNamespace={t("dashboard.projectInfo.contactInfo.title")}
        description={""}
        height={"400px"}
        headerActionIcon={undefined}
        showHeaderAction={canEditProjectSetupProjectCompany()}
        showBottomAction={"none"}
        headerActionClickedProps={editProjectCompany}
        bottomActionClickedProps={() => {}}
        showContent={true}
        noContentText={""}
        showDescription={false}
        hasChildPadding={true}
      >
        <ContactInfoContent project={project} />
      </CardWithHeaderAndFooter>
    </Grid>
  );
};
