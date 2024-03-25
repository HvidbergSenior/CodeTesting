import { useNavigate } from "react-router-dom";
import Box from "@mui/material/Box";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";

interface Props {
  from: string;
  here: string;
}

export const DesktopOneStepBackNavigation = (props: Props) => {
  const { from, here } = props;
  const navigate = useNavigate();
  return (
    <Box sx={{ display: "flex", pl: 3, pb: 2, pt: 4, cursor: "pointer" }} onClick={() => navigate(-1)}>
      <Typography variant="h5" color={"primary.main"} sx={{ pt: "4px", pl: "10px" }}>
        {from}
      </Typography>
      <IconButton sx={{ p: 0 }}>
        <KeyboardArrowRightIcon />
      </IconButton>
      <Typography variant="h5" color={"secondary.main"} sx={{ pt: "4px", pl: 0.5 }}>
        {here}
      </Typography>
    </Box>
  );
};
