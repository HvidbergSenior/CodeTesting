import { CreateProjectRequest } from "api/generatedApi";

// TODO configure CreateProjectFormData type when endpoint is complete
export type Participant = {
  id: number;
  name: string;
};

export interface ProjectSetupFormData extends CreateProjectRequest {
  importedFile: File;
  pieceworkAgreement: File;
  companyName?: string;
  workplaceAdr?: string;
  cvrNumber?: string;
  pNumber?: string;
  participants: Participant[];
  lumpSum?: number;
  projectDateStart?: Date;
  projectDateEnd?: Date;
  orderNumber?: string;
  pieceworkNumber?: string;
}
