import Button from "@mui/material/Button";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import Dialog from "@mui/material/Dialog";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import TableContainer from "@mui/material/TableContainer";
import Paper from "@mui/material/Paper";
import Typography from "@mui/material/Typography";
import { CompensationResponse } from "api/generatedApi";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import { useTranslation } from "react-i18next";
import { DialogBaseProps } from "shared/dialog/types";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { formatNumberToAmount, formatNumberToPrice } from "utils/formats";

interface Props extends DialogBaseProps {
  compensationPayment: CompensationResponse;
}

export const CompensationPaymentInfoDialog = (props: Props) => {
  const { compensationPayment } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();

  return (
    <Dialog
      maxWidth="sm"
      PaperProps={{
        sx: {
          height: screenSize === ScreenSizeEnum.Mobile ? "100%" : "auto", // mobile is ignored
          margin: screenSize === ScreenSizeEnum.Mobile ? "32px 7px" : "32px",
          width: screenSize === ScreenSizeEnum.Mobile ? "100%" : "calc(100´%-64px)",
        },
      }}
      open={props.isOpen}
    >
      <DialogTitleStyled
        title={t("dashboard.compensationPayments.info.title")}
        description={t("dashboard.compensationPayments.info.periodLabel", {
          periodStart: compensationPayment.startDate,
          periodEnd: compensationPayment.endDate,
        })}
        onClose={props.onClose}
        isOpen={props.isOpen}
        handleIconClose={props.onClose}
      />
      <DialogContent sx={{ p: 3, pr: 5 }}>
        <Typography variant="subtitle2" sx={{ pb: 2 }}>
          {t("dashboard.compensationPayments.info.amountLabel", { amount: formatNumberToPrice(compensationPayment.compensationPaymentDkr) })}
        </Typography>
        <TableContainer component={Paper}>
          <Table>
            <TableHead sx={{ backgroundColor: "primary.light" }}>
              <TableRow>
                <TableCell
                  data-testid="dashboard-compensation-payments-info-participant-table-header-name"
                  sx={{ color: "primary.main", backgroundColor: "primary.light" }}
                >
                  {t("common.name")}
                </TableCell>
                <TableCell
                  data-testid="dashboard-compensation-payments-info-participant-table-header-hours-and-amount"
                  sx={{ color: "primary.main", backgroundColor: "primary.light" }}
                  align="right"
                >
                  {t("dashboard.compensationPayments.participants.table.hoursAndAmount")}
                </TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {compensationPayment.compensationParticipant?.map((participant, index) => {
                return (
                  <TableRow key={index}>
                    <TableCell>
                      <Typography variant="body2">{participant.participantName}</Typography>
                      <Typography variant="caption">{participant.participantEmail}</Typography>
                    </TableCell>
                    <TableCell align="right">
                      <Typography variant="body2">
                        {t("valueLabels.hours", { hours: formatNumberToAmount(participant.closedHoursInPeriod) })}
                      </Typography>
                      <Typography variant="caption">
                        {t("valueLabels.price", { price: formatNumberToPrice(participant.compensationAmountDkr) })}
                      </Typography>
                    </TableCell>
                  </TableRow>
                );
              })}
            </TableBody>
          </Table>
        </TableContainer>
      </DialogContent>
      <DialogActions sx={{ pr: 3 }}>
        <Button variant="contained" sx={{ width: "160px" }} color="primary" onClick={props.onClose}>
          {t("common.close")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
