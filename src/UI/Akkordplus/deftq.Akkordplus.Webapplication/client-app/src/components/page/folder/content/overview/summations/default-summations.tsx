import { GetProjectResponse, useGetApiProjectsByProjectIdFoldersAndFolderIdSummationQuery } from "api/generatedApi";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { PieceWorkSummationsDesktop } from "./summations-desktop";
import { PieceWorkSummationsMobile } from "./summations-mobile";

interface Props {
  folder: ExtendedProjectFolder | undefined;
  project: GetProjectResponse;
}

const DefaultSummations = ({ folder, project }: Props) => {
  const { screenSize } = useScreenSize();
  const { data } = useGetApiProjectsByProjectIdFoldersAndFolderIdSummationQuery({
    projectId: project.id ?? "",
    folderId: folder?.projectFolderId ?? "",
  });

  if (screenSize === ScreenSizeEnum.Mobile) {
    return <PieceWorkSummationsMobile summations={data} />;
  }
  return <PieceWorkSummationsDesktop summations={data} />;
};

export default DefaultSummations;
