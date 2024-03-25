import { useTranslation } from "react-i18next";
import CheckIcon from "@mui/icons-material/Check";
import ErrorOutlineIcon from "@mui/icons-material/ErrorOutline";

import { Props as ResultProps } from "shared/dialog/types";
import { ResultDialog } from "components/base/result-dialog/result-dialog";
import { useDialog } from "shared/dialog/use-dialog";
import { isProblemDetails } from "utils/errors/predicates";
import { useToast } from "shared/toast/hooks/use-toast";

type Unwrappable = { unwrap: () => Promise<unknown> };

export enum ResultDialogType {
  Toast,
  Dialog,
}

type Options = {
  onSuccess?: () => void;
  onSuccessReponse?: (response: any) => void;
  onError?: () => void;
  successProps?: Partial<Omit<ResultProps, "isOpen">>;
  errorProps?: Partial<Omit<ResultProps, "isOpen">>;
  resultDialogType: ResultDialogType;
};

export function useMutation({ successProps, errorProps, onSuccess, onSuccessReponse, onError, resultDialogType }: Options) {
  const { t } = useTranslation();
  const [openResult] = useDialog(ResultDialog);
  const toast = useToast();

  return async ({ unwrap }: Unwrappable) => {
    let response: any;
    try {
      response = await unwrap();
    } catch (error) {
      const problemDetails = (error as any)?.data;

      if (isProblemDetails(problemDetails) && problemDetails.code) {
        console.error("Error", problemDetails);
      }

      if (resultDialogType === ResultDialogType.Toast) {
        toast.error(errorProps?.description ?? t("form.error.description"));
      }
      if (resultDialogType === ResultDialogType.Dialog) {
        openResult({
          title: t("form.error.title"),
          description: t("form.error.description"),
          icon: ErrorOutlineIcon,
          color: (theme) => theme.palette.error.main,
          ...errorProps,
        });
      }

      onError?.();
      return [];
    }

    if (onSuccess) {
      onSuccess();
    }

    if (onSuccessReponse) {
      onSuccessReponse(response);
    }

    if (resultDialogType === ResultDialogType.Toast) {
      toast.success(successProps?.description ?? t("form.success.description"));
    }
    if (resultDialogType === ResultDialogType.Dialog) {
      openResult({
        icon: CheckIcon,
        color: (theme) => theme.palette.success.main,
        ...successProps,
      });
    }

    return [];
  };
}
