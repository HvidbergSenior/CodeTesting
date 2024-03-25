import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import { useTranslation } from "react-i18next";
import { costumPalette } from "theme/palette";
import { GetProjectResponse } from "api/generatedApi";
import { ProjectNameAndDescriptionContent } from "../../../project-info/project-name-and-description/project-name-and-description-content";
import { PieceworkTypeAndPeriodContent } from "../../../project-info/piecework-type-and-period/piecework-type-and-period-content";
import { ContactInfoContent } from "../../../project-info/contact-info/contact-info-content";

interface Props {
  project: GetProjectResponse;
}

export const PrintSectionProjectInfo = ({ project }: Props) => {
  const { t } = useTranslation();

  return (
    <Box sx={{ display: "flex", flex: 1, flexDirection: "column", p: 0 }}>
      {project.title !== "" && (
        <Typography variant="h6" sx={{ pb: 2 }}>
          {t("dashboard.reports.printReports.projektInfo.infoTitle", { projektName: project.title })}
        </Typography>
      )}
      <Box
        sx={{
          backgroundColor: `${costumPalette.reportDark} !important`,
          borderRadius: "10px 10px 0 0",
          display: "flex",
          gap: 10,
          pt: 12,
          pl: 15,
          pr: 15,
          pb: 5,
          WebkitPrintColorAdjust: "exact",
        }}
      >
        <Box sx={{ flex: 1 }}>
          <Typography variant="h6">{t("dashboard.projectInfo.projectNameAndDescription.title")}</Typography>
          <ProjectNameAndDescriptionContent project={project} />
        </Box>
        <Box sx={{ flex: 1 }}>
          <Typography variant="h6">{t("dashboard.projectInfo.pieceworkTypeAndPeriod.title")}</Typography>
          <PieceworkTypeAndPeriodContent project={project} usePrintlayOut={true} />
        </Box>
        <Box sx={{ flex: 1 }}>
          <Typography variant="h6">{t("dashboard.projectInfo.contactInfo.title")}</Typography>
          <ContactInfoContent project={project} />
        </Box>
      </Box>
      <svg viewBox="0 0 375.70833 52.916666" height="200" width="1420">
        <path
          d="M -0.00434557,-0.00213304 -0.00244249,10.639361 C 0.47672572,20.980115 4.5943478,28.376162 8.2733575,34.511355 12.309108,40.55263 16.388442,45.023331 24.400439,49.109243 c 9.579149,3.878388 15.690033,4.368601 24.620677,2.466317 L 199.218,20.513375 c 10.58205,-1.70602 19.45001,-1.626673 26.44843,0.399943 l 89.71959,29.255494 c 7.15115,2.463754 15.81889,2.420157 26.83639,-1.50828 11.34872,-3.957162 19.1988,-12.380099 25.69603,-22.529888 3.67392,-7.186795 6.97261,-14.972037 7.79781,-24.6314662 V 1.9467312e-5 Z"
          fill={costumPalette.reportDark}
        />
      </svg>
    </Box>
  );
};
