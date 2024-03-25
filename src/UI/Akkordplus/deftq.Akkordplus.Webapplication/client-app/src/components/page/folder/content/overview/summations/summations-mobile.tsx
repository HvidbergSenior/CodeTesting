import { ValueCard } from "components/shared/card/value-card";
import { useEffect, useState } from "react";
import Box from "@mui/material/Box";
import { GetProjectFolderSummationQueryResponse } from "api/generatedApi";
import { FontSize } from "components/shared/card/use-value-card-formatter";

interface Props {
  summations?: GetProjectFolderSummationQueryResponse;
}

export const PieceWorkSummationsMobile = ({ summations }: Props) => {
  const [isLoading, setIsLoading] = useState<boolean>(!summations ?? false);

  const getNumberOfDigits = (amount: number): number => {
    return amount.toFixed(2).length;
  };

  const fontSizeAdjusted: FontSize =
    getNumberOfDigits(summations?.totalPaymentDkr ?? 0) > 12 || getNumberOfDigits(summations?.totalExtraPaymentDkr ?? 0) > 12 ? "xsmall" : "small";

  useEffect(() => {
    if (summations) {
      setIsLoading(false);
    } else {
      setIsLoading(true);
    }
  }, [summations]);

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
          showBackground={true}
          titleNamespace="content.overview.pieceworkSummation.titleMeasurement"
          unitNamespace={"common.currency"}
          asCurrency={true}
          fontSize={fontSizeAdjusted}
          value={summations?.totalPaymentDkr}
          cardSize={"medium"}
          isLoading={isLoading}
          fixedWidth={"300px"}
        />
      </Box>
      <Box>
        <ValueCard
          showBackground={true}
          titleNamespace="content.overview.pieceworkSummation.titleAdditional"
          unitNamespace={"common.currency"}
          asCurrency={true}
          fontSize={fontSizeAdjusted}
          value={summations?.totalExtraPaymentDkr}
          cardSize={"medium"}
          isLoading={isLoading}
          fixedWidth={"300px"}
        />
      </Box>
    </Box>
  );
};
