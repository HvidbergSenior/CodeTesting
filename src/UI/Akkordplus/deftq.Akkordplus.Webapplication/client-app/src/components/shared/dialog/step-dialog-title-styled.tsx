import type { DialogBaseProps } from "shared/dialog/types";
import Box from "@mui/material/Box";
import MobileStepper from "@mui/material/MobileStepper";
import DialogTitleStyled from "./dialog-title-styled";

interface Props extends DialogBaseProps {
  steps: number;
  activeStep: number;
  title: string | undefined;
  description?: string | undefined;
  handleIconClose?: () => void;
}
const StepDialogTitleStyled = ({ steps, activeStep, title, description, handleIconClose, onClose }: Props) => {
  return (
    <Box sx={{ position: "relative" }}>
      <Box sx={{ position: "absolute", top: 3, left: 0, right: 0, w: "100%", display: "flex", justifyContent: "center", m: 0, pt: 2 }}>
        <MobileStepper variant="dots" steps={steps} activeStep={activeStep} position="static" nextButton={null} backButton={null} />
      </Box>
      <Box sx={{ pt: 2 }}>
        <DialogTitleStyled
          title={title}
          description={description}
          handleIconClose={handleIconClose}
          showDivider={true}
          isOpen={true}
          onClose={onClose}
          bottomPadding={1}
        />
      </Box>
    </Box>
  );
};

export default StepDialogTitleStyled;
