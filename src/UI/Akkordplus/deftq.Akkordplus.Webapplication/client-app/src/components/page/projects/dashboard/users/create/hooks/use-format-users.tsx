import { ProjectUserRole } from "api/generatedApi";
import { useTranslation } from "react-i18next";

export const useFormatUser = () => {
  const { t } = useTranslation();

  const formatRole = (role?: ProjectUserRole): string => {
    if (!role) {
      return "";
    }
    switch (role) {
      case "Owner":
        return t("dashboard.users.roles.owner");
      case "Participant":
        return t("dashboard.users.roles.participant");
      case "Manager":
        return t("dashboard.users.roles.manager");
      default:
        return "";
    }
  };

  return { formatRole };
};
