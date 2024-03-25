import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import { GetExtraWorkAgreementRatesQueryResponse, ProjectResponse } from "api/generatedApi";
import { EditSection } from "components/page/folder/content/calculation/components/section/edit-section";
import { SectionProps } from "components/page/folder/content/calculation/base-rate/edit/edit-base";
import { useAbortDialog } from "components/shared/alert/abort/hook/use-abort";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import { useInputValidation } from "components/shared/validation/input-validation";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import type { DialogBaseProps } from "shared/dialog/types";
import { formatNumberToAmount } from "utils/formats";

interface Props extends DialogBaseProps {
  onSubmit: (customerHours: string, companyHours: string) => void;
  project: ProjectResponse;
  data: GetExtraWorkAgreementRatesQueryResponse | undefined;
}

export const EditHours = ({ onSubmit, onClose, data, isOpen }: Props) => {
  const { t } = useTranslation();
  const { validateEndsWithCommaOrStartsWithComma, validateSum } = useInputValidation();
  const nonFunction = () => {};
  const openAbortDialog = useAbortDialog({ closeDialog: onClose ?? nonFunction });
  const formattedCustomerRate = formatNumberToAmount(data?.customerRatePerHourDkr ?? 0);
  const formattedCompanyRate = formatNumberToAmount(data?.companyRatePerHourDkr ?? 0);
  const [customerHours, setCustomerHours] = useState<string>(formattedCustomerRate);
  const [companyHours, setCompanyHours] = useState<string>(formattedCompanyRate);
  const maxValue = 1000000;

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
    const hasSameValues = companyHours === formattedCompanyRate && customerHours === formattedCustomerRate;

    return !hasSameValues;
  };

  const editSections: SectionProps[] = [
    {
      inputFieldProps: {
        value: companyHours,
        disableInputField: false,
        inputType: "currency",
        maxValue: maxValue,
        dispatchChange: setCompanyHours,
      },
      hasDividerBelow: false,
      title: t("dashboard.projectInfo.editHours.captionCompanyHoursRate"),
      descriptions: [],
    },
    {
      inputFieldProps: {
        value: customerHours,
        disableInputField: false,
        inputType: "currency",
        maxValue: maxValue,
        dispatchChange: setCustomerHours,
      },
      hasDividerBelow: false,
      title: t("dashboard.projectInfo.editHours.captionCustomerHoursRate"),
      descriptions: [],
    },
  ];

  const prepareSubmit = () => {
    // convert to US decimal format before submit, because submit parses string as float, which looks for . not ,
    const tempCustomerHours = customerHours.replace(".", "").replace(",", ".");
    const tempCompanyHours = companyHours.replace(".", "").replace(",", ".");

    onSubmit(tempCustomerHours, tempCompanyHours);
  };

  return (
    <Dialog maxWidth="xs" open={isOpen}>
      <DialogTitleStyled title={t("dashboard.projectInfo.editHours.title")} onClose={onClose} isOpen={isOpen} handleIconClose={closeDialog} />
      <DialogContent sx={{ p: 0, pb: 5 }}>
        {editSections && editSections.map((section) => <EditSection key={section.title} section={section} />)}
      </DialogContent>
      <DialogActions sx={{ pr: 3 }}>
        <Button
          disabled={
            !validateSum(customerHours, maxValue) ||
            !validateSum(companyHours, maxValue) ||
            !validateEndsWithCommaOrStartsWithComma(customerHours) ||
            !validateEndsWithCommaOrStartsWithComma(companyHours)
          }
          variant="contained"
          sx={{ width: "160px" }}
          color="primary"
          onClick={() => prepareSubmit()}
        >
          {t("common.save")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
