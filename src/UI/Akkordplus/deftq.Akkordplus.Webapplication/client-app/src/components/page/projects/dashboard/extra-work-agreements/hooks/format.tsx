import { ExtraWorkAgreementResponse, ExtraWorkAgreementTypeRequest } from "api/generatedApi";
import { useTranslation } from "react-i18next";

export const useExtraWorkAgreementFormatter = () => {
  const { t } = useTranslation();

  const getTypeByAgreementFormatted = (row?: ExtraWorkAgreementResponse | null): string => {
    if (!row?.extraWorkAgreementType) {
      return "";
    }
    return getTypeFormatted(row.extraWorkAgreementType);
  };

  const getTypeFormatted = (type: ExtraWorkAgreementTypeRequest): string => {
    switch (type) {
      case "AgreedPayment":
        return t("dashboard.extraWorkAgreements.types.agreedPayment");
      case "CompanyHours":
        return t("dashboard.extraWorkAgreements.types.companyHours");
      case "CustomerHours":
        return t("dashboard.extraWorkAgreements.types.customerHours");
      case "Other":
        return t("dashboard.extraWorkAgreements.types.other");
    }
  };

  return { getTypeFormatted, getTypeByAgreementFormatted };
};
