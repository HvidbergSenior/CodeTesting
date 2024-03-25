import { UseFormGetValues } from "react-hook-form";
import { ProjectSpecificOperationFormData } from "./project-specific-operation-formdata";

export function useValidateProjectSpecificOperationInput() {
  const hasValidCreateInputs = (getValue: UseFormGetValues<ProjectSpecificOperationFormData>): boolean => {
    const name = getValue("newName");
    const type = getValue("timeType");
    const operationTimeMs = getValue("newOperationTimeMs");
    const workingTimeMs = getValue("newWorkingTimeMs");

    if (!name || name === "") {
      return false;
    }
    if (type === "operationTime" && (!operationTimeMs || operationTimeMs === 0)) {
      return false;
    }
    if (type === "workingTime" && (!workingTimeMs || workingTimeMs === 0)) {
      return false;
    }
    return true;
  };

  const hasValidUpdateInputs = (getValue: UseFormGetValues<ProjectSpecificOperationFormData>): boolean => {
    if (!hasDirtyUpdateInputs(getValue)) {
      return false;
    }
    return hasValidCreateInputs(getValue);
  };

  const hasDirtyUpdateInputs = (getValue: UseFormGetValues<ProjectSpecificOperationFormData>): boolean => {
    const oldExtrWorkNo = getValue("extraWorkAgreementNumber");
    const newExtrWorkNo = getValue("newExtraWorkAgreementNumber");
    if (oldExtrWorkNo !== newExtrWorkNo) {
      return true;
    }
    const oldName = getValue("name");
    const newName = getValue("newName");
    if (oldName !== newName) {
      return true;
    }
    const oldDesc = getValue("description");
    const newDesc = getValue("newDescription");
    if (oldDesc !== newDesc) {
      return true;
    }
    const type = getValue("timeType");
    if (type === "workingTime") {
      const oldWorkTime = getValue("workingTimeMs");
      const newWorkTime = getValue("newWorkingTimeMs");
      if (oldWorkTime !== newWorkTime) {
        return true;
      }
    }
    if (type === "operationTime") {
      const oldOperationTime = getValue("operationTimeMs");
      const newOperationTime = getValue("newOperationTimeMs");
      if (oldOperationTime !== newOperationTime) {
        return true;
      }
    }
    return false;
  };

  return { hasValidCreateInputs, hasValidUpdateInputs, hasDirtyUpdateInputs };
}
