import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import styled from "@mui/system/styled";
import { useTranslation } from "react-i18next";
import { GetExtraWorkAgreementRatesQueryResponse, GetProjectResponse } from "api/generatedApi";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { BaseRateAndSupplementsContent } from "components/page/folder/content/calculation/base-rate/edit/base-rate-and-suplements-content";
import { ExtraWorkAgreementContent } from "../../../project-info/customer-company/customer-company-content";
import { CardWithHeaderAndFooter } from "components/shared/card/card-header-footer";
import { PaymentFactorContent } from "components/page/folder/content/calculation/payment-factor/payment-factor-content";
import { ContentOnlyCard } from "components/shared/card/content-only-card";

interface Props {
  project: GetProjectResponse;
  folder: ExtendedProjectFolder;
  extraworkagreementsRates: GetExtraWorkAgreementRatesQueryResponse;
}

export const PrintSectionProjectTime = ({ project, folder, extraworkagreementsRates }: Props) => {
  const { t } = useTranslation();

  const PrintContainer = styled(Box)({
    "@media print": {
      breakInside: "avoid",
    },
  });

  return (
    <PrintContainer sx={{ display: "flex", gap: 2 }}>
      <Box sx={{ flex: 1 }}>
        <ContentOnlyCard height={"400px"} showContent={true} noContentText={""} hasChildPadding={true} isLoading={false}>
          <Box sx={{ display: "flex", flexDirection: "column", gap: 4, pt: 10 }}>
            <Typography variant="subtitle1">{t("dashboard.reports.printReports.projectTime.description1")}</Typography>
            <Typography variant="subtitle1">{t("dashboard.reports.printReports.projectTime.description2")}</Typography>
          </Box>
        </ContentOnlyCard>
      </Box>
      <Box sx={{ flex: 1 }}>
        <CardWithHeaderAndFooter
          titleNamespace={t("content.calculation.baseRateAndSupplements.title")}
          description={t("content.calculation.baseRateAndSupplements.description")}
          height={"400px"}
          headerActionIcon={undefined}
          showHeaderAction={false}
          showBottomAction={"none"}
          headerActionClickedProps={() => {}}
          bottomActionClickedProps={() => {}}
          showContent={true}
          noContentText={""}
          showDescription={true}
          hasChildPadding={false}
        >
          <BaseRateAndSupplementsContent project={project} folder={folder} />
        </CardWithHeaderAndFooter>
      </Box>
      <Box sx={{ flex: 1 }}>
        <CardWithHeaderAndFooter
          titleNamespace={t("dashboard.projectInfo.customerCompanyTitle")}
          description={t("dashboard.projectInfo.customerCompanyDescription")}
          height={"400px"}
          headerActionIcon={undefined}
          showHeaderAction={false}
          showBottomAction={"none"}
          headerActionClickedProps={() => {}}
          bottomActionClickedProps={() => {}}
          showContent={true}
          noContentText={""}
          showDescription={true}
          hasChildPadding={false}
        >
          <Box height={20} />
          <ExtraWorkAgreementContent data={extraworkagreementsRates} />
        </CardWithHeaderAndFooter>
      </Box>
      <Box sx={{ flex: 1 }}>
        <CardWithHeaderAndFooter
          titleNamespace={t("content.calculation.paymentFactor.title")}
          description={t("content.calculation.paymentFactor.description")}
          height={"400px"}
          headerActionIcon={undefined}
          showHeaderAction={false}
          showBottomAction={"none"}
          headerActionClickedProps={() => {}}
          bottomActionClickedProps={() => {}}
          showContent={true}
          noContentText={""}
          showDescription={true}
          hasChildPadding={false}
        >
          <PaymentFactorContent folder={folder} />
        </CardWithHeaderAndFooter>
      </Box>
    </PrintContainer>
  );
};
