import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import { GetProjectSpecificOperationResponse } from "api/generatedApi";
import { InputContainer } from "shared/styles/input-container-style";
import type { DialogBaseProps } from "shared/dialog/types";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import { ProjectSpecicifOperationTimeInfo } from "./inputs/time-info";
import { ProjectSpecicifOperationPaymentInfo } from "./inputs/payment-info";

interface Props extends DialogBaseProps {
  operation: GetProjectSpecificOperationResponse;
}

export function InfoProjectSpecificOperationDialog(props: Props) {
  const { operation } = props;
  const { t } = useTranslation();
  const showTimeType = !!operation.workingTimeMs && operation.workingTimeMs > 0 ? "workingTime" : "operationTime";
  const { screenSize } = useScreenSize();

  return (
    <Dialog
      open={props.isOpen}
      PaperProps={{
        sx: {
          height: screenSize === ScreenSizeEnum.Mobile ? "100%" : "auto", // mobile is ignored
          margin: screenSize === ScreenSizeEnum.Mobile ? "32px 7px" : "32px",
          width: screenSize === ScreenSizeEnum.Mobile ? "100%" : "450px",
        },
      }}
    >
      <DialogTitleStyled title={t("dashboard.projectSpecificOperations.update.info.title")} onClose={props.onClose} isOpen={props.isOpen} />
      <DialogContent>
        <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
          {operation.extraWorkAgreementNumber !== "" && (
            <InputContainer>
              <Typography variant="caption" color={"primary.main"}>
                {t("dashboard.projectSpecificOperations.update.input.number.label")}
              </Typography>

              <Typography variant="body1">{operation?.extraWorkAgreementNumber}</Typography>
            </InputContainer>
          )}
          <InputContainer>
            <Typography variant="caption" color={"primary.main"}>
              {t("common.name")}
            </Typography>
            <Typography variant="body1">{operation?.name}</Typography>
          </InputContainer>
          <InputContainer>
            <Typography variant="caption" color={"primary.main"}>
              {t("common.note")}
            </Typography>
            <Typography variant="body1">{operation?.description}</Typography>
          </InputContainer>
          {showTimeType === "operationTime" && (
            <ProjectSpecicifOperationTimeInfo label={"dashboard.projectSpecificOperations.types.operationTime"} timeMs={operation.operationTimeMs} />
          )}
          {showTimeType === "workingTime" && (
            <InputContainer>
              <ProjectSpecicifOperationTimeInfo label={"dashboard.projectSpecificOperations.types.workingTime"} timeMs={operation.workingTimeMs} />
            </InputContainer>
          )}

          <ProjectSpecicifOperationPaymentInfo operation={operation} />
        </Box>
      </DialogContent>
      <DialogActions>
        <Button data-testid="update-project-specific-operation-action-btn-close" variant="contained" color="primary" onClick={props.onClose}>
          {t("common.close")}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
