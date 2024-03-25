import { useState, Fragment } from "react";
import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";
import { UseFormGetValues } from "react-hook-form";
import { WorkItemSummary } from "./work-item-summary";
import { FormDataWorkItem } from "../create-work-item-form-data";
export interface Prop {
  getValue: UseFormGetValues<FormDataWorkItem>;
}

export function WorkItemInfoStep(props: Prop) {
  const { getValue } = props;
  const [workItem] = useState(getValue("workItem"));

  return (
    <Fragment>
      {!workItem && (
        <Box sx={{ mt: 5, textAlign: "center" }}>
          <CircularProgress size={100} />
        </Box>
      )}
      {workItem && <WorkItemSummary workItem={workItem} />}
    </Fragment>
  );
}
