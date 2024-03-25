import LocationCityOutlinedIcon from "@mui/icons-material/LocationCityOutlined";
import MuiBreadcrumbs from "@mui/material/Breadcrumbs";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import { NavLink } from "react-router-dom";
import { routes } from "shared/app-routes";
import useBreadcrumbs from "use-react-router-breadcrumbs";

export const Breadcrumbs = () => {
  const breadcrumbs = useBreadcrumbs(routes);

  return (
    <MuiBreadcrumbs separator=">" maxItems={2}>
      {breadcrumbs.length > 1 &&
        breadcrumbs.map(({ breadcrumb, key }, index) => (
          <NavLink key={key} to={key} style={{ textDecoration: "none" }}>
            <Stack direction="row" alignItems="center" gap={1}>
              {index === 0 && <LocationCityOutlinedIcon />}
              {index !== 0 && <Typography variant="h6">{breadcrumb}</Typography>}
            </Stack>
          </NavLink>
        ))}
    </MuiBreadcrumbs>
  );
};
