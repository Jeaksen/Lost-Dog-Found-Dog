import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Shelter } from 'src/app/models/shelter';
import { AuthenticationService } from 'src/app/services/authentication-service';
import { SheltersService } from 'src/app/services/shelters-service';

@Component({
  selector: 'app-see-shelters-list',
  templateUrl: './see-shelters-list.component.html',
  styleUrls: ['./see-shelters-list.component.css']
})
export class SeeSheltersListComponent implements OnInit {
  shelters?: Shelter[];

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService,
    private sheltersService: SheltersService) { }

  getShelters(): void {
    this.sheltersService.getAllShelters().subscribe(response => {
      this.shelters = response.data;
    });
  }

  ngOnInit(): void {
    if (!this.authenticationService.loggedIn) {
      this.router.navigate(['/login']);
    }
    this.getShelters();
  }

}
