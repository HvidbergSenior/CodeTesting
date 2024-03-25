import Grid from "@mui/material/Grid";
import { GetProjectResponse, useGetApiProjectsByProjectIdExtraworkagreementsRatesQuery } from "api/generatedApi";
import { CardWithHeaderAndFooter } from "components/shared/card/card-header-footer";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { useEditHours } from "./edit/hook/use-edit-hours";
import { ExtraWorkAgreementContent } from "./customer-company-content";

interface Props {
  project: GetProjectResponse;
}

export const ExtraWorkAgreement = ({ project }: Props) => {
  const { t } = useTranslation();
  const { canEditExtraAgreementsRates } = useDashboardRestrictions(project);
  const { data } = useGetApiProjectsByProjectIdExtraworkagreementsRatesQuery({ projectId: project.id ?? "" });
  const openEditHours = useEditHours({ project, data });
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    if (data) {
      setIsLoading(false);
    } else {
      setIsLoading(true);
    }
  }, [data]);

  return (
    <Grid item xs={12} lg={4} xl={4} display={"flex"} justifyContent={"center"}>
      <CardWithHeaderAndFooter
        titleNamespace={t("dashboard.projectInfo.customerCompanyTitle")}
        description={t("dashboard.projectInfo.customerCompanyDescription")}
        height={"400px"}
        headerActionIcon={undefined}
        showHeaderAction={canEditExtraAgreementsRates()}
        showBottomAction={"none"}
        headerActionClickedProps={openEditHours}
        bottomActionClickedProps={() => {}}
        showContent={true}
        noContentText={""}
        showDescription={true}
        hasChildPadding={false}
        isLoading={isLoading}
      >
        {!!data && <ExtraWorkAgreementContent data={data} />}
      </CardWithHeaderAndFooter>
    </Grid>
  );
};
