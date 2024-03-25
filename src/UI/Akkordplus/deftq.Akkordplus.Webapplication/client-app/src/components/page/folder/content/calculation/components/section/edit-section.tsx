import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { SwitchControl } from "components/page/folder/content/calculation/components/toggle/toggle";
import { InputFieldCommon } from "../../../../../../shared/number-input/input-common";
import { SectionProps } from "../../base-rate/edit/edit-base";
import { useState, useEffect } from "react";

interface Props {
  section: SectionProps;
  isInRootFolder?: boolean;
}

export const EditSection = ({ section, isInRootFolder }: Props) => {
  const opacity = section.inputFieldProps.alwaysDisableInputField || section.switchProps?.value ? "1" : "0.5";
  const [background, setBackground] = useState<string>("primary.light");

  useEffect(() => {
    if (section.inputFieldProps.alwaysDisableInputField) {
      setBackground("transparent");
      return;
    }

    if (!section.inputFieldProps.alwaysDisableInputField && !section.switchProps) {
      setBackground("transparent");
      return;
    }
    if (section.switchProps?.value) {
      setBackground("transparent");
      return;
    }
    if (!section.switchProps?.value) {
      setBackground("primary.light");
      return;
    }
  }, [section.switchProps, section.inputFieldProps.alwaysDisableInputField]);

  return (
    <Box display={"flex"} flexDirection={"column"} pb={3} pt={3}>
      {section.switchProps && (
        <Box pl={5} pb={2}>
          <SwitchControl switchProps={section.switchProps} isInRootFolder={isInRootFolder} />
        </Box>
      )}
      <Box display={"flex"} flexDirection={"row"} sx={{ backgroundColor: background }} p={1}>
        <Box display={"flex"} flexDirection={"column"}>
          <Box pl={4}>
            <Typography width={"200px"} pb={1} variant="caption" color={"primary.main"}>
              {section.title}
            </Typography>
            <InputFieldCommon inputFieldProps={section.inputFieldProps} />
          </Box>
        </Box>
        {section.descriptions.length > 0 && (
          <Box pl={2} pt={"30px"} display={"flex"} flexDirection={"column"} justifyContent={"center"} sx={{ opacity: opacity }}>
            {section.descriptions.map((description) => (
              <Typography key={description} pt={"2px"} sx={{ color: "primary.main" }} variant="body2">
                {description}
              </Typography>
            ))}
          </Box>
        )}
      </Box>
    </Box>
  );
};
