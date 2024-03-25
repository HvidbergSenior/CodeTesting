import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import { GetProjectResponse } from "api/generatedApi";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import { usePaperProps } from "components/shared/dialog/paper-props";
import { TreeSelector } from "components/shared/tree/selector/default";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import type { DialogBaseProps } from "shared/dialog/types";

interface Props extends DialogBaseProps {
  project: GetProjectResponse;
  dataFlatlist: ExtendedProjectFolder[];
  onSubmit: (destinationFolderId: string) => void;
}

export const CopyWorkItems = (props: Props) => {
  const { project, dataFlatlist, onSubmit } = props;
  const { t } = useTranslation();
  const [selectedFolderId, setSelectedFolderId] = useState("");
  const { paperProps } = usePaperProps();

  return (
    <Dialog maxWidth="sm" PaperProps={paperProps} open={props.isOpen}>
      <DialogTitleStyled
        title={t("content.measurements.copy.title")}
        description={t("content.measurements.copy.description")}
        onClose={props.onClose}
        isOpen={props.isOpen}
      />
      <DialogContent>
        <TreeSelector project={project} dataFlatlist={dataFlatlist} folderSelectedProps={(nodeId: string) => setSelectedFolderId(nodeId)} />
      </DialogContent>
      <DialogActions>
        <Button variant="outlined" onClick={props.onClose}>
          {t("common.cancel")}
        </Button>
        <Button variant="contained" disabled={selectedFolderId === ""} color="primary" onClick={() => onSubmit(selectedFolderId)}>
          {t("common.copyTo")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
