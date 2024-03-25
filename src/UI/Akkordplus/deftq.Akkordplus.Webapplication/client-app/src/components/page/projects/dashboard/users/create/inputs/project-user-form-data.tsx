import { UserRole } from "api/generatedApi";

export interface CreateProjectUserFormData {
  name: string;
  email: string;
  role: UserRole;
  adr?: string;
  phone?: string;
}
