import {
  GetWorkItemMaterialPreviewSupplementOperationRequest,
  GetWorkItemOperationPreviewSupplementRequest,
  MaterialMountingResponse,
  MaterialSupplementOperationRequest,
  MaterialSupplementRequest,
  SupplementOperationResponse,
  SupplementResponse,
  WorkItemSupplementOperationResponse,
  WorkItemSupplementResponse,
} from "api/generatedApi";
import { sortCompareString } from "utils/compares";

export interface ExtendedSupplementOperationResponse extends SupplementOperationResponse {
  amount: number;
}

export interface ExtendedSupplementResponse extends SupplementResponse {
  checked: boolean;
}

export interface ExtendedMaterialSupplementOperationRequest extends MaterialSupplementOperationRequest {
  text: string;
}

export function UseMapWorkItem() {
  const mapMountingCode = (data?: MaterialMountingResponse): number | undefined => {
    if (!data) return undefined;
    return data.mountingCode && data.mountingCode > 0 ? data.mountingCode : undefined;
  };
  const mapSupplementOperations = (data?: ExtendedSupplementOperationResponse[]): WorkItemSupplementOperationResponse[] | undefined => {
    if (!data) return undefined;
    return data
      .filter((so) => so?.text && so?.amount > 0)
      ?.map((os) => {
        if (os.text) {
          return { text: os.text, amount: os.amount };
        }
        return { text: "", amount: 0 };
      });
  };

  const mapSupplementOperationsRequests = (data?: ExtendedSupplementOperationResponse[]): MaterialSupplementOperationRequest[] => {
    if (!data) return [];
    return data
      .filter((os) => os.amount > 0)
      ?.map<MaterialSupplementOperationRequest>((os) => {
        if (os.supplementOperationId) {
          return { supplementOperationId: os.supplementOperationId, amount: os.amount };
        }
        return { supplementOperationId: "", amount: 0 };
      });
  };

  const mapExtendedSupplementOperationsRequests = (data?: ExtendedSupplementOperationResponse[]): ExtendedMaterialSupplementOperationRequest[] => {
    if (!data) return [];
    return data
      .filter((os) => os.amount > 0)
      ?.map<ExtendedMaterialSupplementOperationRequest>((os) => {
        if (os.supplementOperationId) {
          return { supplementOperationId: os.supplementOperationId, amount: os.amount, text: os.text ?? "" };
        }
        return { supplementOperationId: "", amount: 0, text: "" };
      });
  };

  const mapSupplementsToPreviewRequests = (data?: ExtendedSupplementResponse[]): GetWorkItemOperationPreviewSupplementRequest[] => {
    if (!data) return [];
    return data
      .filter((supplement) => supplement.checked)
      .map<GetWorkItemOperationPreviewSupplementRequest>((supplement) => {
        if (supplement.supplementId) {
          return { supplementId: supplement.supplementId };
        }
        return { supplementId: "" };
      });
  };

  const mapSupplementOperationsToPreviewRequests = (
    data?: ExtendedSupplementOperationResponse[]
  ): GetWorkItemMaterialPreviewSupplementOperationRequest[] => {
    if (!data) return [];
    return data
      .filter((os) => os.amount > 0)
      .map<GetWorkItemMaterialPreviewSupplementOperationRequest>((os) => {
        if (os.supplementOperationId) {
          return { supplementOperationId: os.supplementOperationId, amount: os.amount };
        }
        return { supplementOperationId: "", amount: 0 };
      });
  };

  const mapSupplementResponses = (data?: ExtendedSupplementResponse[]): WorkItemSupplementResponse[] | undefined => {
    if (!data) return undefined;
    return data.filter((incon) => incon?.supplementText && incon?.checked)?.map((inco) => (inco as WorkItemSupplementResponse) ?? undefined);
  };
  const mapExtendedSupplementsResponses = (
    data?: SupplementResponse[],
    preSelectedIds?: (string | undefined)[]
  ): ExtendedSupplementResponse[] | undefined => {
    if (!data) return undefined;
    const sorted = [...data].sort((a, b) => sortCompareString("desc", a.supplementNumber, b.supplementNumber));
    const mapped = sorted?.map((incon) => {
      const item = {
        supplementId: incon.supplementId,
        supplementNumber: incon.supplementNumber,
        supplementText: incon.supplementText,
        checked: !!incon.supplementId && preSelectedIds?.includes(incon.supplementId),
      } as ExtendedSupplementResponse;
      return item;
    });
    return mapped;
  };
  const mapSupplementRequests = (data?: ExtendedSupplementResponse[]): MaterialSupplementRequest[] => {
    if (!data) return [];
    const mapped = data
      .filter((sup) => sup?.checked)
      ?.map((sup) => {
        return { supplementId: sup.supplementId } as MaterialSupplementRequest;
      });
    return mapped;
  };
  const mapSupplementTexts = (data?: ExtendedSupplementResponse[]): string[] => {
    if (!data) return [];
    const mapped = data
      .filter((sup) => sup?.checked)
      ?.map((sup) => {
        return sup.supplementText ?? sup.supplementNumber ?? "";
      });
    const filtered = mapped.filter((t) => t !== "");
    return filtered;
  };
  return {
    mapMountingCode,
    mapSupplementOperations,
    mapSupplementOperationsRequests,
    mapExtendedSupplementOperationsRequests,
    mapSupplementResponses,
    mapExtendedSupplementsResponses,
    mapSupplementRequests,
    mapSupplementsToPreviewRequests,
    mapSupplementOperationsToPreviewRequests,
    mapSupplementTexts,
  };
}
