import { Control, Controller, useFieldArray, UseFormGetValues, UseFormSetValue } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { NumberSelectorInput, SizeEnum } from "components/shared/number-selecter/number-selecter-input";
import { FormDataWorkItem } from "../create-work-item-form-data";

export interface Props {
  setValue: UseFormSetValue<FormDataWorkItem>;
  getValue: UseFormGetValues<FormDataWorkItem>;
  control: Control<FormDataWorkItem>;
}

export function SelectSupplementOperationsStep(props: Props) {
  const { getValue, control } = props;
  useFieldArray({
    control,
    name: "supplementOperations",
  });
  const { t } = useTranslation();
  const supplementOperations = getValue("supplementOperations");

  return (
    <Box sx={{ display: "flex", flexDirection: "column", minHeight: "340px", padding: "20px" }}>
      <Typography data-testid="create-workitem-supplement-operations-title" variant="caption" color={"primary.main"}>
        {t("content.measurements.create.selectSupplementOperationStep.caption")}
      </Typography>
      {supplementOperations &&
        supplementOperations?.length > 0 &&
        supplementOperations.map((oprs, index) => {
          return (
            <Controller
              data-testid={`create-workitem-supplement-operations-${oprs.text}-name`}
              key={index}
              control={control}
              name={`supplementOperations.${index}.amount`}
              render={({ field }) => (
                <Box sx={{ display: "flex", pt: 1 }}>
                  <Box sx={{ flex: 1, pt: 0.6 }}>
                    <Typography>{oprs.text}</Typography>
                  </Box>
                  <Box sx={{ pl: 1, width: "110px" }}>
                    <NumberSelectorInput
                      data-testid={`create-workitem-supplement-operations-${oprs.text}-amount`}
                      onChange={(value) => field.onChange(value)}
                      defaultValue={field.value}
                      size={SizeEnum.Normal}
                    />
                  </Box>
                </Box>
              )}
            />
          );
        })}
    </Box>
  );
}
