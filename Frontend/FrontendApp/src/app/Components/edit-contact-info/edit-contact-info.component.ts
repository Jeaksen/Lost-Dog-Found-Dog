import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { CustomValidators } from 'src/app/helpers/custom-validators';
import { UserDetailsData } from 'src/app/models/data';
import { AuthenticationService } from 'src/app/services/authentication-service';
import { UserService } from 'src/app/services/user-service';

@Component({
  selector: 'app-edit-contact-info',
  templateUrl: './edit-contact-info.component.html',
  styleUrls: ['./edit-contact-info.component.css']
})
export class EditContactInfoComponent implements OnInit {
  editContactInfoForm = this.fb.group({
    username: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    phoneNumber: ['', [Validators.required, Validators.pattern('^\\d{9}$')]],
    passwords: this.fb.group({
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required, CustomValidators.matchValues('password')]]
    })
  });

  userDetails?: UserDetailsData;
  editUsernameMode: boolean = false;
  editEmailMode: boolean = false;
  editPhoneNumberMode: boolean = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authenticationService: AuthenticationService,
    private userService: UserService) {
      if (!this.authenticationService.loggedIn) { 
        this.router.navigate(['/login']);
      }
    }

  ngOnInit(): void {
    this.editContactInfoForm.get('passwords')?.get('password')?.valueChanges.subscribe(() => {
      this.editContactInfoForm.get('passwords')?.get('confirmPassword')?.updateValueAndValidity();
    });
    this.userService.getUserDetails(+localStorage.getItem('userId')!)
      .subscribe(response => {
        this.userDetails = response.data;
        this.mapUserDataIntoForm();
      });
    this.editContactInfoForm.get('username')?.disable();
    this.editContactInfoForm.get('email')?.disable();
    this.editContactInfoForm.get('phoneNumber')?.disable();
  }

  onEditUsernameClick() {
    this.editUsernameMode = true;
    this.editContactInfoForm.get('username')?.enable();
  }

  onEditEmailClick() {
    this.editEmailMode = true;
    this.editContactInfoForm.get('email')?.enable();
  }

  onEditPhoneNumberClick() {
    this.editPhoneNumberMode = true;
    this.editContactInfoForm.get('phoneNumber')?.enable();
  }

  onCancelClick() {
    this.editUsernameMode = false;
    this.editEmailMode = false;
    this.editPhoneNumberMode = false;
    this.mapUserDataIntoForm();
  }

  onSubmit() {
    this.userService.updateUserDetails(
      this.editContactInfoForm.get('username')?.value,
      this.editContactInfoForm.get('email')?.value,
      this.editContactInfoForm.get('phoneNumber')?.value,
      this.userDetails!.id)
      .pipe(first())
      .subscribe(
        data => {
          this.router.navigate(['/home']);
        },
        error => {

        });
  }

  onSignUpClick() {
    this.router.navigate(['/register']);
  }

  mapUserDataIntoForm() {
    this.editContactInfoForm.get('username')?.setValue(this.userDetails!.userName);
    this.editContactInfoForm.get('email')?.setValue(this.userDetails!.email);
    this.editContactInfoForm.get('phoneNumber')?.setValue(this.userDetails!.phoneNumber);
  }
}
