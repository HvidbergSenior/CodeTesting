import { TextField } from "@mui/material";
import { ChangeEvent, useEffect, useState } from "react";

export interface Props {
  minCharsForSearch: number;
  label: string;
  searchProps: (value: string) => void;
}

export function SearchInput(props: Props) {
  const { minCharsForSearch, label, searchProps } = props;
  const [searchValue, setSearchValue] = useState("");
  const [oldSearchValue, setOldSearchValue] = useState("");

  useEffect(() => {
    const search = async () => {
      searchProps(searchValue);
    };

    let timerId: any;
    if (searchValue !== oldSearchValue) {
      timerId = setTimeout(() => {
        setOldSearchValue(searchValue);
        search();
      }, 400);
    }

    return () => {
      clearTimeout(timerId);
    };
  }, [searchProps, searchValue, oldSearchValue]);

  const handleSearchChanges = (event: ChangeEvent<HTMLInputElement>) => {
    const value = event?.target?.value;
    if (value && value.length < minCharsForSearch) {
      setSearchValue("");
    } else {
      setSearchValue(value);
    }
  };

  return <TextField data-testid="search-input" fullWidth variant="filled" label={label} onChange={handleSearchChanges} />;
}
