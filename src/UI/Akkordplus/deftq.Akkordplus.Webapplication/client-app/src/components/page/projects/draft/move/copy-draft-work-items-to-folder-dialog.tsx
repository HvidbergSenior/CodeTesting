import { useState } from "react";
import { useTranslation } from "react-i18next";
import Dialog from "@mui/material/Dialog";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";
import { GetProjectResponse } from "api/generatedApi";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import type { DialogBaseProps } from "shared/dialog/types";
import { TreeSelector } from "components/shared/tree/selector/default";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { StepCopyDrafts } from "./step-copy/step-copy-drafts";
import { DraftWorkItemRequest, UseMapDrafts } from "./hooks/use-map-drafts";
import { useOfflineStorage } from "utils/offline-storage/use-offline-storage";
import { UseDraftsCreateWorkItems } from "./hooks/use-create-work-item";
import { useFolderRestrictions } from "shared/user-restrictions/use-folder-restrictions";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";

export interface Props extends DialogBaseProps {
  project: GetProjectResponse;
  foldersFlatlist: ExtendedProjectFolder[];
  selectedDraftIds: string[];
}

export function CopyDraftWorkItemsToFolderDialog(props: Props) {
  const { project, foldersFlatlist, selectedDraftIds } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const { mapToRequests } = UseMapDrafts();
  const { getOfflineWorkitemDrafts } = useOfflineStorage();
  const { createWorkItems } = UseDraftsCreateWorkItems();
  const { canMoveDraftIntoFolder } = useFolderRestrictions(project);

  const stepSelectFolder = 0;
  const stepMoveDrafts = 1;

  const [activeStep, setActiveStep] = useState(stepSelectFolder);
  const [selectedFolderId, setSelectedFolderId] = useState("");
  const [draftWorkItemRequests, setDraftWorkItemRequests] = useState<DraftWorkItemRequest[]>([]);
  const [disableClose, setDisableClose] = useState(true);
  const [validationError, setValidationError] = useState<string | undefined>();

  const folderSelected = (nodeId: string) => {
    setSelectedFolderId(nodeId);
  };

  const getHeader = () => {
    switch (activeStep) {
      case stepSelectFolder:
        return t("project.offline.copy.stepFolderSelect.title");
      case stepMoveDrafts:
        return t("project.offline.copy.stepCopy.title");
    }
  };

  const getDescription = () => {
    switch (activeStep) {
      case stepSelectFolder:
        return t("project.offline.copy.stepFolderSelect.description");
      case stepMoveDrafts:
        return t("project.offline.copy.stepCopy.description");
    }
  };

  const getContent = () => {
    switch (activeStep) {
      case stepSelectFolder:
        return <TreeSelector project={project} dataFlatlist={foldersFlatlist} folderSelectedProps={(nodeId: string) => folderSelected(nodeId)} />;
      case stepMoveDrafts:
        return <StepCopyDrafts draftRequests={draftWorkItemRequests} />;
    }
  };

  const validateStep = () => {
    if (activeStep === stepSelectFolder) {
      const foundFolder = foldersFlatlist.find((f) => f.projectFolderId === selectedFolderId);
      if (!canMoveDraftIntoFolder(foundFolder)) {
        setValidationError(t("project.offline.copy.stepFolderSelect.errorLockFolder"));
        return false;
      }
      setValidationError(undefined);
      return !!selectedFolderId;
    }
  };

  const createRequests = (): DraftWorkItemRequest[] => {
    const allDrafts = getOfflineWorkitemDrafts(project.id ?? "");
    const selectedDrafts = allDrafts.filter((draft) => selectedDraftIds.includes(draft.draftId));
    const requests = mapToRequests(project, selectedDrafts);
    setDraftWorkItemRequests(requests);
    return requests;
  };

  const createWorkItemsFromRequests = async (requests: DraftWorkItemRequest[]) => {
    await createWorkItems(project, selectedFolderId, requests);
    const failed = requests.filter((r) => r.requestFailed && !r.requestSuccess);
    if (failed?.length > 0) {
      setValidationError(t("project.offline.copy.stepCopy.errorFailedMove"));
      setDraftWorkItemRequests(failed);
    }
    setDisableClose(false);
  };

  const handleNext = () => {
    if (!validateStep()) {
      return;
    }
    if (activeStep === stepSelectFolder) {
      const requests = createRequests();
      createWorkItemsFromRequests(requests);
    }
    setActiveStep((prevActiveStep) => {
      return prevActiveStep + 1;
    });
  };

  const showMove = (): boolean => {
    return activeStep === stepSelectFolder;
  };

  const showClose = (): boolean => {
    return activeStep === stepMoveDrafts;
  };

  return (
    <Dialog
      maxWidth="sm"
      PaperProps={{
        sx: {
          minHeight: screenSize === ScreenSizeEnum.Mobile ? "100%" : "550px", // mobile is ignored
          margin: screenSize === ScreenSizeEnum.Mobile ? "32px 7px" : "32px",
          width: screenSize === ScreenSizeEnum.Mobile ? "100%" : "calc(100%-164px)",
        },
      }}
      open={props.isOpen}
    >
      <DialogTitleStyled title={getHeader()} description={getDescription()} showDivider={true} handleIconClose={props.onClose} isOpen={true} />
      {validationError && validationError !== "" && (
        <DialogContentText color={"error"} align={"center"}>
          {validationError}
        </DialogContentText>
      )}
      <DialogContent data-testid="is-the-the-container">{getContent()}</DialogContent>
      <DialogActions>
        {showMove() && (
          <Button
            data-testid="copy-draft-to-work-item-action-btn-move"
            disabled={selectedFolderId === ""}
            variant="contained"
            color="primary"
            onClick={handleNext}
          >
            {t("common.copyTo")}
          </Button>
        )}
        {showClose() && (
          <Button data-testid="copy-draft-to-work-item-action-btn-close" disabled={disableClose} variant="contained" onClick={props.onClose}>
            {t("common.close")}
          </Button>
        )}
      </DialogActions>
    </Dialog>
  );
}
