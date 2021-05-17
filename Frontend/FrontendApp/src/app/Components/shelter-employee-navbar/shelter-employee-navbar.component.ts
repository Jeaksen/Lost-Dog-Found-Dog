import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication-service';

@Component({
  selector: 'app-shelter-employee-navbar',
  templateUrl: './shelter-employee-navbar.component.html',
  styleUrls: ['./shelter-employee-navbar.component.css']
})
export class ShelterEmployeeNavbarComponent implements OnInit {

  constructor(private authenticationService: AuthenticationService) { }

  ngOnInit(): void {
  }

  onClick() {
    this.authenticationService.logout();
  }

}
