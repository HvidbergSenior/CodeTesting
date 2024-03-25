import { useState, useEffect } from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import TextField from "@mui/material/TextField";
import styled from "@mui/system/styled";
import "./number-selector-input.css";

export enum SizeEnum {
  Large,
  Normal,
}

type Props = {
  defaultValue?: number;
  size: SizeEnum;
  onChange: (value: number) => void;
};

export const NumberSelectorInput = (props: Props) => {
  const { defaultValue, size, onChange } = props;
  const formatValue = (v?: number) => {
    return v ? v.toString().replace(".", ",") : "0";
  };
  const font3Char = size === SizeEnum.Normal ? 20 : 40;
  const font4Char = size === SizeEnum.Normal ? 16 : 32;
  const font5Char = size === SizeEnum.Normal ? 14 : 28;
  const font6Char = size === SizeEnum.Normal ? 10 : 20;
  const font7Char = size === SizeEnum.Normal ? 8 : 16;
  const [fontSize, setFontSize] = useState(font3Char);
  const [inputValue, setInputValue] = useState<string>(formatValue(defaultValue));
  const [numericValue, setNumericValue] = useState<number | undefined>(defaultValue);
  const maxValue = 1000000;

  useEffect(() => {
    onChange(numericValue ?? 0);
  }, [numericValue, onChange]);

  useEffect(() => {
    if (!inputValue) {
      setFontSize(font3Char);
      return;
    }

    if (inputValue.length > 6) {
      setFontSize(font7Char);
    } else if (inputValue.length > 5) {
      setFontSize(font6Char);
    } else if (inputValue.length > 4) {
      setFontSize(font5Char);
    } else if (inputValue.length > 3) {
      setFontSize(font4Char);
    } else {
      setFontSize(font3Char);
    }
  }, [font3Char, font4Char, font5Char, font6Char, font7Char, inputValue, setFontSize]);

  const incrementValue = () => {
    setInputValue((previus) => {
      const t = parseFloat(previus);
      const previusValue = t ? t : 0;
      let newValue = previusValue + 1;
      if (newValue > maxValue) {
        newValue = maxValue;
      }
      setNumericValue(newValue);
      return formatValue(newValue);
    });
  };

  const decrementValue = () => {
    setInputValue((previus) => {
      const t = parseFloat(previus);
      const previusValue = t ? t : 0;
      if (previusValue <= 1.0) {
        setNumericValue(0);
        return "0";
      }
      const newValue = previusValue - 1;
      setNumericValue(newValue);
      return formatValue(newValue);
    });
  };

  const ButtonNormalStyled = styled(Button)`
    min-width: 25px;
    font-size: 25px;
    min-height: 30px;
  `;

  const ButtonLargeStyled = styled(Button)`
    min-width: 50px;
    font-size: 50px;
    min-height: 60px;
  `;

  return (
    <Box display="flex">
      {size === SizeEnum.Normal ? (
        <ButtonNormalStyled data-testid="number-selecter-btn-decrement" variant="text" onClick={decrementValue}>
          -
        </ButtonNormalStyled>
      ) : (
        <ButtonLargeStyled data-testid="number-selecter-btn-decrement" variant="text" onClick={decrementValue}>
          -
        </ButtonLargeStyled>
      )}
      <TextField
        data-testid="number-selecter-input"
        variant="standard"
        inputMode={"numeric"}
        onChange={(e) => {
          e.preventDefault();
          // if new value is valid float
          if (/^([\d]*[,.]?[\d]{0,2})$/.test(e.target.value)) {
            let parsed = parseFloat(e.target.value.replaceAll(",", "."));
            if (parsed > maxValue) {
              setNumericValue(maxValue);
              setInputValue(maxValue.toString());
            } else {
              setNumericValue(isNaN(parsed) ? undefined : parsed);
              setInputValue(e.target.value);
            }
          }
        }}
        value={inputValue}
        InputProps={{
          disableUnderline: true,
          sx: {
            width: size === SizeEnum.Normal ? 45 : 90,
            height: size === SizeEnum.Normal ? 20 : 40,
            paddingTop: size === SizeEnum.Normal ? "12px" : "18px",
            fontSize: fontSize,
            "& input": { textAlign: "center", color: "primary.main" },
          },
        }}
      />
      {size === SizeEnum.Normal ? (
        <ButtonNormalStyled data-testid="number-selecter-btn-increment" variant="text" onClick={incrementValue}>
          +
        </ButtonNormalStyled>
      ) : (
        <ButtonLargeStyled data-testid="number-selecter-btn-increment" variant="text" onClick={incrementValue}>
          +
        </ButtonLargeStyled>
      )}
    </Box>
  );
};
