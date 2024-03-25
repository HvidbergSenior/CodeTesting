import { useState } from "react";
import { UseFormGetValues, UseFormSetValue } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Grid from "@mui/material/Grid";
import styled from "@mui/system/styled";
import Typography from "@mui/material/Typography";
import CircularProgress from "@mui/material/CircularProgress";
import { FoundOperation, usePostApiCatalogOperationsSearchMutation } from "api/generatedApi";
import { SelectOperationTable } from "./step-operation-table";
import { useToast } from "shared/toast/hooks/use-toast";
import { SearchInput } from "components/shared/search-input/search-input";
import { FormDataWorkItem } from "../create-work-item-form-data";

export interface Props {
  setValue: UseFormSetValue<FormDataWorkItem>;
  getValue: UseFormGetValues<FormDataWorkItem>;
  validateError?: string;
}

export function SelectOperationStep(props: Props) {
  const { setValue, getValue, validateError } = props;
  const minCharsForSearch = 3;
  const { t } = useTranslation();
  const toast = useToast();
  const [preselectedOperation, setPreselectedOperation] = useState(getValue("operation"));
  const [searchOperations] = usePostApiCatalogOperationsSearchMutation();
  const [searchResults, setSearchResults] = useState<FoundOperation[] | undefined | null>(preselectedOperation ? [preselectedOperation] : undefined);
  const [searchParam, setSearchParam] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const selectedOperation = (operation: FoundOperation) => {
    setPreselectedOperation(operation);
    setValue("operation", operation);
  };

  const searchOperation = (value: string) => {
    if (value.length < minCharsForSearch && preselectedOperation) {
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
    searchOperations({ searchOperationsRequest: { searchString: value, maxHits: 100 } })
      .unwrap()
      .then((response) => {
        setSearchResults(response.foundOperations);
        setIsLoading(false);
      })
      .catch((error) => {
        console.error(error);
        toast.error(t("content.measurements.create.selectOperationStep.searchError"));
        setSearchResults(undefined);
        setIsLoading(false);
      });
  };

  const NoOperationsSelectedTypographyStyled = styled(Typography)`
    font-style: italic;
  `;

  return (
    <Grid container>
      <Grid item xs={12}>
        <SearchInput
          searchProps={searchOperation}
          minCharsForSearch={minCharsForSearch}
          label={t("content.measurements.create.selectOperationStep.search")}
        />
      </Grid>
      <Grid item xs={12} minHeight={30}>
        {searchParam?.length === 0 && (
          <Typography variant="caption">
            {t("content.measurements.create.selectOperationStep.searchMinChars", { minchars: minCharsForSearch })}
          </Typography>
        )}
        {searchParam?.length > 0 && searchResults && searchResults.length > 0 && (
          <Typography variant="caption">
            {t("content.measurements.create.selectOperationStep.searchResultShow", { shown: searchResults?.length })}
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
            <SelectOperationTable searchResults={searchResults} preselected={preselectedOperation} selectedOperationProps={selectedOperation} />
          </Grid>
        )}
        {searchResults && searchResults.length === 0 && (
          <Grid item xs={12} sx={{ mt: 5, textAlign: "center" }}>
            <NoOperationsSelectedTypographyStyled>
              {t("content.measurements.create.selectOperationStep.noResults")}
            </NoOperationsSelectedTypographyStyled>
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
