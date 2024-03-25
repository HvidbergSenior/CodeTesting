import { ExtraWorkAgreementResponse } from "api/generatedApi";
import { UseFormGetValues } from "react-hook-form";

export interface ExtraWorkAgreementFormData extends ExtraWorkAgreementResponse {
  number: string;
}

export function useValidateExtraWorkAgreementInput() {
  const hasDirtyInputs = (getValue: UseFormGetValues<ExtraWorkAgreementFormData>, old?: ExtraWorkAgreementResponse): boolean => {
    const number = getValue("extraWorkAgreementNumber");
    const name = getValue("name");
    const desc = getValue("description");
    const type = getValue("extraWorkAgreementType");
    const payment = getValue("paymentDkr");
    const workHours = getValue("workTime.hours");
    const workMinutes = getValue("workTime.minutes");

    if (!old) {
      return false;
    }
    if (old.extraWorkAgreementNumber !== number || old.name !== name || old.description !== desc || old.extraWorkAgreementType !== type) {
      return true;
    }
    if (type === "AgreedPayment" && old.paymentDkr !== payment) {
      return true;
    }
    if (type === "CompanyHours" || type === "CustomerHours") {
      if (old.workTime?.hours !== workHours || old.workTime?.minutes !== workMinutes) {
        return true;
      }
    }
    return false;
  };

  const hasValidateInputs = (getValue: UseFormGetValues<ExtraWorkAgreementFormData>): boolean => {
    const name = getValue("name");
    const type = getValue("extraWorkAgreementType");
    const payment = getValue("paymentDkr");
    const workHours = getValue("workTime.hours");
    const workMinutes = getValue("workTime.minutes");
    if (!name || name === "") {
      return false;
    }
    if (!type) {
      return false;
    }
    if (type === "AgreedPayment") {
      if (!payment || payment === 0) {
        return false;
      }
    }
    if (type === "CompanyHours" || type === "CustomerHours") {
      if ((!workHours && !workMinutes) || (workHours === 0 && workMinutes === 0)) {
        return false;
      }
    }
    return true;
  };

  return { hasValidateInputs, hasDirtyInputs };
}
