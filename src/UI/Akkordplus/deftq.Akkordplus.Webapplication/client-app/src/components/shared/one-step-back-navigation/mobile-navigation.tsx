import { useNavigate } from "react-router-dom";
import Box from "@mui/material/Box";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import KeyboardArrowLeftIcon from "@mui/icons-material/KeyboardArrowLeft";

interface Props {
  from: string;
}

export const MobileOneStepBackNavigation = ({ from }: Props) => {
  const navigate = useNavigate();
  return (
    <Box sx={{ display: "flex", paddingBottom: "10px", pt: "10px" }} onClick={() => navigate(-1)}>
      <IconButton sx={{ p: 0 }}>
        <KeyboardArrowLeftIcon />
      </IconButton>
      <Typography variant="subtitle1" sx={{ pt: "4px", pl: "10px" }}>
        {from}
      </Typography>
    </Box>
  );
};
