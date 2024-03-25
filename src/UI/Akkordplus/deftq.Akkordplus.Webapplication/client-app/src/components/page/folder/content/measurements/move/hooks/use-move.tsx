import { useTranslation } from "react-i18next";
import { GetProjectResponse, usePutApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMoveMutation } from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { WorkItemsMove } from "../move";
import { useWorkItemRestrictions } from "shared/user-restrictions/use-workitems-restrictions";
import { useToast } from "shared/toast/hooks/use-toast";

interface Props {
  project: GetProjectResponse;
  folder: ExtendedProjectFolder | undefined;
  dataFlatlist: ExtendedProjectFolder[];
  selectedWorkitemIds: Set<string>;
}

export function useMoveWorkitems(props: Props) {
  const { project, folder, dataFlatlist, selectedWorkitemIds } = props;
  const { t } = useTranslation();
  const toast = useToast();
  const { canMoveWorkitems } = useWorkItemRestrictions(project);

  const [openMoveWorkitemsDialog, closeMoveWorkitemsDialog] = useDialog(WorkItemsMove);
  const [moveWorkitems] = usePutApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMoveMutation();

  const handleOnSuccessClose = () => {
    selectedWorkitemIds.clear();
    closeMoveWorkitemsDialog();
  };
  const wrapMutation = useMutation({
    onSuccess: handleOnSuccessClose,
    successProps: {
      description: t("content.measurements.move.success.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit = (destinationFolderId: string) => {
    wrapMutation(
      moveWorkitems({
        projectId: project.id ? project.id : "",
        folderId: folder?.projectFolderId ? folder.projectFolderId : "",
        moveWorkItemsRequest: {
          destinationFolderId: destinationFolderId ?? "",
          workItemIds: Array.from(selectedWorkitemIds),
        },
      })
    );
  };

  const handleClick = () => {
    if (!canMoveWorkitems(folder, selectedWorkitemIds)) {
      toast.error(t("content.measurements.move.restrictionError"));
      return;
    }
    openMoveWorkitemsDialog({
      project: project,
      dataFlatlist: dataFlatlist,
      onSubmit: handleOnSubmit,
    });
  };

  return handleClick;
}
