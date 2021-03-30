import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LostDog } from '../../models/lost-dog';
import { LostDogService } from '../../services/lost-dog-service';
import { AuthenticationService } from '../../services/authentication-service';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {
  lostDogs?: LostDog[];
  imagePath?: string;

  constructor(
    private router: Router,
    private sanitizer: DomSanitizer,
    private lostDogService: LostDogService,
    private authenticationService: AuthenticationService) { }

  getLostDogs(): void {
    this.lostDogService.getAllLostDogs()
      .subscribe(response => {
        this.lostDogs = response.data;
      });
  }

  ngOnInit(): void {
    if (!this.authenticationService.loggedIn) {
      this.router.navigate(['/login']);
    }
    this.getLostDogs();
  }

  onClick() {
    this.router.navigate(['/register-lost-dog']);
  }

  onMarkAsFoundClick(lostDogId: number) {
    this.lostDogService.MarkLostDogAsFound(lostDogId)
      .subscribe(response => {
        console.log(response);
        this.lostDogs!.find(dog => dog.id === lostDogId)!.isFound = true;
      });
  }
}
