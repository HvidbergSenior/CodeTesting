import { FolderSupplementResponse } from "api/generatedApi";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useTranslation } from "react-i18next";

export interface MappedFolderSupplement extends FolderSupplementResponse {
  inherent: boolean;
  text: string;
}

interface Props {
  folder: ExtendedProjectFolder;
}

export const useFolderSupplementsMapper = ({ folder }: Props) => {
  const { t } = useTranslation();

  const mapSupplement = (sup: FolderSupplementResponse, inherent: boolean, newText?: string): MappedFolderSupplement => {
    const mappedSub = {
      supplementId: sup.supplementId,
      supplementNumber: sup.supplementNumber,
      supplementText: sup.supplementText,
      inherent,
      text: newText ?? sup.supplementText,
    } as MappedFolderSupplement;

    return mappedSub;
  };

  const mapText = (supplement: FolderSupplementResponse, folder: string): string => {
    return t("content.calculation.folderSupplements.inherentText", { supplementText: supplement.supplementText, folderName: folder });
  };

  const getInherentSupplements = (folder?: ExtendedProjectFolder): MappedFolderSupplement[] => {
    const list: MappedFolderSupplement[] = [];
    if (!folder) {
      return list;
    }

    if (folder.folderSupplements) {
      const mapped = folder.folderSupplements?.map((supplement) =>
        mapSupplement(supplement, true, mapText(supplement, folder.projectFolderName ?? ""))
      );
      list.push(...mapped);
    }

    if (folder.parent) {
      const newList = getInherentSupplements(folder.parent);
      newList.forEach((newSupplement) => {
        if (!list.some((supplement) => supplement.supplementId === newSupplement.supplementId)) {
          list.push(mapSupplement(newSupplement, true, mapText(newSupplement, folder.projectFolderName ?? "")));
        }
      });
    }
    return list;
  };
  const map = (): MappedFolderSupplement[] => {
    const list: MappedFolderSupplement[] = [];
    if (folder.folderSupplements) {
      const newList = folder.folderSupplements?.map((sub) => mapSupplement(sub, false));
      list.push(...newList);
    }
    const inherentList = getInherentSupplements(folder.parent);
    inherentList.forEach((sub) => {
      if (!list.some((s) => s.supplementId === sub.supplementId)) {
        list.push(sub);
      }
    });
    return list.sort((sup1, sup2) => {
      if (!sup1.supplementNumber || !sup2.supplementNumber) {
        return 0;
      }
      if (sup1.supplementNumber < sup2.supplementNumber) {
        return -1;
      }
      if (sup1.supplementNumber > sup2.supplementNumber) {
        return 1;
      }
      return 0;
    });
  };

  return map;
};
