import { GetProjectResponse } from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { CopyDraftWorkItemsToFolderDialog } from "../copy-draft-work-items-to-folder-dialog";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";

interface Prop {
  project: GetProjectResponse;
  foldersFlatlist: ExtendedProjectFolder[];
  selectedDraftIds: string[];
}

export function useCopyDraftWorkItemToFolder(props: Prop) {
  const { project, foldersFlatlist } = props;
  const [openCopyDraftWorkItemsToFolderDialog] = useDialog(CopyDraftWorkItemsToFolderDialog);

  const handleClick = () => {
    openCopyDraftWorkItemsToFolderDialog({ selectedDraftIds: props.selectedDraftIds, project, foldersFlatlist });
  };

  return handleClick;
}
