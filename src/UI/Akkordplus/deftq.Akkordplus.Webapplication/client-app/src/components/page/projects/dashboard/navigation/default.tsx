import { useTranslation } from "react-i18next";
import { DefaultOneStepBackNavigation } from "components/shared/one-step-back-navigation/default";

interface Props {
  here: string;
}

export const DefaultBack2DashBoardNavigation = (props: Props) => {
  const { here } = props;
  const { t } = useTranslation();

  return <DefaultOneStepBackNavigation from={t("dashboard.title")} here={here} />;
};
