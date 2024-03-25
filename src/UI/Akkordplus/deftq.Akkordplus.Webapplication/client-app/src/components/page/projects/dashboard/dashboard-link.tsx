import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, Typography } from "@mui/material";

interface Props {
  url?: string;
  text: string;
}

export const ProjectDashboardLink = (props: Props) => {
  const { url, text } = props;
  const navigate = useNavigate();
  const { t } = useTranslation();

  const onNavigate = () => {
    if (!url) {
      return;
    }
    navigate(url);
  };

  return (
    <Button variant="contained" sx={{ height: "150px", width: "100%" }} disabled={!url} onClick={onNavigate}>
      <Typography variant="body1" align="center">
        {t(text)}
      </Typography>
    </Button>
  );
};
