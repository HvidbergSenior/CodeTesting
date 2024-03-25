import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import Divider from "@mui/material/Divider";
import InputBase from "@mui/material/InputBase";
import Typography from "@mui/material/Typography";
import { BaseRateAndSupplementsValueStatus, BaseRateUpdate, ProjectResponse } from "api/generatedApi";
import { useAbortDialog } from "components/shared/alert/abort/hook/use-abort";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import { useInputValidation } from "components/shared/validation/input-validation";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import type { DialogBaseProps } from "shared/dialog/types";
import { SubmitState } from "shared/enums";
import { formatNumberToAmount } from "utils/formats";
import { EditSection } from "../../components/section/edit-section";
import { SectionProps } from "../../base-rate/edit/edit-base";

interface Props extends DialogBaseProps {
  onSubmit: (state: SubmitState, baseRateUpdate: BaseRateUpdate) => void;
  folder: ExtendedProjectFolder;
  project: ProjectResponse;
}

export const EditPaymentFactor = (props: Props) => {
  const { onSubmit, onClose, folder } = props;
  const { t } = useTranslation();
  const { validateEndsWithCommaOrStartsWithComma, validateMultipleInputPercentWithAndStatement, validateDoesNotIncludeEmptyString } =
    useInputValidation();
  const percentValue = 100;
  const nonFunction = () => {};
  const openAbortDialog = useAbortDialog({ closeDialog: onClose ?? nonFunction });
  const userDefinedRegulationValue = formatNumberToAmount(folder?.baseRateAndSupplements?.baseRateRegulationPercentage?.value);
  const [userDefinedRegulation, setUserDefinedRegulation] = useState<string>(userDefinedRegulationValue);
  const [overwriteUserDefinedRegulation, setOverwriteUserDefinedRegulation] = useState<BaseRateAndSupplementsValueStatus>(
    folder?.baseRateAndSupplements?.baseRateRegulationPercentage?.valueStatus ?? "Inherit"
  );
  const isInRootFolder = folder?.isRoot;

  const closeDialog = () => {
    if (!hasDirt()) {
      if (onClose) {
        onClose();
        return;
      }
    }

    openAbortDialog();
  };

  const hasDirt = (): boolean => {
    const hasSameOverwriteStatus =
      overwriteUserDefinedRegulation === folder?.baseRateAndSupplements?.baseRateRegulationPercentage?.valueStatus ?? "Inherit";
    const hasSameValues = userDefinedRegulation === userDefinedRegulationValue;

    return !(hasSameOverwriteStatus && hasSameValues);
  };

  const getToggleValueFromStatus = (value: BaseRateAndSupplementsValueStatus): boolean => {
    if (value === "Inherit") {
      return false;
    }
    return true;
  };

  const handleConfirmUserDefinedRegulation = () =>
    overwriteUserDefinedRegulation === "Inherit" ? setOverwriteUserDefinedRegulation("Overwrite") : handleChangeToInherit();

  const handleChangeToInherit = () => {
    setOverwriteUserDefinedRegulation("Inherit");
    setUserDefinedRegulation(formatNumberToAmount(folder?.parent?.baseRateAndSupplements?.baseRateRegulationPercentage?.value));
  };

  const editSections: SectionProps[] = [
    {
      inputFieldProps: {
        value: userDefinedRegulation,
        disableInputField: !getToggleValueFromStatus(overwriteUserDefinedRegulation),
        inputType: "percent",
        maxValue: percentValue,
        dispatchChange: setUserDefinedRegulation,
      },

      switchProps: {
        value: getToggleValueFromStatus(overwriteUserDefinedRegulation),
        title: t("content.calculation.baseRateAndSupplements.edit.overwrite"),
        confirmationDialogProps: {
          titleOn: isInRootFolder
            ? t("content.calculation.baseRateAndSupplements.edit.confirmTitleNoActionPossible")
            : t("content.calculation.baseRateAndSupplements.edit.confirmTitleOnUserRegulation"),
          titleOff: t("content.calculation.baseRateAndSupplements.edit.confirmTitleOffUserRegulation"),
          description: isInRootFolder
            ? t("content.calculation.baseRateAndSupplements.edit.confirmDescriptionNoActionPossibleUserDefined")
            : t("content.calculation.baseRateAndSupplements.edit.confirmDescriptionUserRegulation"),
          handleConfirm: handleConfirmUserDefinedRegulation,
        },
      },
      title: t("content.calculation.paymentFactor.userDefinedRegulation"),
      descriptions: [],
      hasDividerBelow: true,
    },
  ];

  const prepareSubmit = (submitState: SubmitState) => {
    const userDefinedRegulationValue = parseFloat(userDefinedRegulation.replace(/,/g, "."));
    let userDefinedRegulationTemp: BaseRateUpdate = { status: overwriteUserDefinedRegulation, value: userDefinedRegulationValue };

    onSubmit(submitState, userDefinedRegulationTemp);
  };

  return (
    <Dialog maxWidth="xs" open={props.isOpen}>
      <DialogTitleStyled
        title={t("content.calculation.paymentFactor.edit.title")}
        description={t("content.calculation.paymentFactor.edit.description")}
        onClose={props.onClose}
        isOpen={props.isOpen}
        handleIconClose={closeDialog}
        textPaddingHorizontal={5}
      />
      <DialogContent sx={{ p: 0, pb: 5 }}>
        <Box display={"flex"} flexDirection={"row"} sx={{ pt: 3 }} pl={5}>
          <Box display={"flex"} width={"120px"} flexDirection={"column"}>
            <Typography pb={1} variant="caption" color={"primary.main"}>
              {t("content.calculation.paymentFactor.edit.paymentFactor")}
            </Typography>
            <Box
              sx={{
                justifyContent: "center",
                p: "10px 20px",
                pl: "0px",
                display: "flex",
                alignItems: "center",
                width: 120,
                backgroundColor: "#FFFFFF",
              }}
            >
              <InputBase
                sx={{ pointerEvents: "none", pt: 0.5, "& .MuiInputBase-input": { textAlign: "center", width: "50px" } }}
                value={formatNumberToAmount(folder?.baseRateAndSupplements?.baseRatePerMinDkr, 3)}
              />
            </Box>
          </Box>

          <Box pt={5.2} marginLeft={"-20px"} width={"200px"} justifyContent={"center"} height={"60px"}>
            <Typography pt={"1px"} sx={{ color: "primary.main" }} variant="body2">
              {t("content.calculation.paymentFactor.edit.paymentPrMinute")}
            </Typography>
          </Box>
        </Box>

        <Divider variant="fullWidth" sx={{ color: "primary.dark" }}></Divider>

        {editSections && editSections.map((section) => <EditSection key={section.title} section={section} isInRootFolder={isInRootFolder} />)}
      </DialogContent>
      <DialogActions sx={{ pr: 3 }}>
        <Button
          disabled={
            !validateDoesNotIncludeEmptyString([userDefinedRegulation]) ||
            (!validateMultipleInputPercentWithAndStatement([userDefinedRegulation]) && !validateEndsWithCommaOrStartsWithComma(userDefinedRegulation))
          }
          variant="contained"
          sx={{ width: "160px" }}
          color="primary"
          onClick={() => prepareSubmit(SubmitState.Save)}
        >
          {t("common.saveAndClose")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
