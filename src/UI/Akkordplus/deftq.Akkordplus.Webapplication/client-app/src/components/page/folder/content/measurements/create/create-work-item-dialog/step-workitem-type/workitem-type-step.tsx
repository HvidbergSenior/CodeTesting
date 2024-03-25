import { useState } from "react";
import { UseFormGetValues, UseFormSetValue } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import Tab from "@mui/material/Tab";
import Tabs from "@mui/material/Tabs";
import Divider from "@mui/material/Divider";
import SensorsOffIcon from "@mui/icons-material/SensorsOff";
import { SelectMaterialStep } from "../step-material/select-material-step";
import { SelectOperationStep } from "../step-operation/select-operation-step";
import { SelectFavoritStep } from "../step-favorites/select-favorit-step";
import { useOnlineStatus } from "utils/connectivity/hook/use-online-status";
import { FormDataWorkItem } from "../create-work-item-form-data";

export interface Props {
  setValue: UseFormSetValue<FormDataWorkItem>;
  getValue: UseFormGetValues<FormDataWorkItem>;
  validationError?: string;
  clearValidateError: () => void;
}

export function WorkitemTypeStep(props: Props) {
  const { setValue, getValue, validationError, clearValidateError } = props;
  const { t } = useTranslation();
  const isOnline = useOnlineStatus();

  const materialTab = 0;
  const favoritesTab = 1;
  const operationsTab = 2;

  const [selectedTab, setSelectedTab] = useState<number>(() => {
    if (!isOnline || getValue("favorites")?.length > 0) {
      return favoritesTab;
    }
    return getValue("workitemType") === "Material" ? materialTab : operationsTab;
  });

  const handleChange = (event: React.SyntheticEvent, tabId: number) => {
    if (!isOnline) {
      return;
    }
    setSelectedTab(tabId);
    clearValidateError();
    if (tabId === materialTab || tabId === operationsTab) {
      setValue("workitemType", tabId === materialTab ? "Material" : "Operation");
    }
  };

  return (
    <Box sx={{ display: "flex", flexDirection: "column" }}>
      <Tabs
        value={selectedTab}
        onChange={handleChange}
        indicatorColor="secondary"
        TabIndicatorProps={{ sx: { height: 5 } }}
        textColor="primary"
        variant="fullWidth"
        sx={{
          backgroundColor: "background",
          width: "100%",
        }}
      >
        <Tab
          value={materialTab}
          label={isOnline ? t("content.measurements.create.workitemTypeStep.tabs.material") : <SensorsOffIcon />}
          sx={{ height: 40 }}
        />
        {(!isOnline || getValue("favorites")?.length > 0) && (
          <Tab value={favoritesTab} label={t("content.measurements.create.workitemTypeStep.tabs.favorites")} sx={{ height: 40 }} />
        )}
        <Tab
          value={operationsTab}
          label={isOnline ? t("content.measurements.create.workitemTypeStep.tabs.operations") : <SensorsOffIcon />}
          sx={{ height: 40 }}
        />
      </Tabs>
      <Divider sx={{ height: "50px", boxShadow: "0 1em 1em -1em rgba(0, 0, 0, .25)", mt: "-50px", mb: "20px" }} />
      <Box sx={{ padding: "0 10px 10px 10px" }}>
        {selectedTab === materialTab && <SelectMaterialStep setValue={setValue} getValue={getValue} validateError={validationError} />}
        {selectedTab === favoritesTab && <SelectFavoritStep setValue={setValue} getValue={getValue} validateError={validationError} />}
        {selectedTab === operationsTab && <SelectOperationStep setValue={setValue} getValue={getValue} validateError={validationError} />}
      </Box>
    </Box>
  );
}
