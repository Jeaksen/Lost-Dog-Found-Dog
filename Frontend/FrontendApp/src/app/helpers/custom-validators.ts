import { AbstractControl, ValidationErrors } from "@angular/forms";

export class CustomValidators {
    public static matchValues(
        matchTo: string // name of the control to match to
      ): (arg0: AbstractControl) => ValidationErrors | null {
        return (control: AbstractControl): ValidationErrors | null => {
          return !!control.parent &&
            !!control.parent.value &&
            control.value === control.parent.get(matchTo)?.value
            ? null
            : { isMatching: false };
        };
      }
}