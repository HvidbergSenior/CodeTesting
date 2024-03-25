import Box from "@mui/material/Box";
import Chip from "@mui/material/Chip";
import Typography from "@mui/material/Typography";
import { useTranslation } from "react-i18next";
import { costumPalette } from "theme/palette";
import { MappedFolderSupplement } from "../hooks/use-folder-supplements-mapper";

type Props = {
  supplements?: MappedFolderSupplement[];
  showMax?: number;
};

export const FolderSupplementsContent = (props: Props) => {
  const { t } = useTranslation();
  const supplements = props.showMax ? props.supplements?.slice(0, props.showMax) : props.supplements;

  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      {supplements?.map((sup, index) => {
        return (
          <Box sx={{ display: "flex", gap: 0.5 }} key={index}>
            <Chip
              size="small"
              label={t("content.measurements.table.cellValue.supplementSmall")}
              sx={{ bgcolor: sup.inherent ? costumPalette.gray : costumPalette.supplementBg, color: costumPalette.supplementColor, marginRight: 0.5 }}
            />
            <Typography color={sup.inherent ? "grey.100" : "rgba(0, 0, 0, 0.87)"}>{sup.text}</Typography>
          </Box>
        );
      })}
    </Box>
  );
};
