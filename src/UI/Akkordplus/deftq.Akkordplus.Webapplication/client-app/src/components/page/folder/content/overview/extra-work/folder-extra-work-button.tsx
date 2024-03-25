import IconButton from "@mui/material/IconButton";
import { GetProjectResponse } from "api/generatedApi";
import { TextDirection, TextIcon } from "components/shared/text-icon/text-icon";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useFolderRestrictions } from "shared/user-restrictions/use-folder-restrictions";
import { FolderExtraWorkIcon } from "./folder-extra-work-icon";
import { useExtraWorkFolder } from "./hooks/use-extra-work-folder";

export interface Props {
  project: GetProjectResponse;
  folder: ExtendedProjectFolder;
  textDirection: TextDirection;
}

export function FolderExtraWorkButton(props: Props) {
  const { project, folder } = props;
  const { canMarkExtraWork } = useFolderRestrictions(project);
  const openDialog = useExtraWorkFolder({ project, folder });

  const changeState = () => {
    if (!canMarkExtraWork(folder)) {
      return;
    }
    openDialog();
  };

  return (
    <TextIcon translateText={"folder.extraWork.iconText"} textDirection={props.textDirection}>
      <IconButton onClick={changeState} disabled={!canMarkExtraWork(folder)}>
        <FolderExtraWorkIcon folder={folder} disabled={!canMarkExtraWork(folder)} />
      </IconButton>
    </TextIcon>
  );
}
