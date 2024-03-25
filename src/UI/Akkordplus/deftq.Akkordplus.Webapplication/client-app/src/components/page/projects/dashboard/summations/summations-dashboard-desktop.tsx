import Grid from "@mui/material/Grid";
import Box from "@mui/material/Box";
import { GetProjectResponse, GetProjectSummationQueryResponse } from "api/generatedApi";
import { ValueCard } from "components/shared/card/value-card";
import { useEffect, useState } from "react";
import { useEditAgreedLumpsum } from "../edit-lumpsum/hooks/use-edit-lumpsum";
import { FontSize } from "components/shared/card/use-value-card-formatter";

interface Props {
  project: GetProjectResponse;
  summaryData: GetProjectSummationQueryResponse;
  usePrintlayOut: boolean;
}

const SummationsDashboardDesktop = ({ project, summaryData, usePrintlayOut }: Props) => {
  const [cardGridSize, setCardGridSize] = useState<number>(4);
  const editLumpsum = useEditAgreedLumpsum({ project: project, lumpsum: summaryData.totalLumpSumDkr ?? 0 });
  const spacing = 2;

  const getNumberOfDigits = (amount: number): number => {
    return amount.toFixed(2).length;
  };

  const fontSizeAdjusted = (value: number): FontSize => (getNumberOfDigits(value) > 12 || getNumberOfDigits(value) > 12 ? "small" : "large");
  const fontSizeAdjustedAdditionalCard = (value: number): FontSize => (getNumberOfDigits(value) > 11 ? "xsmall" : "small");

  useEffect(() => {
    switch (project.pieceworkType) {
      case "TwelveOneA" || "TwelveOneC":
        setCardGridSize(3);
        break;
      case "TwelveTwo":
        setCardGridSize(3);
        break;
      default:
        setCardGridSize(4);
    }
  }, [project.pieceworkType]);

  const getCard = () => {
    switch (project.pieceworkType) {
      case "TwelveOneA" || "TwelveOneC":
        return calculationCard();
      case "TwelveTwo":
        return PieceworkTotal();
      default:
        return <Box></Box>;
    }
  };

  const calculationCard = () => {
    return (
      <Grid item xs={usePrintlayOut ? cardGridSize : 12} lg={cardGridSize} xl={cardGridSize}>
        <ValueCard
          titleNamespace={"dashboard.widgetTitle.calculation"}
          value={summaryData.totalCalculationSumDkr}
          fontSize={fontSizeAdjustedAdditionalCard(summaryData.totalCalculationSumDkr ?? 0)}
          unitNamespace={"common.currency"}
          asCurrency={true}
          showBackground={false}
        />
      </Grid>
    );
  };

  const PieceworkTotal = () => {
    return (
      <Grid item xs={usePrintlayOut ? cardGridSize : 12} lg={cardGridSize} xl={cardGridSize}>
        <ValueCard
          titleNamespace={"dashboard.widgetTitle.pieceworkTotal"}
          value={summaryData.totalPaymentDkr}
          fontSize={fontSizeAdjustedAdditionalCard(summaryData.totalPaymentDkr ?? 0)}
          unitNamespace={"common.currency"}
          asCurrency={true}
          showBackground={false}
          headerActionClickedProps={usePrintlayOut ? undefined : editLumpsum}
        />
      </Grid>
    );
  };

  return (
    <Grid item xs={12}>
      <Grid container item xs={12} spacing={spacing} pb={2}>
        <Grid item xs={usePrintlayOut ? 6 : 12} lg={6} xl={6}>
          <ValueCard
            titleNamespace={"dashboard.widgetTitle.pieceworkTotalPrice"}
            value={summaryData.totalPaymentDkr}
            unitNamespace={"common.currency"}
            asCurrency={true}
            showBackground={true}
            fontSize={fontSizeAdjusted(summaryData.totalPaymentDkr ?? 0)}
          />
        </Grid>
        <Grid item xs={usePrintlayOut ? 6 : 12} lg={6} xl={6}>
          <ValueCard
            titleNamespace={"dashboard.widgetTitle.logbookHours"}
            value={summaryData.totalLogBookHours}
            unitNamespace={"common.time.hours"}
            showBackground={true}
            fontSize={fontSizeAdjusted(summaryData.totalLogBookHours ?? 0)}
          />
        </Grid>
      </Grid>
      <Grid container item xs={12} spacing={spacing}>
        {getCard()}
        <Grid item xs={usePrintlayOut ? cardGridSize : 12} lg={cardGridSize} xl={cardGridSize}>
          <ValueCard
            titleNamespace={"dashboard.widgetTitle.workitemsTotalPrice"}
            value={summaryData.totalWorkItemPaymentDkr}
            fontSize={fontSizeAdjustedAdditionalCard(summaryData.totalWorkItemPaymentDkr ?? 0)}
            unitNamespace={"common.currency"}
            asCurrency={true}
            showBackground={false}
          />
        </Grid>
        <Grid item xs={usePrintlayOut ? cardGridSize : 12} lg={cardGridSize} xl={cardGridSize}>
          <ValueCard
            titleNamespace={"dashboard.widgetTitle.workitemsExtraTotalPrice"}
            value={summaryData.totalWorkItemExtraWorkPaymentDkr}
            fontSize={fontSizeAdjustedAdditionalCard(summaryData.totalWorkItemExtraWorkPaymentDkr ?? 0)}
            unitNamespace={"common.currency"}
            asCurrency={true}
            showBackground={false}
          />
        </Grid>
        <Grid item xs={usePrintlayOut ? cardGridSize : 12} lg={cardGridSize} xl={cardGridSize}>
          <ValueCard
            titleNamespace={"dashboard.widgetTitle.workAgreements"}
            value={summaryData.totalExtraWorkAgreementDkr}
            fontSize={fontSizeAdjustedAdditionalCard(summaryData.totalExtraWorkAgreementDkr ?? 0)}
            unitNamespace={"common.currency"}
            asCurrency={true}
            showBackground={false}
          />
        </Grid>
      </Grid>
    </Grid>
  );
};

export default SummationsDashboardDesktop;
