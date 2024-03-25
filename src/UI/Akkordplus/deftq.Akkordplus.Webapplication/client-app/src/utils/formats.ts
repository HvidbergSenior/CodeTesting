import i18next from "i18next";

type dateStyle = "long" | "full" | "medium" | "short" | undefined;

const timestampToDate = (timestamp: string): Date => {
  let date: Date;
  date = new Date(timestamp);

  if (date.toString() === "Invalid Date") {
    // invalid date, expect this timestamp to have format dd-mm-yyyyThh.mm.ss.mmm+00:00
    const timestampSplit = timestamp.split("T");
    const dateSplit = timestampSplit[0].split("-");
    const wholeTime = timestampSplit[1].split(".");
    const timeSplit = wholeTime[0].split(":");
    date = new Date(Number.parseInt(dateSplit[2]), Number.parseInt(dateSplit[1]) - 1, Number.parseInt(dateSplit[0]));
    date.setHours(Number.parseInt(timeSplit[0]));
    date.setMinutes(Number.parseInt(timeSplit[1]));
    date.setSeconds(Number.parseInt(timeSplit[2]));
  }

  return date;
};

export const shortDateToDate = (dateString: string): Date => {
  let date: Date;
  date = new Date(dateString);

  if (date.toString() === "Invalid Date") {
    // invalid date, expect this timestamp to have format dd-mm-yyyy
    const dateSplit = dateString.split("-");
    date = new Date(Number.parseInt(dateSplit[2]), Number.parseInt(dateSplit[1]) - 1, Number.parseInt(dateSplit[0]));
  }

  return date;
}

export const formatHoursAndMinutes = (hours?: number | null, minutes?: number | null): string => {
  if (!hours && !minutes) {
    return "";
  }
  return `${hours ?? 0}t ${padNumber(minutes ?? 0, 2)}m`;
};

export const formatDate = (date?: Date, includeTime?: boolean): string => {
  if (!date) {
    return "";
  }
  if (includeTime) {
    return Intl.DateTimeFormat(i18next.language, {
      dateStyle: "long",
      timeStyle: "short",
    }).format(new Date(date));
  }

  return Intl.DateTimeFormat(i18next.language, { dateStyle: "long" }).format(new Date(date));
};

export const formatDateToRequestShortDate = (date?: Date): string => {
  if (!date) {
    return "01-01-0001";
  }
  const d = padNumber(date.getDate(), 2);
  const m = padNumber(date.getMonth() + 1, 2);
  const y = date.getFullYear();
  return `${d}-${m}-${y}`

}

export const formatDateToSortingShortDate = (date?: Date): string => {
  if (!date) {
    return "0001-01-01";
  }
  const d = padNumber(date.getDate(), 2);
  const m = padNumber(date.getMonth() + 1, 2);
  const y = date.getFullYear();
  return `${y}-${m}-${d}`

}

export const formatNewDate = (includeTime?: boolean): string => {
  if (includeTime) {
    return Intl.DateTimeFormat(i18next.language, {
      dateStyle: "long",
      timeStyle: "short",
    }).format(new Date());
  }

  return Intl.DateTimeFormat(i18next.language, { dateStyle: "long" }).format(new Date());
};

export const formatTimestamp = (timestamp: string | undefined, includeTime: boolean = false, style?: dateStyle): string => {
  if (!timestamp) return "undefined";
  const date = timestampToDate(timestamp);

  if (includeTime) {
    return Intl.DateTimeFormat(i18next.language, {
      dateStyle: style ?? "long",
      timeStyle: "short",
    }).format(date);
  }

  return Intl.DateTimeFormat(i18next.language, { dateStyle: style ?? "long" }).format(date);
};

export const getDateFromDateOnly = (date?: string): Date | undefined => {
  if (!date || date === "01-01-0001") {
    return undefined;
  }
  return shortDateToDate(date);
}

export const getShortDayFromTimestamp = (timestamp: string): string => {
  if (!timestamp) return "";
  const date = timestampToDate(timestamp);

  return Intl.DateTimeFormat(i18next.language, { weekday: "short" }).format(date);
};

export const formatTimestampToDate = (timestamp: string | undefined): Date => {
  if (!timestamp) return new Date();
  const date = timestampToDate(timestamp);
  return date;
};

export const getHmsFromMilliSeconds = (milliSeconds?: number): string => {
  if (!milliSeconds || milliSeconds === 0) {
    return "00:00";
  }
  const { hours, minutes, seconds } = convertMilliSecondsToHms(milliSeconds);

  return `${padNumber(hours, 2)}:${padNumber(minutes, 2)}:${padNumber(seconds, 2)}`;
};

export const convertMilliSecondsToHms = (milliSeconds?: number): { hours: number, minutes: number, seconds: number } => {
  if (!milliSeconds || milliSeconds === 0) {
    return { hours: 0, minutes: 0, seconds: 0 };
  }
  const seconds = milliSeconds / 1000;
  const h = Math.floor(seconds / 3600);
  const m = Math.floor((seconds % 3600) / 60);
  const s = Math.round((seconds % 3600) % 60);

  return { hours: h, minutes: m, seconds: s };
};

export const convertMilliSecondsFromHms = (hours?: number, minutes?: number, seconds?: number): number => {
  const hMs = hours ? hours * 3600000 : 0;
  const mMs = minutes ? minutes * 60000 : 0;
  const sMs = seconds ? seconds * 1000 : 0;
  return hMs + mMs + sMs;
}

export const formatNumberToDkNumber = (value?: number): string => {
  if (!value) return "0";
  return value.toString().replace(".", ",");
};

export const formatNumberToAmount = (value?: number, maxFracDigits?: number): string => {
  if (!value) return "0";
  if (!maxFracDigits) {
    maxFracDigits = 2;
  }
  return Intl.NumberFormat(i18next.language, { minimumFractionDigits: 0, maximumFractionDigits: maxFracDigits }).format(value);
};

export const formatStringOfNumbers = (value?: string): string => {
  if (!value) return "0";
  const editedNumber = parseFloat(parseFloat(value.replace(/[.\s]/g, "").replace(/[,\s]/g, ".")).toFixed(2));
  return Intl.NumberFormat(i18next.language, { minimumFractionDigits: 0, maximumFractionDigits: 10 }).format(editedNumber);
};

export const formatNumberToPrice = (value?: number | null): string => {
  if (!value) return "0,00";
  return Intl.NumberFormat(i18next.language, { minimumFractionDigits: 2, maximumFractionDigits: 2 }).format(value);
};

export const padNumber = (value?: number, size: number = 2): string => {
  if (!value) return "00";
  var s = String(value);
  while (s.length < size) {
    s = `0${s}`;
  }
  return s;
};

export const parseCurrencyToFloat = (value: string): number => {
  value = value.replaceAll(".", "");
  value = value.replace(",", ".");
  return parseFloat(value);
}

export const capitalizeString = (value?: string): string => {
  if (!value) {
    return "";
  }
  return value.charAt(0).toUpperCase() + value?.slice(1);
};
