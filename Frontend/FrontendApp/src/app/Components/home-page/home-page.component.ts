import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LostDog } from '../../models/lost-dog';
import { LostDogService } from '../../services/lost-dog-service';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {
  lostDogs?: LostDog[];

  constructor(
    private router: Router,
    private lostDogService: LostDogService) { }

  getLostDogs(): void {
    this.lostDogService.getAllLostDogs()
      .subscribe(response => {
        this.lostDogs = response.data;
      });
  }

  ngOnInit(): void {
    this.getLostDogs();
    console.log(this.lostDogs)
  }

  onClick() {
    this.router.navigate(['/register-lost-dog']);
  }
}
