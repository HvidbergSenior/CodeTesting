import { Fragment } from "react";
import CheckBoxOutlineBlankOutlinedIcon from "@mui/icons-material/CheckBoxOutlineBlankOutlined";
import CheckBoxOutlinedIcon from "@mui/icons-material/CheckBoxOutlined";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { costumPalette } from "theme/palette";

export interface Props {
  folder: ExtendedProjectFolder;
  disabled: boolean;
}

export function FolderExtraWorkIcon(props: Props) {
  const { folder, disabled } = props;

  return (
    <Fragment>
      {folder.folderExtraWork === "ExtraWork" && <CheckBoxOutlinedIcon sx={{ color: "secondary.main", opacity: disabled ? 0.4 : 1 }} />}
      {folder.folderExtraWork === "NormalWork" && (
        <CheckBoxOutlineBlankOutlinedIcon sx={{ color: costumPalette.iconGrey, opacity: disabled ? 0.4 : 1 }} />
      )}
    </Fragment>
  );
}
