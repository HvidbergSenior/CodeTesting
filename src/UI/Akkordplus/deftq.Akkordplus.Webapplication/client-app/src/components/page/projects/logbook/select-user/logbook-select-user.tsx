import { useTranslation } from "react-i18next";
import { Fragment } from "react";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import FormControl from "@mui/material/FormControl";
import Select, { SelectChangeEvent } from "@mui/material/Select";
import MenuItem from "@mui/material/MenuItem";

import { GetProjectLogBookQueryResponse, GetProjectResponse } from "api/generatedApi";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { useLogbookRestrictions } from "shared/user-restrictions/use-logbook-restrictions";
import { ProjectLogbookInfo } from "../info/logbook-info";

interface Props {
  project: GetProjectResponse;
  getLogbookUsersData: GetProjectLogBookQueryResponse;
  selectedUserId?: string;
  onSelectUserId: (userId: string) => void;
}

export const ProjectLogbookSelectUser = (props: Props) => {
  const { project, getLogbookUsersData, selectedUserId } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const { canSelectLogbookParticipant } = useLogbookRestrictions(project);

  const changeUser = (event: SelectChangeEvent) => {
    props.onSelectUserId(event.target.value);
  };
  return (
    <Fragment>
      {screenSize !== ScreenSizeEnum.Mobile && (
        <Box sx={{ display: "flex", justifyContent: "end" }}>
          <ProjectLogbookInfo />
        </Box>
      )}
      {canSelectLogbookParticipant() && (
        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            p: 1,
            pb: screenSize === ScreenSizeEnum.Mobile ? 0.5 : 2,
            pr: screenSize === ScreenSizeEnum.Mobile ? 0 : 1,
          }}
        >
          <Typography variant="h6">{t("logbook.select")}</Typography>
          <FormControl sx={{ ml: 2, minWidth: 120, flex: 1 }} size="small">
            <Select value={selectedUserId ?? ""} onChange={changeUser}>
              {getLogbookUsersData?.users?.map((user) => {
                return (
                  <MenuItem key={user.userId} value={user.userId}>
                    {user.name}
                  </MenuItem>
                );
              })}
            </Select>
          </FormControl>
          {screenSize === ScreenSizeEnum.Mobile && <ProjectLogbookInfo />}
        </Box>
      )}
    </Fragment>
  );
};
