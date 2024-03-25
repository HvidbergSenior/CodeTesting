import { useEffect } from "react";
import { UseFormGetValues, UseFormSetValue } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Grid from "@mui/material/Grid";
import CircularProgress from "@mui/material/CircularProgress";
import {
  usePostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMaterialPreviewMutation,
  usePostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsOperationPreviewMutation,
  WorkItemMaterialResponse,
  WorkItemOperationResponse,
} from "api/generatedApi";
import { useToast } from "shared/toast/hooks/use-toast";
import { UseMapWorkItem } from "../../hooks/use-map-work-item";
import { FormDataWorkItem } from "../create-work-item-form-data";

export interface Props {
  getValue: UseFormGetValues<FormDataWorkItem>;
  setValue: UseFormSetValue<FormDataWorkItem>;
  projectId: string;
  folderId: string;
  onPreviewCalculatedProps: () => void;
  isMaterialWorkItem: boolean;
}

export function CalculationForPreviewStep(props: Props) {
  const { onPreviewCalculatedProps, getValue, setValue, projectId, folderId, isMaterialWorkItem } = props;
  const { t } = useTranslation();
  const toast = useToast();
  const { mapSupplementOperations, mapSupplementResponses, mapSupplementsToPreviewRequests, mapSupplementOperationsToPreviewRequests } =
    UseMapWorkItem();
  const [getMaterialPreview] = usePostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMaterialPreviewMutation();
  const [getOperationPreview] = usePostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsOperationPreviewMutation();

  useEffect(() => {
    if (isMaterialWorkItem) {
      setMaterialPreview();
    } else {
      setOperationPreview();
    }
  }, []);

  const setMaterialPreview = async () => {
    const workItemAmount = getValue("amount") ?? 0;
    const supplementOperations = mapSupplementOperations(getValue("supplementOperations"));
    const supplementOperationsForPreview = mapSupplementOperationsToPreviewRequests(getValue("supplementOperations"));
    const supplements = mapSupplementResponses(getValue("supplements"));

    const workItemText = getValue("material.name") ?? "Ukendt materiale";
    const mountingCode = getValue("mountingCode");
    const workItemEanNumber = getValue("material.eanNumber") ?? "Ukendt EAN nummer";
    const workItemMaterialId = getValue("material.id") ?? "Ukendt id";
    const workItemMaterial: WorkItemMaterialResponse = {
      workItemEanNumber: workItemEanNumber,
      workItemMountingCode: mountingCode?.mountingCode,
      workItemMountingCodeText: mountingCode?.text,
      supplementOperations: supplementOperations,
    };

    const workItemType = "Material";

    await getMaterialPreview({
      projectId: projectId,
      folderId: folderId,
      getWorkItemMaterialPreviewRequest: {
        materialId: workItemMaterialId,
        workItemAmount: workItemAmount,
        workItemMountingCode: mountingCode?.mountingCode,
        supplementOperations: supplementOperationsForPreview,
        supplements: supplements,
      },
    })
      .unwrap()
      .then((response) => {
        const workItemTotalOperationTimeMilliseconds = response.totalWorkTimeMilliseconds;
        const workItemOperationTimeMilliseconds = response.operationTimeMilliseconds;
        const workItemTotalPaymentDkr = response.workItemTotalPaymentDkr;

        setValue("workItem", {
          workItemType,
          workItemText,
          workItemAmount,
          supplements,
          workItemMaterial,
          workItemTotalOperationTimeMilliseconds,
          workItemOperationTimeMilliseconds,
          workItemTotalPaymentDkr,
        });

        onPreviewCalculatedProps();
      })
      .catch((error) => {
        console.error(error);
        toast.error(t("content.measurements.create.calculatePreview.calculatePreviewMaterialError"));
      });
  };

  const setOperationPreview = async () => {
    const workItemAmount = getValue("amount") ?? 0;
    const supplementsForPreview = mapSupplementsToPreviewRequests(getValue("supplements"));
    const supplements = mapSupplementResponses(getValue("supplements"));

    const workItemText = getValue("operation.operationText") ?? "Ukendt materiale";
    const workItemOperationId = getValue("operation.operationId") ?? "Ukendt operationsid";
    const workItemOperation: WorkItemOperationResponse = {
      operationNumber: getValue("operation.operationNumber") ?? "Ukendt operationsnummer",
    };

    const workItemType = "Operation";

    await getOperationPreview({
      projectId: projectId,
      folderId: folderId,
      getWorkItemOperationPreviewRequest: {
        operationId: workItemOperationId,
        workItemAmount: workItemAmount,
        supplements: supplementsForPreview,
      },
    })
      .unwrap()
      .then((response) => {
        const workItemTotalOperationTimeMilliseconds = response.totalWorkTimeMilliseconds;
        const workItemOperationTimeMilliseconds = response.operationTimeMilliseconds;
        const workItemTotalPaymentDkr = response.workItemTotalPaymentDkr;

        setValue("workItem", {
          workItemType,
          workItemText,
          workItemAmount,
          supplements,
          workItemOperation,
          workItemTotalOperationTimeMilliseconds,
          workItemOperationTimeMilliseconds,
          workItemTotalPaymentDkr,
        });

        onPreviewCalculatedProps();
      })
      .catch((error) => {
        console.error(error);
        toast.error(t("content.measurements.create.calculatePreview.calculatePreviewOperationError"));
      });
  };

  return (
    <Grid container sx={{ padding: "20px" }}>
      <Grid item xs={12} sx={{ pt: 5, pb: 5, textAlign: "center" }}>
        <CircularProgress size={100} />
      </Grid>
    </Grid>
  );
}
