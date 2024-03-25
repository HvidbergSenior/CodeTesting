import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import Divider from "@mui/material/Divider";
import { BaseRateAndSupplementsValueResponse, BaseRateAndSupplementsValueStatus, BaseSupplementUpdate, GetProjectResponse } from "api/generatedApi";
import { useAbortDialog } from "components/shared/alert/abort/hook/use-abort";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import { InputFieldProps } from "components/shared/number-input/input-common";
import { useInputValidation } from "components/shared/validation/input-validation";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import type { DialogBaseProps } from "shared/dialog/types";
import { SubmitState } from "shared/enums";
import { formatNumberToAmount } from "utils/formats";
import { EditSection } from "../../components/section/edit-section";

interface Props extends DialogBaseProps {
  onSubmit: (state: SubmitState, indirectTime: BaseRateAndSupplementsValueResponse, siteSpecificTime: BaseRateAndSupplementsValueResponse) => void;
  folder: ExtendedProjectFolder | undefined;
  project: GetProjectResponse;
}

export type SectionProps = {
  inputFieldProps: InputFieldProps;
  switchProps?: SwitchProps;
  hasDividerBelow?: boolean;
  descriptions: string[];
  title: string;
};

export type SwitchProps = {
  title: string;
  value: boolean;
  confirmationDialogProps?: ConfirmationDialogProps;
};

export type ConfirmationDialogProps = {
  titleOff: string;
  titleOn: string;
  description: string;
  handleConfirm: () => void;
};

export const EditBaseRateAndSupplements = (props: Props) => {
  const { onSubmit, onClose, folder } = props;
  const { t } = useTranslation();
  const { validateMultipleInputPercentWithAndStatement, validateEndsWithCommaOrStartsWithComma, validateDoesNotIncludeEmptyString } =
    useInputValidation();
  const indirectTimeValue = formatNumberToAmount(folder?.baseRateAndSupplements?.indirectTimeSupplementPercentage?.value);
  const siteSpecificTimeValue = formatNumberToAmount(folder?.baseRateAndSupplements?.siteSpecificTimeSupplementPercentage?.value);
  const [indirectTime, setIndirectTime] = useState<string>(indirectTimeValue);
  const [siteSpecificTime, setSiteSpecificTime] = useState<string>(siteSpecificTimeValue);
  const [overwriteIndirectTime, setOverwriteIndirectTime] = useState<BaseRateAndSupplementsValueStatus>(
    folder?.baseRateAndSupplements?.indirectTimeSupplementPercentage?.valueStatus ?? "Inherit"
  );
  const [overwriteSiteSpecificTime, setOverwriteSiteSpecificTime] = useState<BaseRateAndSupplementsValueStatus>(
    folder?.baseRateAndSupplements?.siteSpecificTimeSupplementPercentage?.valueStatus ?? "Inherit"
  );

  const personalTime = folder?.baseRateAndSupplements?.personalTimeSupplementPercentage?.toString() ?? "0";
  const percentValue = 100;
  const nonFunction = () => {};
  const isInRootFolder = folder?.isRoot;
  const openAbortDialog = useAbortDialog({ closeDialog: onClose ?? nonFunction });

  const areInputsValid = () => {
    return (
      !validateDoesNotIncludeEmptyString([indirectTime, siteSpecificTime]) ||
      !(
        validateMultipleInputPercentWithAndStatement([indirectTime, siteSpecificTime]) &&
        validateEndsWithCommaOrStartsWithComma(indirectTime) &&
        validateEndsWithCommaOrStartsWithComma(siteSpecificTime)
      )
    );
  };
  const closeDialog = () => {
    if (!hasDirt() && onClose) {
      onClose();
      return;
    }
    openAbortDialog();
  };

  const hasDirt = (): boolean => {
    const hasSameOverwriteStatus =
      overwriteIndirectTime === folder?.baseRateAndSupplements?.indirectTimeSupplementPercentage?.valueStatus &&
      overwriteSiteSpecificTime === folder?.baseRateAndSupplements?.siteSpecificTimeSupplementPercentage?.valueStatus;
    const hasSameValues = indirectTime === indirectTimeValue && siteSpecificTime === siteSpecificTimeValue;

    return !(hasSameOverwriteStatus && hasSameValues);
  };

  const getValueFromStatus = (value: BaseRateAndSupplementsValueStatus): boolean => {
    if (value === "Inherit") {
      return false;
    }
    return true;
  };

  const handleConfirmIndirectTime = () =>
    overwriteIndirectTime === "Inherit" ? setOverwriteIndirectTime("Overwrite") : handleChangeToInherit("indirect");
  const handleConfirmSiteSpecificTime = () =>
    overwriteSiteSpecificTime === "Inherit" ? setOverwriteSiteSpecificTime("Overwrite") : handleChangeToInherit("siteSpecific");

  const handleChangeToInherit = (supplementType: string) => {
    if (supplementType === "indirect") {
      setOverwriteIndirectTime("Inherit");
      setIndirectTime(formatNumberToAmount(folder?.parent?.baseRateAndSupplements?.indirectTimeSupplementPercentage?.value));
    }
    if (supplementType === "siteSpecific") {
      setOverwriteSiteSpecificTime("Inherit");
      setSiteSpecificTime(formatNumberToAmount(folder?.parent?.baseRateAndSupplements?.siteSpecificTimeSupplementPercentage?.value));
    }
  };

  const prepareSubmit = (submitState: SubmitState) => {
    const indirectTimeValue = parseFloat(indirectTime.replace(/,/g, "."));
    const siteSpecificTimeValue = parseFloat(siteSpecificTime.replace(/,/g, "."));

    let indirectTimeTemp: BaseSupplementUpdate = { status: overwriteIndirectTime, value: indirectTimeValue };
    let siteSpecificTimeTemp: BaseSupplementUpdate = { status: overwriteSiteSpecificTime, value: siteSpecificTimeValue };

    onSubmit(submitState, indirectTimeTemp, siteSpecificTimeTemp);
  };

  const editSections: SectionProps[] = [
    {
      inputFieldProps: {
        value: indirectTime,
        disableInputField: !getValueFromStatus(overwriteIndirectTime),
        inputType: "percent",
        maxValue: percentValue,
        dispatchChange: setIndirectTime,
      },

      switchProps: {
        value: getValueFromStatus(overwriteIndirectTime),
        title: t("content.calculation.baseRateAndSupplements.edit.overwrite"),
        confirmationDialogProps: {
          titleOn: isInRootFolder
            ? t("content.calculation.baseRateAndSupplements.edit.confirmTitleNoActionPossible")
            : t("content.calculation.baseRateAndSupplements.edit.confirmTitleOn"),
          titleOff: t("content.calculation.baseRateAndSupplements.edit.confirmTitleOff"),
          description: isInRootFolder
            ? t("content.calculation.baseRateAndSupplements.edit.confirmDescriptionNoActionPossibleBaseRate")
            : t("content.calculation.baseRateAndSupplements.edit.confirmDescription"),
          handleConfirm: handleConfirmIndirectTime,
        },
      },
      title: t("content.calculation.baseRateAndSupplements.indirectTime"),
      descriptions: [
        t("content.calculation.baseRateAndSupplements.edit.lower") + " " + 53 + "%",
        t("content.calculation.baseRateAndSupplements.edit.median") + " " + 64 + "%",
        t("content.calculation.baseRateAndSupplements.edit.upper") + " " + 68 + "%",
      ],
      hasDividerBelow: true,
    },

    {
      inputFieldProps: {
        value: siteSpecificTime,
        disableInputField: !getValueFromStatus(overwriteSiteSpecificTime),
        inputType: "percent",
        maxValue: percentValue,
        dispatchChange: setSiteSpecificTime,
      },

      switchProps: {
        value: getValueFromStatus(overwriteSiteSpecificTime),
        title: t("content.calculation.baseRateAndSupplements.edit.overwrite"),
        confirmationDialogProps: {
          titleOn: isInRootFolder
            ? t("content.calculation.baseRateAndSupplements.edit.confirmTitleNoActionPossible")
            : t("content.calculation.baseRateAndSupplements.edit.confirmTitleOn"),
          titleOff: t("content.calculation.baseRateAndSupplements.edit.confirmTitleOff"),
          description: isInRootFolder
            ? t("content.calculation.baseRateAndSupplements.edit.confirmDescriptionNoActionPossibleBaseRate")
            : t("content.calculation.baseRateAndSupplements.edit.confirmDescription"),
          handleConfirm: handleConfirmSiteSpecificTime,
        },
      },
      title: t("content.calculation.baseRateAndSupplements.siteSpecificTime"),
      descriptions: [
        t("content.calculation.baseRateAndSupplements.edit.lower") + " " + 1 + "%",
        t("content.calculation.baseRateAndSupplements.edit.median") + " " + 2 + "%",
        t("content.calculation.baseRateAndSupplements.edit.upper") + " " + 4 + "%",
      ],
      hasDividerBelow: true,
    },

    {
      inputFieldProps: {
        value: personalTime,
        disableInputField: false,
        inputType: "percent",
        maxValue: percentValue,
        dispatchChange: setSiteSpecificTime,
        alwaysDisableInputField: true,
      },
      title: t("content.calculation.baseRateAndSupplements.personalTime"),
      descriptions: [t("content.calculation.baseRateAndSupplements.edit.notChange")],
      hasDividerBelow: false,
    },
  ];

  return (
    <Dialog maxWidth="sm" open={props.isOpen}>
      <DialogTitleStyled
        title={t("content.calculation.baseRateAndSupplements.edit.title")}
        description={t("content.calculation.baseRateAndSupplements.edit.description")}
        onClose={props.onClose}
        isOpen={props.isOpen}
        handleIconClose={closeDialog}
        textPaddingHorizontal={5}
      />
      <DialogContent sx={{ p: 0 }}>
        {editSections &&
          editSections.map((section) => (
            <Box key={section.title}>
              <EditSection section={section} isInRootFolder={isInRootFolder} />
              {section.hasDividerBelow && <Divider variant="fullWidth" sx={{ color: "primary.dark", marginTop: "-24px" }}></Divider>}
            </Box>
          ))}
      </DialogContent>

      <DialogActions sx={{ pr: 3 }}>
        <Button
          disabled={areInputsValid()}
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
