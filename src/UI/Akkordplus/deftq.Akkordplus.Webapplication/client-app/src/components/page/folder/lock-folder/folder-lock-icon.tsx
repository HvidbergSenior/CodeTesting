import { Fragment } from "react";
import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import LockOpenOutlinedIcon from "@mui/icons-material/LockOpenOutlined";

import { ProjectFolderResponse } from "api/generatedApi";
import { costumPalette } from "theme/palette";

export interface Props {
  folder: ProjectFolderResponse;
  disabled: boolean;
}

export function FolderLockIcon(props: Props) {
  const { folder, disabled } = props;

  return (
    <Fragment>
      {folder?.projectFolderLocked === "Locked" && <LockOutlinedIcon sx={{ color: costumPalette.iconGrey, opacity: disabled ? 0.4 : 1 }} />}
      {folder?.projectFolderLocked === "Unlocked" && <LockOpenOutlinedIcon sx={{ color: costumPalette.iconGrey, opacity: disabled ? 0.4 : 1 }} />}
    </Fragment>
  );
}
