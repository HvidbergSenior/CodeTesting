import type { FavoritesResponse, FoundMaterial, FoundOperation, MaterialMountingResponse, WorkItemResponse, WorkItemType } from "api/generatedApi";
import { ExtendedSupplementOperationResponse, ExtendedSupplementResponse } from "../hooks/use-map-work-item";

export interface FormDataWorkItem {
  workitemType: WorkItemType;
  favorites: FavoritesResponse[];
  material: FoundMaterial;
  operation: FoundOperation;
  mountingCode?: MaterialMountingResponse;
  supplementOperations?: ExtendedSupplementOperationResponse[];
  supplementOperationsUsedMountingCode?: number;
  amount?: number;
  workItem: WorkItemResponse | undefined;
  supplements?: ExtendedSupplementResponse[];
  preSelectedSupplements?: (string | undefined)[];
}

export interface FormDataWorkItemDraft extends FormDataWorkItem {
  note?: string;
}

export interface FormDataWorkItemUpdate extends FormDataWorkItem {
  oldAmount?: number;
}
