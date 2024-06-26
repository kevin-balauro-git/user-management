import {
  AbstractControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
} from '@angular/forms';

export class ConfirmPasswordValidator {
  public static isSame(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const password = control.parent?.get('password')?.value;
      const confirmPassword = control.value;

      return password !== confirmPassword ? { notSame: true } : null;
    };
  }
}
