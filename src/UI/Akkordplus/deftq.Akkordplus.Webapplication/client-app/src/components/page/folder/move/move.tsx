import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";
import DialogContent from "@mui/material/DialogContent";
import { useTranslation } from "react-i18next";
import type { DialogBaseProps } from "shared/dialog/types";
import { GetProjectResponse } from "api/generatedApi";
import { useState } from "react";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { TreeSelector } from "components/shared/tree/selector/default";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import { usePaperProps } from "components/shared/dialog/paper-props";

interface Props extends DialogBaseProps {
  project: GetProjectResponse;
  dataFlatlist: ExtendedProjectFolder[];
  onSubmit: (destinationFolderId: string) => void;
}

export const FolderMove = (props: Props) => {
  const { project, dataFlatlist, onSubmit } = props;
  const { t } = useTranslation();
  const [selectedFolderId, setSelectedFolderId] = useState("");
  const { paperProps } = usePaperProps();
  const folderSelected = (nodeId: string) => {
    setSelectedFolderId(nodeId);
  };

  return (
    <Dialog maxWidth="sm" open={props.isOpen} PaperProps={paperProps}>
      <DialogTitleStyled title={t("folder.move.title")} description={t("folder.move.description")} onClose={props.onClose} isOpen={props.isOpen} />
      <DialogContent>
        <TreeSelector project={project} dataFlatlist={dataFlatlist} folderSelectedProps={(nodeId: string) => folderSelected(nodeId)} />
      </DialogContent>
      <DialogActions>
        <Button variant="outlined" onClick={props.onClose}>
          {t("common.cancel")}
        </Button>
        <Button variant="contained" color="primary" onClick={() => onSubmit(selectedFolderId)}>
          {t("common.move")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
