import { GetProjectResponse, ProjectFolderResponse, usePutApiProjectsByProjectIdFoldersAndSourceFolderIdCopyMutation } from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { useTranslation } from "react-i18next";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { FolderCopy } from "../copy";

interface Props {
  project: GetProjectResponse;
  selectedFolder: ProjectFolderResponse | undefined;
  dataFlatlist: ExtendedProjectFolder[];
}

export function useCopyFolder(props: Props) {
  const { project, selectedFolder, dataFlatlist } = props;
  const { t } = useTranslation();

  const [openCopyFolderDialog, closeCopyFolderDialog] = useDialog(FolderCopy);
  const [copyFolder] = usePutApiProjectsByProjectIdFoldersAndSourceFolderIdCopyMutation();

  const wrapMutation = useMutation({
    onSuccess: closeCopyFolderDialog,
    successProps: {
      description: t("folder.copy.success.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit = (data: string) => {
    wrapMutation(
      copyFolder({
        projectId: project.id ? project.id : "",
        sourceFolderId: selectedFolder?.projectFolderId ? selectedFolder.projectFolderId : "",
        copyProjectFolderRequest: {
          destinationFolderId: data,
        },
      })
    );
  };

  const handleClick = () => {
    openCopyFolderDialog({
      project: project,
      dataFlatlist: dataFlatlist,
      onSubmit: handleOnSubmit,
    });
  };

  return handleClick;
}
