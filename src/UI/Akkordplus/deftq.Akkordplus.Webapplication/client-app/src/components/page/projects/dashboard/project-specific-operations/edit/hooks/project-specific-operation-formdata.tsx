import { GetProjectSpecificOperationResponse } from "api/generatedApi";

export type ProjectSpecificOperationTimeType = "workingTime" | "operationTime";

export interface ProjectSpecificOperationFormData extends GetProjectSpecificOperationResponse {
  timeType: ProjectSpecificOperationTimeType;
  newExtraWorkAgreementNumber?: string;
  newName?: string;
  newDescription?: string;
  newOperationTimeMs?: number;
  newWorkingTimeMs?: number;
}
