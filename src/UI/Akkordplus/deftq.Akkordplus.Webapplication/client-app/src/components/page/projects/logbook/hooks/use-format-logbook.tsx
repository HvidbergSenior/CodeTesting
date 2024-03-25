export const timeToDecimal = (hours?: number, minutes?: number): number => {
  if (hours !== undefined && minutes !== undefined) {
    let min = 0;
    if (minutes === 15) {
      min = 0.25;
    }
    if (minutes === 30) {
      min = 0.5;
    }
    if (minutes === 45) {
      min = 0.75;
    }
    return hours + min;
  }
  return 0;
};
