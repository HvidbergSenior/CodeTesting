import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useTranslation } from "react-i18next";
import type { DialogBaseProps } from "shared/dialog/types";

interface Props extends DialogBaseProps {
  onSubmit: () => void;
  folder: ExtendedProjectFolder;
}

export const FolderExtraWork = (props: Props) => {
  const { t } = useTranslation();
  const titleTranslate = props.folder.folderExtraWork === "ExtraWork" ? "folder.extraWork.normal.title" : "folder.extraWork.extraWork.title";
  const descriptionTranslate =
    props.folder.folderExtraWork === "ExtraWork" ? "folder.extraWork.normal.description" : "folder.extraWork.extraWork.description";
  const buttonTranslate = props.folder.folderExtraWork === "ExtraWork" ? "folder.extraWork.normal.button" : "folder.extraWork.extraWork.button";

  return (
    <Dialog maxWidth="xs" open={props.isOpen}>
      <DialogTitleStyled
        description={t(descriptionTranslate)}
        title={t(titleTranslate)}
        onClose={props.onClose}
        isOpen={props.isOpen}
        showDivider={false}
      />
      <DialogActions>
        <Button variant="outlined" onClick={props.onClose}>
          {t("common.close")}
        </Button>
        <Button variant="contained" color="primary" onClick={props.onSubmit}>
          {t(buttonTranslate)}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
