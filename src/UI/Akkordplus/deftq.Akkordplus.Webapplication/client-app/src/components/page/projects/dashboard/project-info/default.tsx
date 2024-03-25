import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import { GetProjectResponse } from "api/generatedApi";
import { BaseRateAndSupplements } from "components/page/folder/content/calculation/base-rate/base-rate";
import { PaymentFactor } from "components/page/folder/content/calculation/payment-factor/payment-factor";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useTranslation } from "react-i18next";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { DefaultBack2DashBoardNavigation } from "../navigation/default";
import { PieceworkTypeAndPeriod } from "./piecework-type-and-period/piecework-type-and-period";
import { ContactInfo } from "./contact-info/contact-info";
import { ExtraWorkAgreement } from "./customer-company/customer-company";
import { ProjectNameAndDescription } from "./project-name-and-description/project-name-and-description";

interface Props {
  project: GetProjectResponse;
  selectedFolder: ExtendedProjectFolder;
}

export const DefaultProjectInfo = (props: Props) => {
  const { project, selectedFolder } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const maxWidth = screenSize === ScreenSizeEnum.LargeDesktop ? "1200px" : "900px";
  const spacing = 3;

  return (
    <Box sx={{ p: 0, pb: 1, position: "relative", flex: 1, overflowY: "hidden", display: "flex", flexDirection: "column", height: "100%" }}>
      <DefaultBack2DashBoardNavigation here={t("dashboard.links.projectInfo")} />
      <Box sx={{ p: 2, overflow: "auto", height: "100%" }}>
        <Grid container alignItems={"center"} maxWidth={maxWidth} marginLeft={"auto"} marginRight={"auto"} rowSpacing={spacing} overflow={"auto"}>
          <Grid container item xs={12} spacing={spacing} pb={9}>
            <Grid container alignItems={"center"} item spacing={spacing}>
              <ProjectNameAndDescription project={project} />
              <PieceworkTypeAndPeriod project={project} />
              <ContactInfo project={project} />
              <BaseRateAndSupplements folder={selectedFolder} project={project} />
              <ExtraWorkAgreement project={project} />
              <PaymentFactor folder={selectedFolder} project={project} />
            </Grid>
          </Grid>
        </Grid>
      </Box>
    </Box>
  );
};
