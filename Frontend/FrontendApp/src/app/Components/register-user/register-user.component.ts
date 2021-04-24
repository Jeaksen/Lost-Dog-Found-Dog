import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, ValidationErrors, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';
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

  private constructRegisterForm(): FormData {
    let data = new FormData();
    data.append('username', this.registerUserForm.get('username')?.value);
    data.append('password', this.registerUserForm.get('passwords')?.get('password')?.value);
    data.append('phone_number', this.registerUserForm.get('phoneNumber')?.value);
    data.append('email', this.registerUserForm.get('email')?.value);
    return data;
  }

  onSubmit() {
    this.authenticationService.register(this.constructRegisterForm())
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
