import { useTranslation } from "react-i18next";
import Grid from "@mui/material/Grid";
import { GetProjectResponse } from "api/generatedApi";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useEditPaymentFactor } from "./edit/hooks/use-edit-factor";
import { useRateRestrictions } from "shared/user-restrictions/use-rate-restrictions";
import { CardWithHeaderAndFooter } from "components/shared/card/card-header-footer";
import { PaymentFactorContent } from "./payment-factor-content";
interface Props {
  folder: ExtendedProjectFolder;
  project: GetProjectResponse;
}

export const PaymentFactor = ({ folder, project }: Props) => {
  const { t } = useTranslation();
  const editPaymentFactorDialog = useEditPaymentFactor({ folder: folder, project: project });
  const { canEditBaseRateAndSupplements } = useRateRestrictions(project);

  return (
    <Grid item xs={12} lg={4} xl={4} display={"flex"} justifyContent={"center"}>
      <CardWithHeaderAndFooter
        titleNamespace={t("content.calculation.paymentFactor.title")}
        description={t("content.calculation.paymentFactor.description")}
        height={"400px"}
        headerActionIcon={undefined}
        showHeaderAction={canEditBaseRateAndSupplements()}
        showBottomAction={"none"}
        headerActionClickedProps={editPaymentFactorDialog}
        bottomActionClickedProps={() => {}}
        showContent={true}
        noContentText={""}
        showDescription={true}
        hasChildPadding={false}
      >
        <PaymentFactorContent folder={folder} />
      </CardWithHeaderAndFooter>
    </Grid>
  );
};
