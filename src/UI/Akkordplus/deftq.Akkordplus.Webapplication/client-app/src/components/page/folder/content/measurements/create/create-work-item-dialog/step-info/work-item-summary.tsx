import { Fragment } from "react";
import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import { costumPalette } from "theme/palette";
import { getHmsFromMilliSeconds, formatNumberToPrice, padNumber } from "utils/formats";
import { WorkItemResponse } from "api/generatedApi";
import { CardNoBorderStyled, CardStyled, DividerStyled, TypographyValueLineStyled } from "./work-item-summary-styles";
export interface Prop {
  workItem: WorkItemResponse;
}

export function WorkItemSummary(props: Prop) {
  const { workItem } = props;
  const { t } = useTranslation();

  // KTH 2/1-23 hide for better calculation
  // const calcWorkTime = ():number => {
  //   if (!workItem ||
  //     !workItem.workItemTotalOperationTimeMilliseconds || workItem.workItemTotalOperationTimeMilliseconds === 0 ||
  //     !workItem.workItemAmount || workItem.workItemAmount === 0) {
  //       return 0
  //     }

  //   return workItem.workItemTotalOperationTimeMilliseconds / workItem.workItemAmount;
  // };

  return (
    <Box sx={{ display: "flex", gap: 2, flexDirection: "column", padding: "20px" }}>
      <Box sx={{ marginLeft: "calc(50% - 120px)", marginRight: "calc(50% - 120px)", textAlign: "center" }}>
        <Typography data-testid="create-workitem-info-price-title" variant="h6" sx={{ textTransform: "uppercase", pb: 0.5 }}>
          {t("content.measurements.info.price")}
        </Typography>
        <Typography data-testid="create-workitem-info-price-amount" variant="h5">
          {formatNumberToPrice(workItem?.workItemTotalPaymentDkr)} {t("common.currency")}
        </Typography>
        <DividerStyled sx={{ borderColor: "secondary.main" }} />
      </Box>
      <CardNoBorderStyled data-testid="material-container" sx={{ backgroundColor: "grey.100" }}>
        <Box sx={{ display: "flex" }}>
          <Box sx={{ flex: 1 }}>
            <Typography variant="overline">{t(`content.measurements.info.${workItem?.workItemType?.toLowerCase()}`)}</Typography>
            <TypographyValueLineStyled data-testid="create-workitem-info-name" variant="body1" color="primary.main">
              {workItem?.workItemText}
            </TypographyValueLineStyled>
            {workItem?.workItemType === "Material" && (
              <Fragment>
                <Typography data-testid="create-workitem-info-ean" variant="overline">
                  {t("content.measurements.info.eanNumber")}
                </Typography>
                <TypographyValueLineStyled variant="body1" color="primary.main">
                  {workItem?.workItemMaterial?.workItemEanNumber}
                </TypographyValueLineStyled>
              </Fragment>
            )}
            {workItem?.workItemType === "Operation" && (
              <Fragment>
                <Typography data-testid="create-workitem-info-number" variant="overline">
                  {t("content.measurements.info.operationNumber")}
                </Typography>
                <TypographyValueLineStyled variant="body1" color="primary.main">
                  {workItem?.workItemOperation?.operationNumber}
                </TypographyValueLineStyled>
              </Fragment>
            )}
          </Box>
          <Box sx={{ display: "flex", flexDirection: "column", pt: "6px", pl: 2, pr: 2, alignItems: "center" }}>
            <Typography variant="overline">{t("content.measurements.info.amount")}</Typography>
            <Typography data-testid="create-workitem-info-amount" variant="body1" color="primary.main" sx={{ pt: 1 }}>
              {workItem?.workItemAmount}
            </Typography>
          </Box>
        </Box>
      </CardNoBorderStyled>
      {workItem?.workItemMaterial?.workItemMountingCode && workItem?.workItemMaterial?.workItemMountingCode > 0 && (
        <CardNoBorderStyled data-testid="mounting-code-container" sx={{ backgroundColor: "grey.100" }}>
          <Typography variant="overline">{t("content.measurements.info.mountingCode")}</Typography>
          <Typography variant="body1" color="primary.main">{`${padNumber(workItem?.workItemMaterial.workItemMountingCode)} - ${
            workItem.workItemMaterial.workItemMountingCodeText
          }`}</Typography>
        </CardNoBorderStyled>
      )}
      {workItem?.workItemMaterial?.supplementOperations && workItem.workItemMaterial?.supplementOperations.length > 0 && (
        <CardNoBorderStyled data-testid="operation-sup-container" sx={{ backgroundColor: costumPalette.operationSupplementBg }}>
          <Typography variant="overline">{t("content.measurements.info.operationSupplement")}</Typography>
          <List sx={{ listStyleType: "disc", pl: 2, pt: 0, pb: 0 }}>
            {workItem?.workItemMaterial?.supplementOperations?.map((so, index) => (
              <ListItem key={index} sx={{ display: "list-item", p: 0, pb: 0.5, color: "primary.main" }}>
                <Typography variant="body1">{`${so.text} (${so.amount})`}</Typography>
              </ListItem>
            ))}
          </List>
        </CardNoBorderStyled>
      )}
      {workItem?.supplements && workItem.supplements.length > 0 && (
        <CardNoBorderStyled data-testid="supplement-container" sx={{ bgcolor: costumPalette.supplementBg }}>
          <Typography variant="overline">{t("content.measurements.info.supplement")}</Typography>
          <List sx={{ listStyleType: "disc", pl: 2, pt: 0, pb: 0 }}>
            {workItem?.supplements?.map((sup, index) => (
              <ListItem key={index} sx={{ display: "list-item", p: 0, pb: 0.5, color: "primary.main" }}>
                <Typography variant="body1">{sup.supplementText}</Typography>
              </ListItem>
            ))}
          </List>
        </CardNoBorderStyled>
      )}
      {/* KTH 2/1-23 hide for better calculation <CardStyled data-testid="operation-times-container" variant="outlined">
        <Box sx={{ display: "flex", width: "100%", justifyContent: "center" }}>
          <Box sx={{ flex: 1, pr: 2, textAlign: "right",  borderRight: "primary.dark", borderRightWidth: 1, borderRightStyle: "solid"}}>
            <Typography variant="overline">{t("content.measurements.info.operationTime")}</Typography>
            <Typography data-testid="create-workitem-info-operationtime"
                        variant="body1" color="primary.main">{getHmsFromMilliSeconds(workItem?.workItemOperationTimeMilliseconds)}</Typography>
          </Box>
          <Box sx={{ flex: 1, pl: 2 }}>
            <Typography variant="overline">{t("content.measurements.info.workTime")}</Typography>
            <Typography data-testid="create-workitem-info-worktime"
                        variant="body1" color="primary.main">{getHmsFromMilliSeconds(calcWorkTime())}</Typography>
          </Box>
        </Box>
      </CardStyled> */}
      <CardStyled data-testid="operation-totaltimes-container" variant="outlined">
        <Box sx={{ display: "flex", width: "100%", justifyContent: "center", alignItems: "center", flexDirection: "column" }}>
          <Typography variant="overline">{t("content.measurements.info.totalWorkTime")}</Typography>
          <Typography data-testid="create-workitem-info-total-worktime" variant="body1" color="primary.main">
            {getHmsFromMilliSeconds(workItem?.workItemTotalOperationTimeMilliseconds)}
          </Typography>
        </Box>
      </CardStyled>
    </Box>
  );
}
