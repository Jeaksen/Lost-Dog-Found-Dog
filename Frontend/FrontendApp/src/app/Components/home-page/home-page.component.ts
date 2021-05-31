import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LostDogFromBackend } from '../../models/lost-dog-from-backend';
import { LostDogService } from '../../services/lost-dog-service';
import { AuthenticationService } from '../../services/authentication-service'
import { UserService } from 'src/app/services/user-service';
import { UserDetailsData } from 'src/app/models/data';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {
  userDetails?: UserDetailsData;
  lostDogs?: LostDogFromBackend[];
  imagePath?: string;
  dogID?: number;

  constructor(
    private router: Router,
    private lostDogService: LostDogService,
    private userService: UserService,
    private authenticationService: AuthenticationService) { }

  getLostDogs(): void {
    this.lostDogService.getUserLostDogs(localStorage.getItem('userId')!)
      .subscribe(response => {
        this.lostDogs = response.data;
        console.log("Count: " + this.lostDogs.length);
      });
  }

  ngOnInit(): void {
    if (!this.authenticationService.loggedIn) {
      this.router.navigate(['/login']);
    }
    this.userService.getUserDetails(+localStorage.getItem('userId')!)
      .subscribe(response => {
        this.userDetails = response.data;
      });
    this.getLostDogs();
  }

  onRegisterLostDogClick() {
    this.router.navigate(['/register-lost-dog']);
  }

  onEditDetailsClick(lostDogId: number) {
    this.router.navigate(['/edit-lost-dog', lostDogId]);
  }
  
  onEditContactInfoClick() {
    this.router.navigate(['/edit-contact-info']);
  }

  onMarkAsFoundClick(lostDogId: number) {
    if(confirm("Are you sure you want mark dog as found?")) {
    this.lostDogService.MarkLostDogAsFound(lostDogId)
      .subscribe(response => {
        console.log(response);
        this.lostDogs!.find(dog => dog.id === lostDogId)!.isFound = true;
      });
    }
  }
}
