import { DocumentReferenceResponse } from "api/generatedApi";
import { formatTimestampToDate } from "utils/formats";

export function useSortDocuments() {
  const sortDocuments = (input: DocumentReferenceResponse[] | undefined): DocumentReferenceResponse[] => {
    if (!input || input.length <= 0) {
      return [];
    }
    const documents = [...input];
    const docs = documents.sort((a, b) => {
      if (!a?.uploadedTimestamp || !b?.uploadedTimestamp) {
        return 0;
      }
      const dateA = formatTimestampToDate(a.uploadedTimestamp);
      const dateB = formatTimestampToDate(b.uploadedTimestamp);
      return dateA < dateB ? 1 : -1;
    });
    return docs;
  };

  return sortDocuments;
}
