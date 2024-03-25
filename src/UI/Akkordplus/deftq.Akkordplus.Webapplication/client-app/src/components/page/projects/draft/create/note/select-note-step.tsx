import { TextField } from "@mui/material";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { FormDataWorkItemDraft } from "components/page/folder/content/measurements/create/create-work-item-dialog/create-work-item-form-data";
import { ChangeEvent } from "react";
import { UseFormGetValues, UseFormSetValue } from "react-hook-form";
import { useTranslation } from "react-i18next";

export interface Props {
  setValue: UseFormSetValue<FormDataWorkItemDraft>;
  getValue: UseFormGetValues<FormDataWorkItemDraft>;
}

export function SelectNoteStep(props: Props) {
  const { setValue, getValue } = props;
  const { t } = useTranslation();

  const handleOnChange = (event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setValue("note", event.target.value);
  };

  const inputProps = {
    maxLength: 60,
  };

  return (
    <Box sx={{ display: "flex", flexDirection: "column", minHeight: "340px", padding: "20px" }}>
      <Typography variant="caption" color={"primary.main"}>
        {t("captions.noteOptional")}
      </Typography>
      <Box sx={{ pt: 1, pb: 1 }}>
        <TextField
          onChange={(event) => handleOnChange(event)}
          variant="filled"
          sx={{ width: "100%" }}
          defaultValue={getValue("note")}
          label={t("project.offline.create.note.noteLabel")}
          multiline
          minRows={2}
          inputProps={inputProps}
        />
      </Box>
      <Typography variant="caption">{t("project.offline.create.note.maxChar")}</Typography>
    </Box>
  );
}
