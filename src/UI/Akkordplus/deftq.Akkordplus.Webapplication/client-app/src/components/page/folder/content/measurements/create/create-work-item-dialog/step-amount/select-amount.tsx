import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import Divider from "@mui/material/Divider";
import styled from "@mui/system/styled";
import { UseFormGetValues, UseFormSetValue } from "react-hook-form";
import { useTranslation } from "react-i18next";
import { NumberSelectorInput, SizeEnum } from "components/shared/number-selecter/number-selecter-input";
import { FormDataWorkItem } from "../create-work-item-form-data";

export interface Props {
  setValue: UseFormSetValue<FormDataWorkItem>;
  getValue: UseFormGetValues<FormDataWorkItem>;
}

export function SelectAmountStep(props: Props) {
  const { setValue, getValue } = props;
  const { t } = useTranslation();

  const DividerStyled = styled(Divider)`
    margin-top: 10px;
    margin-bottom: 10px;
  `;

  return (
    <Box sx={{ display: "flex", flexDirection: "column", padding: "20px" }}>
      {getValue("workitemType") === "Operation" ? (
        <Box sx={{ flex: 1 }}>
          <Typography variant="overline">{t("content.measurements.create.selectAmountStep.operation")}</Typography>
          <Typography data-testid="create-workitem-amount-operation" variant="body1" color="primary.main">
            {getValue("operation.operationText")}
          </Typography>
        </Box>
      ) : (
        <Box sx={{ flex: 1 }}>
          <Typography variant="overline">{t("content.measurements.create.selectAmountStep.material")}</Typography>
          <Typography data-testid="create-workitem-amount-material" variant="body1" color="primary.main">
            {getValue("material.name")}
          </Typography>
        </Box>
      )}
      <DividerStyled sx={{ pt: 1 }} />
      <Box sx={{ display: "flex", flexDirection: "column", pt: 1 }}>
        <Typography variant="caption" color={"primary.main"}>
          {t("content.measurements.create.selectAmountStep.header")}
        </Typography>
        <Box sx={{ pt: 5, pb: 6, display: "flex", justifyContent: "center" }}>
          <NumberSelectorInput
            data-testid="create-workitem-mountingcode-amount"
            defaultValue={getValue("amount")}
            onChange={(value) => setValue("amount", value)}
            size={SizeEnum.Large}
          />
        </Box>
      </Box>
    </Box>
  );
}
