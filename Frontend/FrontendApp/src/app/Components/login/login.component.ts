import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
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
  returnUrl: string = '/home';

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private authenticationService: AuthenticationService) {
      if (this.authenticationService.loggedIn) { 
        this.router.navigate(['/home']);
      }
    }

  ngOnInit(): void {
    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/home';
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
        data => {
          this.router.navigate([this.returnUrl]);
        },
        error => {

        });
  }

  onSignUpClick() {
    this.router.navigate(['/register']);
  }
}
