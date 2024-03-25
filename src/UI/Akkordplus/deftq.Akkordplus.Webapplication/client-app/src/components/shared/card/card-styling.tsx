import styled from "@mui/system/styled";
import Card from "@mui/material/Card";
import CardActions from "@mui/material/CardActions";
import CardContent from "@mui/material/CardContent";
import CardHeader from "@mui/material/CardHeader";
import Typography from "@mui/material/Typography";

const CardStyled = styled(Card)`
  display: flex;
  flex-direction: column;
`;

const CardHeaderStyled = styled(CardHeader)`
  height: 45px !important;
  > div {
    display: inherit !important;
    padding-right: 2px !important;
    > button {
      margin-top: -14px;
    }
  }
`;

const CardContentStyled = styled(CardContent)`
  align-content: center;
  overflow: hidden;
  flex-grow: 1;
  display: flex;
  flex-direction: column;
`;

const CardActionsRightStyled = styled(CardActions)`
  justify-content: right;
  padding-bottom: 15px !important;
  padding-right: 40px;
  height: 45px !important;
`;

const CardActionsCenterStyled = styled(CardActions)`
  justify-content: center;
  padding-bottom: 35px !important;
  height: 45px !important;
`;

const NothingToShowTypographyStyled = styled(Typography)`
  height: 90%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-style: italic;
  text-align: center;
`;

export { NothingToShowTypographyStyled, CardActionsRightStyled, CardActionsCenterStyled, CardContentStyled, CardHeaderStyled, CardStyled };
