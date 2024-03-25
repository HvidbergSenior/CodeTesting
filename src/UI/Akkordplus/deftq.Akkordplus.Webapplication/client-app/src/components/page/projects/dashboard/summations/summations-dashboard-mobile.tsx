import { ValueCard } from "components/shared/card/value-card";
import Box from "@mui/material/Box";
import { GetProjectResponse, GetProjectSummationQueryResponse } from "api/generatedApi";
import { useEditAgreedLumpsum } from "../edit-lumpsum/hooks/use-edit-lumpsum";
import { FontSize } from "components/shared/card/use-value-card-formatter";

export type Summations = {
  sum: number;
  extraWork: number;
};

interface Props {
  project: GetProjectResponse;
  summaryData: GetProjectSummationQueryResponse;
}

export const SummationsDashboardMobile = ({ project, summaryData }: Props) => {
  const editLumpsum = useEditAgreedLumpsum({ project, lumpsum: summaryData.totalLumpSumDkr ?? 0 });
  const fixedWidth = "300px";
  const cardSize = "medium";

  const getNumberOfDigits = (amount: number): number => {
    return amount.toFixed(2).length;
  };

  const fontSizeAdjusted = (value: number): FontSize => (getNumberOfDigits(value) > 12 || getNumberOfDigits(value) > 12 ? "xsmall" : "small");

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
      <ValueCard
        titleNamespace={"dashboard.widgetTitle.calculation"}
        value={summaryData.totalCalculationSumDkr}
        fontSize={fontSizeAdjusted(summaryData.totalCalculationSumDkr ?? 0)}
        unitNamespace={"common.currency"}
        asCurrency={true}
        showBackground={false}
        fixedWidth={fixedWidth}
        cardSize={cardSize}
      />
    );
  };

  const PieceworkTotal = () => {
    return (
      <ValueCard
        titleNamespace={"dashboard.widgetTitle.pieceworkTotal"}
        value={summaryData.totalPaymentDkr}
        fontSize={fontSizeAdjusted(summaryData.totalPaymentDkr ?? 0)}
        unitNamespace={"common.currency"}
        asCurrency={true}
        showBackground={false}
        headerActionClickedProps={editLumpsum}
        fixedWidth={fixedWidth}
        cardSize={cardSize}
      />
    );
  };

  return (
    <Box
      pt={2}
      sx={{
        overflowX: "auto",
        display: "flex",
        width: "calc(100vw - 30px)",
      }}
    >
      <Box pr={2}>
        <ValueCard
          titleNamespace={"dashboard.widgetTitle.pieceworkTotalPrice"}
          value={summaryData.totalPaymentDkr}
          unitNamespace={"common.currency"}
          asCurrency={true}
          showBackground={true}
          fixedWidth={fixedWidth}
          cardSize={cardSize}
          fontSize={fontSizeAdjusted(summaryData.totalPaymentDkr ?? 0)}
        />
      </Box>
      <Box pr={2}>
        <ValueCard
          titleNamespace={"dashboard.widgetTitle.logbookHours"}
          value={summaryData.totalLogBookHours}
          unitNamespace={"common.time.hours"}
          showBackground={true}
          fixedWidth={fixedWidth}
          cardSize={cardSize}
          fontSize={fontSizeAdjusted(summaryData.totalLogBookHours ?? 0)}
        />
      </Box>
      <Box>{getCard()}</Box>

      <Box pr={2}>
        <ValueCard
          titleNamespace={"dashboard.widgetTitle.workitemsTotalPrice"}
          value={summaryData.totalWorkItemPaymentDkr}
          fontSize={fontSizeAdjusted(summaryData.totalWorkItemPaymentDkr ?? 0)}
          unitNamespace={"common.currency"}
          asCurrency={true}
          showBackground={false}
          fixedWidth={fixedWidth}
          cardSize={cardSize}
        />
      </Box>
      <Box pr={2}>
        <ValueCard
          titleNamespace={"dashboard.widgetTitle.workitemsExtraTotalPrice"}
          value={summaryData.totalWorkItemExtraWorkPaymentDkr}
          fontSize={fontSizeAdjusted(summaryData.totalWorkItemExtraWorkPaymentDkr ?? 0)}
          unitNamespace={"common.currency"}
          asCurrency={true}
          showBackground={false}
          fixedWidth={fixedWidth}
          cardSize={cardSize}
        />
      </Box>
      <Box pr={2}>
        <ValueCard
          titleNamespace={"dashboard.widgetTitle.workAgreements"}
          value={summaryData.totalExtraWorkAgreementDkr}
          fontSize={fontSizeAdjusted(summaryData.totalExtraWorkAgreementDkr ?? 0)}
          unitNamespace={"common.currency"}
          asCurrency={true}
          showBackground={false}
          cardSize={cardSize}
          fixedWidth={fixedWidth}
        />
      </Box>
    </Box>
  );
};
