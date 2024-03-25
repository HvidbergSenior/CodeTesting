import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import { EditSection } from "components/page/folder/content/calculation/components/section/edit-section";
import { SectionProps } from "components/page/folder/content/calculation/base-rate/edit/edit-base";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import { useInputValidation } from "components/shared/validation/input-validation";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import type { DialogBaseProps } from "shared/dialog/types";
import { formatNumberToPrice } from "utils/formats";

interface Props extends DialogBaseProps {
  onSubmit: (sum: string) => void;
  lumpsum: number;
}

export const EditAgreedLumpsum = (props: Props) => {
  const { onSubmit, lumpsum } = props;
  const { t } = useTranslation();
  const [agreedLumpum, setAgreedLumpsum] = useState<string>(formatNumberToPrice(lumpsum));
  const { validateSum, validateEndsWithCommaOrStartsWithComma } = useInputValidation();

  const editSections: SectionProps[] = [
    {
      inputFieldProps: {
        value: agreedLumpum,
        inputType: "currency",
        disableInputField: false,
        alwaysDisableInputField: false,
        maxValue: 100000000000,
        dispatchChange: setAgreedLumpsum,
      },
      title: t("dashboard.editAgreedLumpsum.caption"),
      descriptions: [],
    },
  ];

  return (
    <Dialog maxWidth="xs" open={props.isOpen}>
      <DialogTitleStyled title={t("dashboard.editAgreedLumpsum.title")} onClose={props.onClose} isOpen={props.isOpen} />
      <DialogContent sx={{ p: "0" }}>
        {editSections && editSections.map((section) => <EditSection key={section.title} section={section} />)}
      </DialogContent>

      <DialogActions sx={{ pr: 3 }}>
        <Button
          variant="contained"
          disabled={!(validateEndsWithCommaOrStartsWithComma(agreedLumpum) && validateSum(agreedLumpum, editSections[0].inputFieldProps.maxValue))}
          sx={{ width: "160px" }}
          color="primary"
          onClick={() => onSubmit(agreedLumpum)}
        >
          {t("common.save")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
