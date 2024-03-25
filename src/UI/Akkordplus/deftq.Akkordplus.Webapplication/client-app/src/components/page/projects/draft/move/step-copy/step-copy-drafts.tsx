import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
import CircularProgress from "@mui/material/CircularProgress";
import CheckIcon from "@mui/icons-material/Check";
import ErrorOutlineOutlinedIcon from "@mui/icons-material/ErrorOutlineOutlined";
import { DraftWorkItemRequest } from "../hooks/use-map-drafts";

interface Props {
  draftRequests: DraftWorkItemRequest[];
}

export const StepCopyDrafts = (props: Props) => {
  const { draftRequests } = props;

  return (
    <Table>
      <TableBody>
        {draftRequests?.map((request) => (
          <TableRow key={request.draftId}>
            <TableCell sx={{ width: 30 }}>
              {request.requestFailed && <ErrorOutlineOutlinedIcon color="error" sx={{ mt: 0.5 }} />}
              {request.requestSuccess && <CheckIcon color="success" />}
              {!request.requestFailed && !request.requestSuccess && <CircularProgress size={18} sx={{ mt: 0.5 }} />}
            </TableCell>
            <TableCell>{request.text}</TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  );
};
