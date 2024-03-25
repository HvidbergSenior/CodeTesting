import { UseFormGetValues } from "react-hook-form";
import { CreateProjectUserFormData } from "../inputs/project-user-form-data";

export const useValidateProjectUserInputs = (getValue: UseFormGetValues<CreateProjectUserFormData>) => {
  const validateHasName = (): boolean => {
    const name = getValue("name");
    if (!name || name === "") {
      return false;
    } else {
      return true;
    }
  };

  const validateHasEmail = (): boolean => {
    const email = getValue("email");
    if (!email || email === "") {
      return false;
    } else {
      return true;
    }
  };

  const validateHasValidEmail = (): boolean => {
    const email = getValue("email");
    if (!/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i.test(email)) {
      return false;
    } else {
      return true;
    }
  };

  const validateHasValidPhone = (phone: string): boolean => {
    if (!phone || phone === "") {
      return true;
    }
    return phone.length === 8;
  };

  const validateCreateParameter = () => {
    return validateHasName() && validateHasEmail() && validateHasValidEmail();
  };

  return { validateHasName, validateHasEmail, validateHasValidEmail, validateHasValidPhone, validateCreateParameter };
};
