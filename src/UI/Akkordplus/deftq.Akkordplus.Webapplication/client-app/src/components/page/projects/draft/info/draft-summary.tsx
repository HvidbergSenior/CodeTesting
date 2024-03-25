import { useTranslation } from "react-i18next";
import { Fragment } from "react";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import Card from "@mui/material/Card";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import styled from "@mui/system/styled";
import { DraftWorkItem } from "utils/offline-storage/use-offline-storage";
import { padNumber } from "utils/formats";
import { costumPalette } from "theme/palette";

interface Props {
  draft: DraftWorkItem;
}

export function DraftSummary(props: Props) {
  const { draft } = props;
  const { t } = useTranslation();

  const TypographyValueLineStyled = styled(Typography)`
    margin-bottom: 10px;
  `;

  const CardNoBorderStyled = styled(Card)`
    padding: 10px 20px;
    border: 0;
  `;

  return (
    <Box sx={{ display: "flex", gap: 2, flexDirection: "column", padding: "20px" }}>
      <CardNoBorderStyled data-testid="material-container" sx={{ backgroundColor: "grey.100" }}>
        <Box sx={{ display: "flex" }}>
          <Box sx={{ flex: 1 }}>
            <Typography variant="overline">{t(`content.measurements.info.${draft.workItemType?.toLowerCase()}`)}</Typography>
            <TypographyValueLineStyled data-testid="create-workitem-info-name" variant="body1" color="primary.main">
              {draft.workItemType === "Material" ? draft.material?.name : draft.operation?.operationText}
            </TypographyValueLineStyled>
            {draft.workItemType === "Material" && (
              <Fragment>
                <Typography data-testid="create-workitem-info-ean" variant="overline">
                  {t("content.measurements.info.eanNumber")}
                </Typography>
                <TypographyValueLineStyled variant="body1" color="primary.main">
                  {draft.material?.eanNumber}
                </TypographyValueLineStyled>
              </Fragment>
            )}
            {draft.workItemType === "Operation" && (
              <Fragment>
                <Typography data-testid="create-workitem-info-number" variant="overline">
                  {t("content.measurements.info.operationNumber")}
                </Typography>
                <TypographyValueLineStyled variant="body1" color="primary.main">
                  {draft.operation?.operationNumber}
                </TypographyValueLineStyled>
              </Fragment>
            )}
          </Box>
          <Box sx={{ display: "flex", flexDirection: "column", pt: "6px", pl: 2, pr: 2, alignItems: "center" }}>
            <Typography variant="overline">{t("content.measurements.info.amount")}</Typography>
            <Typography data-testid="create-workitem-info-amount" variant="body1" color="primary.main" sx={{ pt: 1 }}>
              {draft.workItemAmount}
            </Typography>
          </Box>
        </Box>
      </CardNoBorderStyled>
      {draft.workItemMountingCode && draft.workItemMountingCode > 0 && (
        <CardNoBorderStyled data-testid="mounting-code-container" sx={{ backgroundColor: "grey.100" }}>
          <Typography variant="overline">{t("content.measurements.info.mountingCode")}</Typography>
          <Typography variant="body1" color="primary.main">{`${padNumber(draft.workItemMountingCode)} - ${draft.workItemMountingText}`}</Typography>
        </CardNoBorderStyled>
      )}
      {draft.supplementOperations && draft.supplementOperations.length > 0 && (
        <CardNoBorderStyled data-testid="operation-sup-container" sx={{ backgroundColor: costumPalette.operationSupplementBg }}>
          <Typography variant="overline">{t("content.measurements.info.operationSupplement")}</Typography>
          <List sx={{ listStyleType: "disc", pl: 2, pt: 0, pb: 0 }}>
            {draft.supplementOperations.map((so, index) => (
              <ListItem key={index} sx={{ display: "list-item", p: 0, pb: 0.5, color: "primary.main" }}>
                <Typography variant="body1">{`${so.amount} (${so.text})`}</Typography>
              </ListItem>
            ))}
          </List>
        </CardNoBorderStyled>
      )}
      {draft.supplementTexts && draft.supplementTexts.length > 0 && (
        <CardNoBorderStyled data-testid="supplement-container" sx={{ bgcolor: costumPalette.supplementBg }}>
          <Typography variant="overline">{t("content.measurements.info.supplement")}</Typography>
          <List sx={{ listStyleType: "disc", pl: 2, pt: 0, pb: 0 }}>
            {draft.supplementTexts.map((sup, index) => (
              <ListItem key={index} sx={{ display: "list-item", p: 0, pb: 0.5, color: "primary.main" }}>
                <Typography variant="body1">{sup}</Typography>
              </ListItem>
            ))}
          </List>
        </CardNoBorderStyled>
      )}
      {draft.note && draft.note !== "" && (
        <CardNoBorderStyled data-testid="note-container" sx={{ bgcolor: "grey.100" }}>
          <Typography variant="overline">{t("common.note")}</Typography>
          <Typography variant="body1" color={"primary.main"}>
            {draft.note}
          </Typography>
        </CardNoBorderStyled>
      )}
    </Box>
  );
}
