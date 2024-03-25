import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";

interface Props {
  translationProperty: string;
}

export const ReportCardContentList = ({ translationProperty }: Props) => {
  const { t } = useTranslation();
  const contentItems = t(translationProperty, { returnObjects: true }) as string[];
  const listStyles = { padding: "0 0 0 20px", margin: 0 };
  const itemStyles = { padding: "0 0 3px 0" };

  return (
    <Box>
      <Typography variant="body2" fontStyle="italic">
        {t("dashboard.reports.cardContentTitle")}
      </Typography>
      <ul style={listStyles}>
        {contentItems?.map((item, index) => {
          return (
            <li key={index} style={itemStyles}>
              <Typography variant="body2">{item}</Typography>
            </li>
          );
        })}
      </ul>
    </Box>
  );
};
