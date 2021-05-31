import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { SheltersService } from 'src/app/services/shelters-service';
import { ShelterDog } from 'src/app/models/shelter-dog';

@Component({
  selector: 'app-shelter-dog-details',
  templateUrl: './shelter-dog-details.component.html',
  styleUrls: ['./shelter-dog-details.component.css']
})
export class ShelterDogDetailsComponent implements OnInit {
  url!: any;
  previousUrl!: string;
  shelterID!: number;
  shelterDogID!: number;
  shelterDog?: ShelterDog;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private shelterService: SheltersService) { }

  ngOnInit(): void {
    this.previousUrl = this.activatedRoute.snapshot.paramMap.get('previousUrl')!;
    this.shelterID = parseInt(this.activatedRoute.snapshot.paramMap.get('shelterId')!);
    this.shelterDogID = parseInt(this.activatedRoute.snapshot.paramMap.get('dogId')!);
    this.shelterService.getShelterDogByID(this.shelterID, this.shelterDogID).subscribe(response => {
      this.shelterDog = response.data;
      this.url = 'data:' + this.shelterDog!.picture!.fileType + ';base64,' + this.shelterDog!.picture!.data;
    });
  }

  onBackToListClick(): void {
    this.router.navigateByUrl(this.previousUrl);
  }

}
