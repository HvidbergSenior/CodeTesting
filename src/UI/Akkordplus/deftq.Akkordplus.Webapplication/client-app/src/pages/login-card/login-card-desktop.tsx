import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Card from "@mui/material/Card";
import Typography from "@mui/material/Typography";
import DefLogo from "assets/icons/Def_logo.svg";
import Logo from "assets/icons/Login_logo.svg";
import TekniqLogo from "assets/icons/Tekniq_logo.svg";
import { useTranslation } from "react-i18next";
import { useOnlineStatus } from "utils/connectivity/hook/use-online-status";

interface Props {
  handleLogin: () => void;
}

export const LoginCardDesktop = ({ handleLogin }: Props) => {
  const { t } = useTranslation();
  const isOnline = useOnlineStatus();

  return (
    <Card
      variant="outlined"
      sx={{ position: "absolute", top: "50%", left: "50%", transform: "translate(-50%, -50%)", width: "692px", height: "456px" }}
    >
      <Box sx={{ width: "100%", height: "100%", position: "relative" }}>
        <Box sx={{ display: "flex", justifyContent: "center", alignItems: "center", flexDirection: "column", height: "100%" }}>
          <Typography variant="h4" sx={{ marginBottom: 3, fontWeight: "200", fontSize: "24px", color: "primary.main" }}>
            {t("login.welcome")}
          </Typography>
          <Box component="img" sx={{ width: "420px", marginBottom: 3 }} alt="Logo" src={Logo} />
          <Typography variant="subtitle2" align="center" sx={{ color: "grey.50", marginBottom: 3, width: "400px", pr: 4, pl: 4 }}>
            {t("login.description")}
          </Typography>
          <Button variant="contained" sx={{ minWidth: "220px" }} onClick={handleLogin} disabled={!isOnline}>
            {t("login.login")}
          </Button>
        </Box>
        <Box
          sx={{
            position: "absolute",
            width: "100%",
            bottom: "24px",
            pl: 4,
            pr: 4,
            display: "flex",
            alignItems: "center",
            justifyContent: "start",
          }}
        >
          <Box component="img" sx={{ width: "100px", pr: "10px" }} alt="Dansk el-forbund Logo" src={DefLogo} />
          <Box component="img" sx={{ width: "100px" }} alt="Tekniq Logo" src={TekniqLogo} />
        </Box>
      </Box>
    </Card>
  );
};
