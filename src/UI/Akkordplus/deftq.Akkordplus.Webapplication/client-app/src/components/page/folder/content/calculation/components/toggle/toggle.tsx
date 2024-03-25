import { FormControlLabel, FormGroup, Switch, Typography } from "@mui/material";
import Box from "@mui/material/Box";
import { SwitchProps } from "components/page/folder/content/calculation/base-rate/edit/edit-base";
import { useTranslation } from "react-i18next";
import { useConfirm } from "../../../../../../shared/alert/confirm/hooks/use-confirm";
import { useNoOptionsConfirm } from "../../../../../../shared/alert/confirm/hooks/use-no-options-confirm";

interface Props {
  switchProps: SwitchProps;
  isInRootFolder?: boolean;
}

export const SwitchControl = ({ switchProps, isInRootFolder }: Props) => {
  const { t } = useTranslation();

  const getTitle = (): string => {
    if (switchProps.value) {
      return switchProps.confirmationDialogProps?.titleOff ?? "";
    }
    return switchProps.confirmationDialogProps?.titleOn ?? "";
  };

  const nonAction = () => {};

  const openConfirmDialogToggleOn = useConfirm({
    title: getTitle(),
    description: switchProps.confirmationDialogProps?.description ?? "",
    submit: switchProps.confirmationDialogProps?.handleConfirm ?? nonAction,
  });

  const openNoOptionsConfirm = useNoOptionsConfirm({
    title: switchProps.confirmationDialogProps?.titleOn ?? "",
    description: switchProps.confirmationDialogProps?.description ?? "",
  });

  const handleChange = () => {
    isInRootFolder ? openNoOptionsConfirm() : openConfirmDialogToggleOn();
  };

  return (
    <Box>
      <Typography width={"200px"} pb={1} variant="overline">
        {}
      </Typography>
      <Box>
        <FormGroup>
          <FormControlLabel
            control={<Switch checked={switchProps.value} onChange={() => handleChange()} inputProps={{ "aria-label": "controlled" }} />}
            label={switchProps.value ? t("common.on") : t("common.off")}
            sx={{ width: "100px" }}
          />
        </FormGroup>
      </Box>
    </Box>
  );
};
