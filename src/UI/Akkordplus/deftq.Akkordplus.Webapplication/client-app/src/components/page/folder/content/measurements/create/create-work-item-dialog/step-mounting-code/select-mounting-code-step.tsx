import { ChangeEvent, useState, useEffect, useRef } from "react";
import { UseFormSetValue, UseFormGetValues } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import Divider from "@mui/material/Divider";
import FormControl from "@mui/material/FormControl";
import RadioGroup from "@mui/material/RadioGroup";
import FormControlLabel from "@mui/material/FormControlLabel";
import Radio from "@mui/material/Radio";
import CircularProgress from "@mui/material/CircularProgress";
import styled from "@mui/system/styled";
import { MaterialMountingResponse } from "api/generatedApi";
import { api } from "api/enhancedEndpoints";
import { useToast } from "shared/toast/hooks/use-toast";
import { padNumber } from "utils/formats";
import { useOnlineStatus } from "utils/connectivity/hook/use-online-status";
import { useOfflineStorage } from "utils/offline-storage/use-offline-storage";
import { FormDataWorkItem } from "../create-work-item-form-data";

export interface Props {
  setValue: UseFormSetValue<FormDataWorkItem>;
  getValue: UseFormGetValues<FormDataWorkItem>;
}

export function SelectMountingCodeStep(props: Props) {
  const { setValue, getValue } = props;
  const noMountingCodeSelected: MaterialMountingResponse = { mountingCode: -1, operationTimeMilliseconds: 0 };
  const [selectedMountingCode, setSelectedMountingCode] = useState<MaterialMountingResponse | undefined>(
    getValue("mountingCode") ?? noMountingCodeSelected
  );
  const { t } = useTranslation();
  const tRef = useRef(t);
  const toast = useToast();
  const toastRef = useRef(toast);
  const [loading, setLoading] = useState(true);
  const [materialId] = useState<string>(getValue("material.id") ?? "");
  const [queryByMaterialId, { data: materialMetaData, error: materialMetaDataError }] =
    api.endpoints.getApiCatalogMaterialsByMaterialId.useLazyQuery();
  const isOnline = useOnlineStatus();
  const { getOfflineProjectId, getOfflineMaterialMountings } = useOfflineStorage();
  const [mountings, setMountings] = useState<MaterialMountingResponse[] | undefined>();

  useEffect(() => {
    if (isOnline && materialId && materialId !== "") {
      queryByMaterialId({ materialId });
    }
  }, [materialId, queryByMaterialId, isOnline]);

  useEffect(() => {
    if (!isOnline && !mountings) {
      const list = getOfflineMaterialMountings(getOfflineProjectId(), materialId);
      setMountings(list);
      setLoading(false);
    }
  }, [materialId, isOnline, getOfflineMaterialMountings, getOfflineProjectId, mountings]);

  useEffect(() => {
    if (materialMetaData || materialMetaDataError) {
      setMountings(materialMetaData?.mountings ?? []);
      setLoading(false);
    }

    if (materialMetaDataError) {
      toastRef.current.error(tRef.current("content.measurements.create.selectMountingCodesStep.getMountingCodesError"));
    }
  }, [materialMetaData, materialMetaDataError, toastRef, tRef]);

  const DividerStyled = styled(Divider)`
    margin-top: 10px;
    margin-bottom: 10px;
  `;

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    const selectedId = parseInt((event.target as HTMLInputElement).value);
    if (selectedId === -1) {
      setSelectedMountingCode(noMountingCodeSelected);
      setValue("mountingCode", undefined);
      return;
    }
    var found = mountings?.find((a) => a.mountingCode === selectedId);
    if (!found) {
      found = noMountingCodeSelected;
    }
    setSelectedMountingCode(found);
    setValue("mountingCode", found);
  };

  return (
    <Box sx={{ display: "flex", flexDirection: "column", padding: "20px" }}>
      <Box sx={{ display: "flex" }}>
        <Box sx={{ flex: 1 }}>
          <Typography variant="overline">{t("content.measurements.create.selectMountingCodesStep.material")}</Typography>
          <Typography data-testid="create-workitem-mountingcode-material" variant="body1" color="primary.main">
            {getValue("material.name")}
          </Typography>
        </Box>
        <Box sx={{ pl: 2, pr: 3 }}>
          <Typography variant="overline">{t("content.measurements.create.common.amount")}</Typography>
          <Typography data-testid="create-workitem-mountingcode-amount" variant="body1" color="primary.main" textAlign={"center"}>
            {getValue("amount")}
          </Typography>
        </Box>
      </Box>
      <DividerStyled sx={{ pt: 1 }} />
      <Typography variant="caption" color={"primary.main"} sx={{ pt: 1 }}>
        {t("content.measurements.create.selectMountingCodesStep.header")}
      </Typography>
      <Box sx={{ minHeight: "250px" }}>
        {!loading && mountings && (
          <FormControl>
            <RadioGroup value={selectedMountingCode?.mountingCode} onChange={handleChange}>
              <FormControlLabel
                data-testid="create-workitem-mountingcode-codes-non-selected"
                value={"-1"}
                control={<Radio />}
                label={t("content.measurements.create.selectMountingCodesStep.noMountingCode")}
              />
              {mountings.map((code, index) => {
                return (
                  <FormControlLabel
                    data-testid={`create-workitem-mountingcode-codes-${padNumber(code.mountingCode)} - ${code.text}`}
                    key={index}
                    value={code.mountingCode}
                    control={<Radio />}
                    label={`${padNumber(code.mountingCode)} - ${code.text}`}
                  />
                );
              })}
            </RadioGroup>
          </FormControl>
        )}
        {loading && (
          <Box sx={{ pt: 5, pb: 5, textAlign: "center" }}>
            <CircularProgress size={100} />
          </Box>
        )}
      </Box>
    </Box>
  );
}
