import styled from "@mui/system/styled";
import Typography from "@mui/material/Typography";
import TableCell from "@mui/material/TableCell";

export const TableCellStyled = styled(TableCell)`
  white-space: nowrap;
  text-overflow: ellipsis;
  overflow: hidden;
`;

export const TypographyStyled = styled(Typography)`
  white-space: nowrap;
  text-overflow: ellipsis;
  overflow: hidden;
`;

export const Typography2LinesStyled = styled(Typography)`
  white-space: nowrap;
  text-overflow: ellipsis;
  overflow: hidden;

  @supports (-webkit-line-clamp: 2) {
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: initial;
    display: -webkit-box;
    -webkit-line-clamp: 2;
    -webkit-box-orient: vertical;
  }
`;
