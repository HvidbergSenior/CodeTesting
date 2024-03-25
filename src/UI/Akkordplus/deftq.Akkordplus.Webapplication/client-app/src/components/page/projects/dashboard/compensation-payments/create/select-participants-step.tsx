import { UseFormGetValues, UseFormSetValue, UseFormWatch } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { CompensationPaymentFormData } from "./compensation-payment-form-data";
import { GetCompensationPaymentParticipantResponse, usePostApiProjectsByProjectIdCompensationsParticipantsMutation } from "api/generatedApi";
import { CompensationPaymentParticipantsTable } from "../participants/participants-table";
import { formatDate, formatDateToRequestShortDate, formatNumberToPrice } from "utils/formats";
import { useToast } from "shared/toast/hooks/use-toast";
import { useEffect, useRef, useState } from "react";

interface Props {
  getValue: UseFormGetValues<CompensationPaymentFormData>;
  setValue: UseFormSetValue<CompensationPaymentFormData>;
  watch: UseFormWatch<CompensationPaymentFormData>;
  projectId: string;
  onStepIsValid: (isValid: boolean) => void;
}

export function CompensationPaymentUsersSelectorStep(props: Props) {
  const { projectId, getValue, setValue, watch, onStepIsValid } = props;
  const { t } = useTranslation();
  const tRef = useRef(t);
  const toast = useToast();
  const toastRef = useRef(toast);
  const [participants, setParticipants] = useState<GetCompensationPaymentParticipantResponse[]>([]);

  const [getParticipants] = usePostApiProjectsByProjectIdCompensationsParticipantsMutation();
  useEffect(() => {
    getParticipants({
      projectId: projectId,
      getCompensationPaymentParticipantsInPeriodRequest: {
        amount: getValue("amount"),
        startDate: formatDateToRequestShortDate(getValue("periodDateStart")),
        endDate: formatDateToRequestShortDate(getValue("periodDateEnd")),
      },
    })
      .unwrap()
      .then((response) => {
        setParticipants(response.participants ?? []);
      })
      .catch((error) => {
        console.error(error);
        toastRef.current.error(tRef.current("dashboard.compensationPayments.create.step2Users.getParticipantsError"));
      });
  }, [getParticipants, setParticipants, toastRef, tRef, projectId, getValue]);

  useEffect(() => {
    const subscription = watch((value, { name, type }) => {
      if (name === "selectedUserIds") {
        onStepIsValid(!!value.selectedUserIds && value.selectedUserIds.length > 0);
      }
    });

    return () => subscription.unsubscribe();
  }, [watch, onStepIsValid]);

  const userIdsSelected = (ids: string[]) => {
    setValue("selectedUserIds", ids);
  };

  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
      <Typography variant="subtitle2">
        {t("dashboard.compensationPayments.info.amountLabel", { amount: formatNumberToPrice(getValue("amount")) })}
      </Typography>
      <Typography variant="subtitle2">
        {t("dashboard.compensationPayments.info.periodLabel", {
          periodStart: formatDate(getValue("periodDateStart")),
          periodEnd: formatDate(getValue("periodDateEnd")),
        })}
      </Typography>
      <Typography variant="caption" color="grey.100">
        {t("dashboard.compensationPayments.create.step2Users.periodDescription")}
      </Typography>
      <CompensationPaymentParticipantsTable participants={participants} onSelectedIds={userIdsSelected} />
    </Box>
  );
}
