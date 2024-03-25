import { useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import { ProjectResponse } from "api/generatedApi";
import { useAbortDialog } from "components/shared/alert/abort/hook/use-abort";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { DialogBaseProps } from "shared/dialog/types";
import { FormDataWorkItem } from "../../../measurements/create/create-work-item-dialog/create-work-item-form-data";
import { SelectSupplementsStep } from "../../../measurements/create/create-work-item-dialog/step-supplements/select-supplements-step";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";

interface Props extends DialogBaseProps {
  onSubmit: (data: string[]) => void;
  folder: ExtendedProjectFolder;
  project: ProjectResponse;
}

export const UpdateFolderSupplements = (props: Props) => {
  const { onSubmit, onClose, folder } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const nonFunction = () => {};
  const openAbortDialog = useAbortDialog({ closeDialog: onClose ?? nonFunction });

  const { getValues, setValue, control } = useForm<FormDataWorkItem>({
    mode: "all",
    defaultValues: {
      preSelectedSupplements: folder.folderSupplements?.map((s) => s.supplementId) ?? [],
    },
  });

  const getSelectedIds = (): string[] => {
    const selected = getValues("supplements")
      ?.filter((s) => !!s.supplementId && s.checked)
      ?.map((s) => s.supplementId);
    if (!selected) {
      return [];
    }
    const res: string[] = [];
    selected.forEach((s) => {
      if (!!s && s !== "") {
        res.push(s);
      }
    });
    return res;
  };

  const closeDialog = () => {
    if (!hasDirt()) {
      if (onClose) {
        onClose();
        return;
      }
    }

    openAbortDialog();
  };

  const onSave = () => {
    onSubmit(getSelectedIds());
  };

  const hasDirt = (): boolean => {
    if (!folder.folderSupplements || folder.folderSupplements.length === 0) {
      return false;
    }
    const selected = getSelectedIds();
    if (selected.length !== folder.folderSupplements?.length) {
      return true;
    }
    return false;
  };

  return (
    <Dialog
      maxWidth="sm"
      PaperProps={{
        sx: {
          height: screenSize === ScreenSizeEnum.Mobile ? "100%" : "750px", // mobile is ignored
          margin: screenSize === ScreenSizeEnum.Mobile ? "32px 7px" : "32px",
          width: screenSize === ScreenSizeEnum.Mobile ? "100%" : "calc(100´%-64px)",
        },
      }}
      open={props.isOpen}
    >
      <DialogTitleStyled
        title={
          folder.folderSupplements?.length === 0
            ? t("content.calculation.folderSupplements.dialog.titleAdd")
            : t("content.calculation.folderSupplements.dialog.titleUpdate")
        }
        description={t("content.calculation.folderSupplements.description")}
        onClose={onClose}
        isOpen={props.isOpen}
        handleIconClose={closeDialog}
        textPaddingHorizontal={5}
      />
      <DialogContent sx={{ p: 0, pb: 5 }}>
        <SelectSupplementsStep setValue={setValue} getValue={getValues} control={control} />
      </DialogContent>
      <DialogActions sx={{ pr: 3 }}>
        <Button variant="contained" sx={{ width: "160px" }} color="primary" onClick={onSave}>
          {t("common.save")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
