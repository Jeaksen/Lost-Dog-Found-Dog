import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormBuilder, Validators, ValidationErrors, AbstractControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
import { RegisterUserRequest } from 'src/app/models/register-user-request';
import { AuthenticationService } from 'src/app/services/authentication-service';

@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrls: ['./register-user.component.css']
})
export class RegisterUserComponent implements OnInit {
  registerUserForm = this.fb.group({
    username: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    phoneNumber: ['', [Validators.required, Validators.pattern('^\\d{9}$')]],
    passwords: this.fb.group({
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required, RegisterUserComponent.matchValues('password')]]
    })
  });

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authenticationService: AuthenticationService) { }

  ngOnInit(): void {
    this.registerUserForm.get('passwords')?.get('password')?.valueChanges.subscribe(() => {
      this.registerUserForm.get('passwords')?.get('confirmPassword')?.updateValueAndValidity();
    });
  }

  onSubmit() {
    const RegisterRequest: RegisterUserRequest = new RegisterUserRequest(
      this.registerUserForm.get('username')?.value,
      this.registerUserForm.get('passwords')?.get('password')?.value,
      this.registerUserForm.get('email')?.value,
      this.registerUserForm.get('phoneNumber')?.value
    );
    this.authenticationService.register(RegisterRequest)
      .pipe(first())
      .subscribe(
        data => {
          this.router.navigate(['/login']);
        },
        error => {

        });
  }

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
