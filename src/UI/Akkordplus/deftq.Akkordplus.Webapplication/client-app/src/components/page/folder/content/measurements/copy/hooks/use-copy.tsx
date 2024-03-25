import type { GetProjectResponse } from "api/generatedApi";
import { usePostApiProjectsByProjectIdFoldersAndSourceFolderIdWorkitemsCopyMutation } from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { useTranslation } from "react-i18next";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { CopyWorkItems } from "../copy";
import { useWorkItemRestrictions } from "shared/user-restrictions/use-workitems-restrictions";
import { useToast } from "shared/toast/hooks/use-toast";

interface Props {
  project: GetProjectResponse;
  selectedFolder: ExtendedProjectFolder | undefined;
  dataFlatlist: ExtendedProjectFolder[];
  selectedIds: Set<string>;
}

export function useCopyWorkItems(props: Props) {
  const { project, dataFlatlist, selectedIds, selectedFolder } = props;
  const { t } = useTranslation();
  const { canCopyWorkitems } = useWorkItemRestrictions(project);
  const toast = useToast();

  const [openCopyWorkItemsDialog, closeCopyMeasurementsDialog] = useDialog(CopyWorkItems);
  const [copyMeasurements] = usePostApiProjectsByProjectIdFoldersAndSourceFolderIdWorkitemsCopyMutation();

  const handleSuccess = () => {
    selectedIds.clear();
    closeCopyMeasurementsDialog();
  };

  const wrapMutation = useMutation({
    onSuccess: handleSuccess,
    successProps: {
      description: t("content.measurements.copy.success.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit = (data: string) => {
    wrapMutation(
      copyMeasurements({
        projectId: project.id ? project.id : "",
        sourceFolderId: selectedFolder?.projectFolderId ? selectedFolder.projectFolderId : "",
        copyWorkItemsRequest: { destinationFolderId: data, workItemIds: Array.from(selectedIds) },
      })
    );
  };

  const handleClick = () => {
    if (!canCopyWorkitems(selectedFolder, selectedIds)) {
      toast.error(t("content.measurements.move.restrictionError"));
      return;
    }
    openCopyWorkItemsDialog({
      project: project,
      dataFlatlist: dataFlatlist,
      onSubmit: handleOnSubmit,
    });
  };

  return handleClick;
}
