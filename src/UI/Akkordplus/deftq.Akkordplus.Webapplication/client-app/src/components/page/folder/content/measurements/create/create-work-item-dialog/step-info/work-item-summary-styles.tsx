import Card from "@mui/material/Card";
import Divider from "@mui/material/Divider";
import styled from "@mui/system/styled";
import Typography from "@mui/material/Typography";

export const TypographyValueLineStyled = styled(Typography)`
  margin-bottom: 10px;
`;

export const CardNoBorderStyled = styled(Card)`
  padding: 10px 20px;
  border: 0;
`;

export const CardStyled = styled(Card)`
  padding: 10px 20px;
  border: 1;
`;

export const DividerStyled = styled(Divider)`
  margin-left: calc(50% - 120px);
  margin-right: calc(50% - 120px);
  border-width: 3px;
  border-radius: 5px;
`;
