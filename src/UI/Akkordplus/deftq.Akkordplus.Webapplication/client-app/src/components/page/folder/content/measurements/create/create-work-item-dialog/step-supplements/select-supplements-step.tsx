import { useEffect, useState } from "react";
import { Control, Controller, useFieldArray, UseFormGetValues, UseFormSetValue } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import Checkbox from "@mui/material/Checkbox";
import CircularProgress from "@mui/material/CircularProgress";
import FormControlLabel from "@mui/material/FormControlLabel";
import styled from "@mui/system/styled";
import Typography from "@mui/material/Typography";
import { useGetApiCatalogSupplementsQuery } from "api/generatedApi";
import { UseMapWorkItem } from "../../hooks/use-map-work-item";
import { useToast } from "shared/toast/hooks/use-toast";
import { FormDataWorkItem } from "../create-work-item-form-data";

export interface Props {
  setValue: UseFormSetValue<FormDataWorkItem>;
  getValue: UseFormGetValues<FormDataWorkItem>;
  control: Control<FormDataWorkItem>;
}

export function SelectSupplementsStep(props: Props) {
  const { setValue, getValue, control } = props;
  const { t } = useTranslation();
  const toast = useToast();
  const { mapExtendedSupplementsResponses } = UseMapWorkItem();
  const [loading, setLoading] = useState(true);
  const { data: supplementsData, error: supplementsDataError } = useGetApiCatalogSupplementsQuery();

  useEffect(() => {
    const supplements = getValue("supplements");
    if (supplements && supplements.length > 0) {
      setLoading(false);
      return;
    }

    if (supplementsData?.supplements) {
      const mapped = mapExtendedSupplementsResponses(supplementsData.supplements, getValue("preSelectedSupplements"));

      setValue("supplements", mapped);
      setLoading(false);
    }

    if (supplementsDataError) {
      toast.error(t("content.measurements.create.selectSupplementsStep.getSupplementsError"));
    }
  }, [getValue, setValue, supplementsData, supplementsDataError, mapExtendedSupplementsResponses, toast, t]);

  useFieldArray({
    control,
    name: "supplements",
  });

  const FormControlLabelStyled = styled(FormControlLabel)`
    .MuiFormControlLabel-label {
      padding-top: 2px;
    }
  `;

  return (
    <Box sx={{ display: "flex", flexDirection: "column", minHeight: "340px", padding: "20px" }}>
      <Typography data-testid="create-workitem-supplements-title" variant="caption" color={"primary.main"}>
        {t("content.measurements.create.selectSupplementsStep.caption")}
      </Typography>
      <Box sx={{ minHeight: "250px", pt: 1 }}>
        {!loading &&
          // There must be some supplements or else there is an error, the list from BE can never be empty
          getValue("supplements")?.map((incon, index) => {
            return (
              <Controller
                key={index}
                control={control}
                name={`supplements.${index}.checked`}
                render={({ field }) => (
                  <FormControlLabelStyled
                    data-testid={`create-workitem-supplements-${incon.supplementText}-name`}
                    control={<Checkbox checked={field.value} onChange={(event, checked) => field.onChange(checked)} sx={{ pt: 0 }} />}
                    label={incon.supplementText}
                    sx={{ pb: 1, alignItems: "flex-start" }}
                  />
                )}
              />
            );
          })}
        {loading && (
          <Box sx={{ pt: 5, pb: 5, textAlign: "center" }}>
            <CircularProgress size={100} />
          </Box>
        )}
      </Box>
    </Box>
  );
}
