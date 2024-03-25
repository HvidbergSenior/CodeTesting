import { Grid } from "@mui/material";
import { ValueCard } from "components/shared/card/value-card";
import { useState, useEffect } from "react";
import { GetProjectFolderSummationQueryResponse } from "api/generatedApi";

interface Props {
  summations?: GetProjectFolderSummationQueryResponse;
}

export const PieceWorkSummationsDesktop = ({ summations }: Props) => {
  const [isLoading, setIsLoading] = useState<boolean>(!summations ?? true);

  useEffect(() => {
    if (summations) {
      setIsLoading(false);
    } else {
      setIsLoading(true);
    }
  }, [summations]);

  return (
    <Grid container item spacing={2} pb={4}>
      <Grid item xs={12} lg={6} xl={6}>
        <ValueCard
          showBackground={true}
          titleNamespace="content.overview.pieceworkSummation.titleMeasurement"
          unitNamespace={"common.currency"}
          asCurrency={true}
          fontSize="large"
          value={summations?.totalPaymentDkr}
          cardSize={"large"}
          isLoading={isLoading}
        />
      </Grid>
      <Grid item xs={12} lg={6} xl={6}>
        <ValueCard
          showBackground={true}
          titleNamespace="content.overview.pieceworkSummation.titleAdditional"
          unitNamespace={"common.currency"}
          asCurrency={true}
          fontSize="large"
          value={summations?.totalExtraPaymentDkr}
          cardSize={"large"}
          isLoading={isLoading}
        />
      </Grid>
    </Grid>
  );
};
