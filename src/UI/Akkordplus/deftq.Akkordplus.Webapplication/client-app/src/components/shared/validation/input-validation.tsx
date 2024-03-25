export const useInputValidation = () => {
  // allows input between 0-100 with up to 2 decimals
  const regexPercent = new RegExp(/^(\d{0,2}(?:,\d{1,2})?|100?)$/);
  // allows input between 0-100 with up to 2 decimals with use for checking during onChange
  const regexPercentOnChange = new RegExp(/^(?:\d|[0-9]\d|100)(?:,0?\d{0,1}|,\d{0,2})?$/);
  // allows all number inputs with two decimals
  const regexAllPricesWithTwoDecimals = new RegExp(/^([\d.]+(?:[,]\d{0,2})?)$/);
  const regexAllPricesWithTwoDecimalsAndNegative = new RegExp(/^(-?[\d.]+(?:[,]\d{0,2})?)$/);
  // Allows only numbers
  const regexOnlyNumbers = new RegExp(/^[0-9\b]+$/);

  const validateMultipleInputPercentWithAndStatement = (inputs: string[]) => {
    return inputs.every((input) => regexPercent.test(input));
  };

  const validateDoesNotIncludeEmptyString = (inputs: string[]) => {
    const doesNotIncludeEmptyString = inputs.every((input) => input.length !== 0);
    return doesNotIncludeEmptyString;
  };

  const validateOnChangeInputPercent = (input: string) => {
    return regexPercentOnChange.test(input);
  };

  const validateSum = (input: string, maxValue: number, minValue?: number) => {
    // if min value is set, check with allowed negative values, min and max
    if (minValue && minValue < 0) {
      if (input === "-") {
        return true;
      }
      if (regexAllPricesWithTwoDecimalsAndNegative.test(input)) {
        const val = parseFloat(input.replace(/[.\s]/g, ""));
        return val >= minValue && val <= maxValue;
      }
      // regex test failed
      return false;
    }
    return regexAllPricesWithTwoDecimals.test(input) && parseFloat(input.replace(/[.\s]/g, "")) <= maxValue;
  };

  const validateEndsWithCommaOrStartsWithComma = (input: string) => {
    if (input.charAt(input.length - 1) === "," || input.charAt(0) === ",") {
      return false;
    }
    return true;
  };

  const validateInteger = (input: string, minValue?: number, maxValue?: number): boolean => {
    if (!regexOnlyNumbers.test(input)) {
      return false;
    }
    const value = parseInt(input);
    if (minValue && value < minValue) {
      return false;
    }
    if (maxValue && value > maxValue) {
      return false;
    }
    return true;
  };

  return {
    validateMultipleInputPercentWithAndStatement,
    validateOnChangeInputPercent,
    validateSum,
    validateEndsWithCommaOrStartsWithComma,
    validateInteger,
    validateDoesNotIncludeEmptyString,
  };
};
