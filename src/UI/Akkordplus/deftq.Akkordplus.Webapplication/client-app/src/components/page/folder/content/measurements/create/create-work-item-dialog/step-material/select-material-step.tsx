import { useState } from "react";
import { UseFormGetValues, UseFormSetValue } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Grid from "@mui/material/Grid";
import styled from "@mui/system/styled";
import Typography from "@mui/material/Typography";
import CircularProgress from "@mui/material/CircularProgress";
import { FoundMaterial, usePostApiCatalogMaterialsSearchMutation } from "api/generatedApi";
import { SelectMaterialTable } from "./select-material-table";
import { useToast } from "shared/toast/hooks/use-toast";
import { SearchInput } from "components/shared/search-input/search-input";
import { FormDataWorkItem } from "../create-work-item-form-data";

export interface Props {
  setValue: UseFormSetValue<FormDataWorkItem>;
  getValue: UseFormGetValues<FormDataWorkItem>;
  validateError?: string;
}

export function SelectMaterialStep(props: Props) {
  const { setValue, getValue, validateError } = props;
  const minCharsForSearch = 3;
  const { t } = useTranslation();
  const toast = useToast();
  const [preselectedMaterial, setPreselectedMaterial] = useState(getValue("material"));
  const [searchMaterials] = usePostApiCatalogMaterialsSearchMutation();
  const [searchResults, setSearchResults] = useState<FoundMaterial[] | undefined | null>(preselectedMaterial ? [preselectedMaterial] : undefined);
  const [searchParam, setSearchParam] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const selectedMaterial = (material: FoundMaterial) => {
    setPreselectedMaterial(material);
    setValue("material", material);
  };

  const searchMaterial = (value: string) => {
    if (value.length < minCharsForSearch && preselectedMaterial) {
      return;
    }

    if (value.length < minCharsForSearch) {
      setSearchParam("");
      setSearchResults(undefined);
      return;
    }

    if (value === searchParam) {
      return;
    }

    setIsLoading(true);
    setSearchParam(value);
    setSearchResults(undefined);
    searchMaterials({ searchMaterialsRequest: { searchString: value, maxHits: 100 } })
      .unwrap()
      .then((response) => {
        setSearchResults(response.foundMaterials);
        setIsLoading(false);
      })
      .catch((error) => {
        console.error(error);
        toast.error(t("content.measurements.create.selectMaterialStep.searchError"));
        setSearchResults(undefined);
        setIsLoading(false);
      });
  };

  const NoMaterialsSelectedTypographyStyled = styled(Typography)`
    font-style: italic;
  `;

  return (
    <Grid container>
      <Grid item xs={12}>
        <SearchInput
          searchProps={searchMaterial}
          minCharsForSearch={minCharsForSearch}
          label={t("content.measurements.create.selectMaterialStep.search")}
        />
      </Grid>
      <Grid item xs={12} minHeight={30}>
        {searchParam?.length === 0 && (
          <Typography color={"primary.main"}>
            {t("content.measurements.create.selectMaterialStep.searchMinChars", { minchars: minCharsForSearch })}
          </Typography>
        )}
        {searchParam?.length > 0 && searchResults && searchResults.length > 0 && (
          <Typography variant="caption" color={"primary.main"}>
            {t("content.measurements.create.selectMaterialStep.searchResultShow", { shown: searchResults?.length })}
          </Typography>
        )}
      </Grid>
      {validateError && (
        <Grid item xs={12} color={"error.main"} textAlign={"center"}>
          {validateError}
        </Grid>
      )}
      <Grid item xs={12} minHeight={220}>
        {searchResults && searchResults.length > 0 && (
          <Grid item xs={12} sx={{ mt: 1 }}>
            <SelectMaterialTable searchResults={searchResults} preselected={preselectedMaterial} selectedMaterialProps={selectedMaterial} />
          </Grid>
        )}
        {searchResults && searchResults.length === 0 && (
          <Grid item xs={12} sx={{ mt: 5, textAlign: "center" }}>
            <NoMaterialsSelectedTypographyStyled>{t("content.measurements.create.selectMaterialStep.noResults")}</NoMaterialsSelectedTypographyStyled>
          </Grid>
        )}
        {isLoading && (
          <Grid item xs={12} sx={{ mt: 5, textAlign: "center" }}>
            <CircularProgress size={100} />
          </Grid>
        )}
      </Grid>
    </Grid>
  );
}
