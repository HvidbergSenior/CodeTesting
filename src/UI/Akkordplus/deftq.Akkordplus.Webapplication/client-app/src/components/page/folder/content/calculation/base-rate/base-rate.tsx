import Grid from "@mui/material/Grid";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { CardWithHeaderAndFooter } from "components/shared/card/card-header-footer";
import { useTranslation } from "react-i18next";
import { useEditBaseRateAndSupplements } from "./edit/hooks/use-edit-base";
import { ProjectResponse } from "api/generatedApi";
import { useRateRestrictions } from "shared/user-restrictions/use-rate-restrictions";
import { BaseRateAndSupplementsContent } from "./edit/base-rate-and-suplements-content";

interface Props {
  folder: ExtendedProjectFolder | undefined;
  project: ProjectResponse;
}

export const BaseRateAndSupplements = ({ folder, project }: Props) => {
  const { t } = useTranslation();

  const editBaseRateAndSupplementsDialog = useEditBaseRateAndSupplements({ folder: folder, project: project });
  const { canEditBaseRateAndSupplements } = useRateRestrictions(project);

  return (
    <Grid item xs={12} lg={4} xl={4} display={"flex"} justifyContent={"center"}>
      <CardWithHeaderAndFooter
        titleNamespace={t("content.calculation.baseRateAndSupplements.title")}
        description={t("content.calculation.baseRateAndSupplements.description")}
        height={"400px"}
        headerActionIcon={undefined}
        showHeaderAction={canEditBaseRateAndSupplements()}
        showBottomAction={"none"}
        headerActionClickedProps={editBaseRateAndSupplementsDialog}
        bottomActionClickedProps={() => {}}
        showContent={true}
        noContentText={""}
        showDescription={true}
        hasChildPadding={false}
      >
        <BaseRateAndSupplementsContent folder={folder} project={project} />
      </CardWithHeaderAndFooter>
    </Grid>
  );
};
