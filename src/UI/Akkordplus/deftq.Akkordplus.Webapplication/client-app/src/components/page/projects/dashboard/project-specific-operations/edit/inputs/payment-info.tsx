import { useTranslation } from "react-i18next";
import Typography from "@mui/material/Typography";
import { GetProjectSpecificOperationResponse } from "api/generatedApi";
import { InputContainer } from "shared/styles/input-container-style";
import { formatNumberToPrice } from "utils/formats";

interface Props {
  operation: GetProjectSpecificOperationResponse;
}

export const ProjectSpecicifOperationPaymentInfo = (props: Props) => {
  const { operation } = props;
  const { t } = useTranslation();

  return (
    <InputContainer sx={{ height: 1, maxWidth: 400 }}>
      <Typography variant="caption" color={"primary.main"}>
        {t("common.currency")}
      </Typography>
      <Typography variant="body1">{formatNumberToPrice(operation.payment)}</Typography>
    </InputContainer>
  );
};
