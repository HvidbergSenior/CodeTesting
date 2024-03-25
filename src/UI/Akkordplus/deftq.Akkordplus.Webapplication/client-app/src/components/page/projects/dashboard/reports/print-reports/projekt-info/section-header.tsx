import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import { useTranslation } from "react-i18next";
import { formatDate } from "utils/formats";

export const PrintSectionProjectInfoHeader = () => {
  const { t } = useTranslation();
  const dateNow = formatDate(new Date(), false);

  return (
    <Box sx={{ p: 0, flex: 1, display: "flex", width: "100%", justifyContent: "space-between", pb: 5 }}>
      <Box
        component="img"
        sx={{
          justifySelf: "start",
          width: "350px",
          height: "70px",
        }}
        alt="Logo"
        src="/logo_inv.png"
      />
      <Typography sx={{ pt: 3 }} variant="body2">
        {t("dashboard.reports.printReports.projektInfo.date", { date: dateNow })}
      </Typography>
    </Box>
  );
};
