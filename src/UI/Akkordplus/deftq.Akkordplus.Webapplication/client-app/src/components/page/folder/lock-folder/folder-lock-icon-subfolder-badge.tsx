import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import LockOpenOutlinedIcon from "@mui/icons-material/LockOpenOutlined";

import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { FolderLockIcon } from "./folder-lock-icon";
import { Badge, BadgeProps, styled } from "@mui/material";
import { costumPalette } from "theme/palette";

export interface Props {
  folder: ExtendedProjectFolder | undefined;
}

export function FolderLockIconSubfolderBadge(props: Props) {
  const { folder } = props;

  const hasOppesiteSubvalues = () => {
    if (!folder) {
      return false;
    }
    if (!folder.flatlist || folder.flatlist.length === 0) {
      return false;
    }
    const res = folder.flatlist.every((f) => f.projectFolderLocked === folder.projectFolderLocked);
    return !res;
  };

  const StyledBadge = styled(Badge)<BadgeProps>(({ theme }) => ({
    "& .MuiBadge-badge": {
      padding: 0,
      margin: 0,
      height: "16px",
      minWidth: "16px",
      top: "6px",
      right: "2px",
      background: costumPalette.iconBadgeGrey,
    },
    "& .MuiBadge-badge svg": {
      height: "10px",
      width: "10px",
      color: costumPalette.iconGrey,
    },
  }));
  return (
    <StyledBadge
      badgeContent={folder?.projectFolderLocked === "Unlocked" ? <LockOutlinedIcon /> : <LockOpenOutlinedIcon />}
      anchorOrigin={{
        vertical: "bottom",
        horizontal: "right",
      }}
      invisible={!hasOppesiteSubvalues()}
      data-testid={`folder-lock-icon-with-badge-${folder?.projectFolderName}`}
    >
      {folder && <FolderLockIcon folder={folder} disabled={false} />}
    </StyledBadge>
  );
}
