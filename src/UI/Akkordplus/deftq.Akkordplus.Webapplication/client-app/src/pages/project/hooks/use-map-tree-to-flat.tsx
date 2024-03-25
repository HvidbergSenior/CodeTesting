import type { ProjectFolderResponse } from "api/generatedApi";

export interface ExtendedProjectFolder extends ProjectFolderResponse {
  parent: ExtendedProjectFolder | undefined;
  flatlist: ExtendedProjectFolder[] | undefined;
  isRoot: boolean;
}

export function useMapTreeToFlat() {
  const mapTreeToFlat = (
    treestructureFolders: ProjectFolderResponse[],
    parent: ExtendedProjectFolder
  ): {
    folders: ExtendedProjectFolder[];
    flatlist: ExtendedProjectFolder[] | undefined;
  } => {
    const folders: ExtendedProjectFolder[] = [];
    const flatlist: ExtendedProjectFolder[] = [];
    treestructureFolders.forEach((n) => {
      const node = mapTreeNode(n, parent);
      if (Array.isArray(n.subFolders)) {
        const childNodes = mapTreeToFlat(n.subFolders, node);
        node.subFolders = childNodes.folders;
        node.flatlist = childNodes.flatlist;
      }

      node.flatlist?.forEach((fn) => flatlist.push(fn));
      flatlist.push(node);
      folders.push(node);
    });
    return { folders, flatlist };
  };

  const mapTreeNode = (pf: ProjectFolderResponse, parent: ExtendedProjectFolder | undefined, isRoot: boolean = false): ExtendedProjectFolder => {
    const node = {} as ExtendedProjectFolder;
    Object.assign(node, pf);
    node.parent = parent;
    node.isRoot = isRoot;
    return node;
  };

  return { mapTreeToFlat, mapTreeNode };
}
