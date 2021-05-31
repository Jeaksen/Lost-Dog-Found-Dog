import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ShelterDog } from '../../models/shelter-dog';
import { SheltersService } from '../../services/shelters-service';
import { AuthenticationService } from '../../services/authentication-service'
import { Shelter } from 'src/app/models/shelter';

@Component({
  selector: 'app-shelter-employee-home-page',
  templateUrl: './shelter-employee-home-page.component.html',
  styleUrls: ['./shelter-employee-home-page.component.css']
})
export class ShelterEmployeeHomePageComponent implements OnInit {
  shelterDogs?: ShelterDog[];
  shelter?: Shelter;
  shelterID!: number;

  constructor(private router: Router,
    private shelterService: SheltersService,
    private authenticationService: AuthenticationService) { }

  getShelterDogs(): void {
    this.shelterService.getAllShelterDogs(this.shelterID)
      .subscribe(response => {
        this.shelterDogs = response.data;
      });
  }

  ngOnInit(): void {
    if (!this.authenticationService.loggedIn) {
      this.router.navigate(['/login']);
    }
    this.shelterID = +localStorage.getItem('userId')!
    this.shelterService.getShelterByID(this.shelterID)
    .subscribe(response => {
      this.shelter = response.data;
    });
    this.getShelterDogs();
  }

  onViewDetailsClick(dogID: number) {

  }

  onDeleteShelterDogClick(dogID: number) {
    this.shelterService.deleteShelterDog(this.shelterID, dogID).subscribe(
      response => {
        console.log(response);
        window.location.reload();
    });
  }

}
