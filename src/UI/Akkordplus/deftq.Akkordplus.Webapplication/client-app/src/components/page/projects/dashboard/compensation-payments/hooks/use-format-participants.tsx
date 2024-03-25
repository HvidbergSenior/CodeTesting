import { ProjectCompensationParticipant } from "api/generatedApi";
import { sortCompareString } from "utils/compares";

export const useCompensationPaymentFormatter = () => {
  const formatParticipantList = (max: number, participants?: ProjectCompensationParticipant[] | null): string => {
    if (!participants || participants.length === 0) {
      return "";
    }

    const result = [...participants]
      .sort((a, b) => sortCompareString("desc", a.participantName, b.participantName))
      .slice(0, max)
      .map((p) => p.participantName)
      .join(", ");
    return participants.length > result.length ? `${result} ...` : result;
  };

  return { formatParticipantList };
};
