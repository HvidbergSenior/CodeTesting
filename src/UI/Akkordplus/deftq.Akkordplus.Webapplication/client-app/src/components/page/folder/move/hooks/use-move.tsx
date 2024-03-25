import { usePutApiProjectsByProjectIdFoldersMoveMutation } from "api/generatedApi";
import type { GetProjectResponse, ProjectFolderResponse } from "api/generatedApi";
import { FolderMove } from "components/page/folder/move/move";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { useTranslation } from "react-i18next";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useFolderRestrictions } from "shared/user-restrictions/use-folder-restrictions";
import { useToast } from "shared/toast/hooks/use-toast";

interface Props {
  project: GetProjectResponse;
  selectedFolder: ProjectFolderResponse | undefined;
  dataFlatlist: ExtendedProjectFolder[];
}

export function useMoveFolder(props: Props) {
  const { project, selectedFolder, dataFlatlist } = props;
  const { t } = useTranslation();
  const { canMoveFolder } = useFolderRestrictions(project);
  const toast = useToast();
  const [openMoveFolderDialog, closeMoveFolderDialog] = useDialog(FolderMove);
  const [moveFolder] = usePutApiProjectsByProjectIdFoldersMoveMutation();

  const wrapMutation = useMutation({
    onSuccess: closeMoveFolderDialog,
    successProps: {
      description: t("folder.move.success.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit = (data: string) => {
    wrapMutation(
      moveFolder({
        projectId: project.id ? project.id : "",
        moveProjectFolderRequest: {
          folderId: selectedFolder?.projectFolderId ? selectedFolder.projectFolderId : "",
          destinationFolderId: data,
        },
      })
    );
  };

  const handleClick = () => {
    if (!canMoveFolder()) {
      toast.error(t("folder.move.restrictionError"));
      return;
    }
    openMoveFolderDialog({
      project: project,
      dataFlatlist: dataFlatlist,
      onSubmit: handleOnSubmit,
    });
  };

  return handleClick;
}
