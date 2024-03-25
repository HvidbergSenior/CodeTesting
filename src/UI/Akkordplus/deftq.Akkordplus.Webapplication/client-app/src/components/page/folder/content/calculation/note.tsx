import AddIcon from "@mui/icons-material/Add";
import { Grid } from "@mui/material";
import { Box } from "@mui/system";
import { CardWithHeaderAndFooter } from "components/shared/card/card-header-footer";
import { useTranslation } from "react-i18next";

export const CalculationNote = () => {
  const { t } = useTranslation();

  return (
    <Grid item display={"flex"} flexDirection={"column"} justifyContent={"center"}>
      <CardWithHeaderAndFooter
        titleNamespace={t("common.note")}
        description={""}
        height={"400px"}
        headerActionIcon={<AddIcon color="primary" />}
        showHeaderAction={true}
        showBottomAction={"none"}
        headerActionClickedProps={() => {}}
        bottomActionClickedProps={() => {}}
        showContent={false}
        noContentText={t("content.calculation.note.emptyState")}
        showDescription={false}
        hasChildPadding={false}
      >
        <Box></Box>
      </CardWithHeaderAndFooter>
    </Grid>
  );
};
