import { GetProjectResponse } from "api/generatedApi";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { DesktopFolderContentNavigation } from "./desktop-navigation";
import { MobileFolderContentNavigation } from "./mobile-navigation";

interface Props {
  folder: ExtendedProjectFolder | undefined;
  project: GetProjectResponse;
  preSelectedPage: number;
  changeTabProps: (tabId: number) => void;
}

export const FolderContentNavigation = (props: Props) => {
  const { folder, project, preSelectedPage } = props;
  const { screenSize } = useScreenSize();

  return screenSize === ScreenSizeEnum.Mobile ? (
    <MobileFolderContentNavigation folder={folder} project={project} preSelectedPage={preSelectedPage} changeTabProps={props.changeTabProps} />
  ) : (
    <DesktopFolderContentNavigation folder={folder} project={project} preSelectedPage={preSelectedPage} changeTabProps={props.changeTabProps} />
  );
};
