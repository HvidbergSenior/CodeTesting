import { useTranslation } from "react-i18next";
import Typography from "@mui/material/Typography";
import { InputContainer } from "shared/styles/input-container-style";
import { getHmsFromMilliSeconds } from "utils/formats";

interface Props {
  label: string;
  timeMs?: number;
}

export const ProjectSpecicifOperationTimeInfo = (props: Props) => {
  const { label, timeMs } = props;
  const { t } = useTranslation();

  return (
    <InputContainer sx={{ height: 1, maxWidth: 400 }}>
      <Typography variant="caption" color={"primary.main"}>
        {t(label)}
      </Typography>
      <Typography variant="body1">{getHmsFromMilliSeconds(timeMs)}</Typography>
    </InputContainer>
  );
};
