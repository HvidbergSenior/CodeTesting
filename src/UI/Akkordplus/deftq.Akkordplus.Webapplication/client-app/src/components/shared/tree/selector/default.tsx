import { useState } from "react";
import type { GetProjectResponse } from "api/generatedApi";
import type { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useScreenSize, ScreenSizeEnum } from "shared/use-screen-size";
import { ProjectMobileTree } from "../mobile/tree/tree";
import { ProjectTree } from "../desktop/tree";

interface Props {
  project: GetProjectResponse;
  dataFlatlist: ExtendedProjectFolder[];
  folderSelectedProps: (nodeId: string) => void;
}

export const TreeSelector = (props: Props) => {
  const { project, dataFlatlist } = props;
  const { screenSize } = useScreenSize();
  const [selectedFolder, setSelectedFolder] = useState<ExtendedProjectFolder | undefined>(dataFlatlist.find((f) => f.isRoot));

  const folderSelected = (nodeId: string) => {
    const found = dataFlatlist.find((folder) => folder.projectFolderId === nodeId);
    setSelectedFolder(found);
    props.folderSelectedProps(nodeId);
  };
  return screenSize === ScreenSizeEnum.Mobile ? (
    <ProjectMobileTree
      project={project}
      selectedFolder={selectedFolder}
      folderSelectedProps={folderSelected}
      dataFlatlist={dataFlatlist}
      includeGoToContent={false}
    />
  ) : (
    <ProjectTree
      project={project}
      selectedFolder={selectedFolder}
      folderSelectedProps={folderSelected}
      dataFlatlist={dataFlatlist}
      includeMenu={false}
    />
  );
};
