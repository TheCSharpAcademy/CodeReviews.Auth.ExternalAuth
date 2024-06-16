import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const passwordValidator: ValidatorFn = (
  control: AbstractControl,
): ValidationErrors | null => {
  if (!control.value) {
    return null;
  }

  const hasLowercase: boolean = /[a-z]/.test(control.value);
  const hasSpecialChar: boolean = /[!@#$%^&*(),.?":{}|<>]/.test(control.value);
  const hasDigit: boolean = /[0-9]/.test(control.value);

  if (!hasLowercase || !hasSpecialChar || !hasDigit) {
    return {
      invalidPassword: true,
    };
  }

  return null;
};
