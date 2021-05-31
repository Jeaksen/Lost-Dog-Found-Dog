import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { SheltersService } from 'src/app/services/shelters-service';
import { Shelter } from 'src/app/models/shelter';
import { Address } from 'src/app/models/address';
import { ShelterDog } from 'src/app/models/shelter-dog';

@Component({
  selector: 'app-shelter-details',
  templateUrl: './shelter-details.component.html',
  styleUrls: ['./shelter-details.component.css']
})
export class ShelterDetailsComponent implements OnInit {
  shelterID!: number;
  shelter?: Shelter;
  shelterDogs?: ShelterDog[];
  
  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private shelterService: SheltersService,
    private datepipe: DatePipe) { }

  ngOnInit(): void {
    this.shelterID = parseInt(this.activatedRoute.snapshot.paramMap.get('shelterId')!);
    this.shelterService.getShelterByID(this.shelterID).subscribe(response => {
      this.shelter = response.data;
    });
    this.shelterService.getAllShelterDogs(this.shelterID).subscribe(response => {
      this.shelterDogs = response.data;
    });
  }

  onBackToSheltersClick(): void {
    this.router.navigate(['/shelters-list']);
  }

  onViewDogDetailsClick(dogID: number) {
    //this.router.navigate(['/shelter-dog-details']);
  }
}
