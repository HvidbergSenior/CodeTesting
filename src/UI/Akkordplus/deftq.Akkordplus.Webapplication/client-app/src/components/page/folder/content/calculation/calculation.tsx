import { Grid } from "@mui/material";
import { GetProjectResponse } from "api/generatedApi";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { FolderLockedButton } from "../../lock-folder/folder-locked-button";
import { BaseRateAndSupplements } from "./base-rate/base-rate";
import { FolderSupplements } from "./folder-supplements/folder-supplements";
import { PaymentFactor } from "./payment-factor/payment-factor";

interface Props {
  project: GetProjectResponse;
  folder: ExtendedProjectFolder;
}

export const FolderCalculation = (props: Props) => {
  const { project, folder } = props;
  const spacing = 3;
  const { screenSize } = useScreenSize();
  const maxWidth = screenSize === ScreenSizeEnum.LargeDesktop ? "1200px" : "900px";

  return (
    <Grid container alignItems={"center"} maxWidth={maxWidth} marginLeft={"auto"} marginRight={"auto"} rowSpacing={spacing} overflow={"auto"}>
      <Grid item xs={12} style={{ display: "flex", justifyContent: "flex-end", gap: 2 }}>
        {folder && <FolderLockedButton project={project} folder={folder} showText={true} />}
      </Grid>

      <Grid container item xs={12} spacing={spacing} pb={9}>
        <Grid container alignItems={"center"} item spacing={spacing}>
          <BaseRateAndSupplements folder={props.folder} project={project} />
          <PaymentFactor folder={props.folder} project={project} />
          <FolderSupplements folder={props.folder} project={project} />
        </Grid>
      </Grid>
    </Grid>
  );
};
