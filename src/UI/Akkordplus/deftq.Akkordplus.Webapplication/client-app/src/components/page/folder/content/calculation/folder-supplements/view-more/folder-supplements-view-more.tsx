import { Button, DialogActions, DialogContent } from "@mui/material";
import Dialog from "@mui/material/Dialog";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import { useTranslation } from "react-i18next";
import { DialogBaseProps } from "shared/dialog/types";
import { FolderSupplementsContent } from "../content/folder-supplements-content";
import { useFolderSupplementsMapper } from "../hooks/use-folder-supplements-mapper";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";

interface Props extends DialogBaseProps {
  folder: ExtendedProjectFolder;
}

export const FolderSupplementsViewMore = (props: Props) => {
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const mapper = useFolderSupplementsMapper({ folder: props.folder });
  const mappedSupplements = mapper();

  return (
    <Dialog
      maxWidth="sm"
      PaperProps={{
        sx: {
          height: screenSize === ScreenSizeEnum.Mobile ? "100%" : "750px", // mobile is ignored
          margin: screenSize === ScreenSizeEnum.Mobile ? "32px 7px" : "32px",
          width: screenSize === ScreenSizeEnum.Mobile ? "100%" : "calc(100´%-64px)",
        },
      }}
      open={props.isOpen}
    >
      <DialogTitleStyled
        title={t("content.calculation.folderSupplements.title")}
        description={t("content.calculation.folderSupplements.description")}
        onClose={props.onClose}
        isOpen={props.isOpen}
        handleIconClose={props.onClose}
      />
      <DialogContent sx={{ p: 3, pr: 5 }}>
        <FolderSupplementsContent supplements={mappedSupplements} />
      </DialogContent>
      <DialogActions sx={{ pr: 3 }}>
        <Button variant="contained" sx={{ width: "160px" }} color="primary" onClick={props.onClose}>
          {t("common.close")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
