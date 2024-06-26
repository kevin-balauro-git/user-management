import { AbstractControl, ValidationErrors } from '@angular/forms';

export class PasswordValidator {
  public static isWeak(control: AbstractControl): ValidationErrors | null {
    const hasNumber = /\d/.test(control.value);
    const hasUpper = /[A-Z]/.test(control.value);
    const hasSpecialCharacter = /[\^_=\!#\$%&\(\)\*\+\-\.:'/\?@]/.test(
      control.value
    );
    return hasNumber && hasUpper && hasSpecialCharacter ? null : { weak: true };
  }

  public static hasSpace(control: AbstractControl): ValidationErrors | null {
    const value = [...control.value.toString()];
    let space = false;
    value.forEach((element, index) => {
      if (element === ' ') {
        space = true;
      }
    });
    return space ? { hasSpace: true } : null;
  }

  //   public static firstLetterNotDigit(
  //     control: AbstractControl
  //   ): ValidationErrors | null {
  //     const firstSubstring = /\d/.test(control.value.toString().substring(0, 1));
  //     console.log(firstSubstring);
  //     return { firstLetter: true };
  //   }
}
