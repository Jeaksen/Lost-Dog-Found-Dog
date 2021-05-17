import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
import { UserType } from 'src/app/models/user-type.enum';
import { AuthenticationService } from 'src/app/services/authentication-service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required])
  });

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService) { }

  ngOnInit(): void {
    if (this.authenticationService.loggedIn) {
      if (localStorage.getItem('userType') == UserType.Shelter) {
        this.router.navigate(['/shelter-employee-home']);
      }
      else if (localStorage.getItem('userType') == UserType.Regular) {
        this.router.navigate(['/home']);
      }
    }
  }

  private constructLoginForm(): FormData {
    let data = new FormData();
    data.append('username', this.loginForm.get('username')?.value);
    data.append('password', this.loginForm.get('password')?.value);
    return data;
  }

  onSubmit() {
    this.authenticationService.login(this.constructLoginForm())
      .pipe(first())
      .subscribe(
        response => {
          if (response.data.userType == UserType.Shelter) {
            this.router.navigate(['/shelter-employee-home']);
          }
          else if (localStorage.getItem('userType') == UserType.Regular) {
            this.router.navigate(['/home']);
          }
        },
        error => {

        });
  }

  onSignUpClick() {
    this.router.navigate(['/register']);
  }
}
