import { useTranslation } from "react-i18next";
import { Control, Controller } from "react-hook-form";
import Typography from "@mui/material/Typography";
import FormControl from "@mui/material/FormControl";
import FormControlLabel from "@mui/material/FormControlLabel";
import Radio from "@mui/material/Radio";
import RadioGroup from "@mui/material/RadioGroup";
import { InputContainer } from "shared/styles/input-container-style";
import { ExtraWorkAgreementResponse } from "api/generatedApi";
import { ExtraWorkAgreementFormData } from "../hooks/use-input-validation";
import { useExtraWorkAgreementFormatter } from "../../hooks/format";

interface Props {
  control: Control<ExtraWorkAgreementFormData>;
  disabled: boolean;
  agreement?: ExtraWorkAgreementResponse;
}

export const EditExtraWorkAgreementSelectType = (props: Props) => {
  const { t } = useTranslation();
  const { getTypeFormatted, getTypeByAgreementFormatted } = useExtraWorkAgreementFormatter();

  return (
    <InputContainer>
      <Typography variant="caption" color={"primary.main"}>
        {props.disabled ? t("dashboard.extraWorkAgreements.create.type.noSelectLabel") : t("captions.typeRequired")}
      </Typography>
      {props.disabled ? (
        <Typography variant="body1">{getTypeByAgreementFormatted(props.agreement)}</Typography>
      ) : (
        <FormControl>
          <Controller
            rules={{ required: true }}
            control={props.control}
            name="extraWorkAgreementType"
            render={({ field }) => (
              <RadioGroup {...field}>
                <FormControlLabel
                  data-testid="create-project-extra-work-agreement-customer-hours"
                  value={"CustomerHours"}
                  control={<Radio />}
                  label={getTypeFormatted("CustomerHours")}
                />
                <FormControlLabel
                  data-testid="create-project-extra-work-agreement-company-hours"
                  value={"CompanyHours"}
                  control={<Radio />}
                  label={getTypeFormatted("CompanyHours")}
                />
                <FormControlLabel
                  data-testid="create-project-extra-work-agreement-agreed-payment"
                  value={"AgreedPayment"}
                  control={<Radio />}
                  label={getTypeFormatted("AgreedPayment")}
                />
                <FormControlLabel
                  data-testid="create-project-extra-work-agreement-other"
                  value={"Other"}
                  control={<Radio />}
                  label={getTypeFormatted("Other")}
                />
              </RadioGroup>
            )}
          />
        </FormControl>
      )}
    </InputContainer>
  );
};
