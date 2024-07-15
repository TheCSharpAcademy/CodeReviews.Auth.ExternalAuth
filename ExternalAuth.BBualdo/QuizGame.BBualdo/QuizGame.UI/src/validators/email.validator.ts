import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const emailValidator: ValidatorFn = (
  control: AbstractControl,
): ValidationErrors | null => {
  const regex: RegExp = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/;

  if (control.value && !control.value.match(regex)) {
    return {
      invalidEmail: true,
    };
  }

  return null;
};
