import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { LostDogService } from 'src/app/services/lost-dog-service';
import { LostDog } from 'src/app/models/lost-dog';
import { LostDogFromBackend } from '../../models/lost-dog-from-backend';


@Component({
  selector: 'app-lost-dog-details',
  templateUrl: './lost-dog-details.component.html',
  styleUrls: ['./lost-dog-details.component.css']
})
export class LostDogDetailsComponent implements OnInit {
  url!: any;
  lostDogID!: number;
  lostDog?: LostDogFromBackend;
  
  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private lostDogService: LostDogService,
    private datepipe: DatePipe) { }

  ngOnInit(): void {
    this.lostDogID = parseInt(this.activatedRoute.snapshot.paramMap.get('dogId')!);
    this.lostDogService.getLostDogByID(this.lostDogID).subscribe(response => {
      this.lostDog = response.data;
      this.url = 'data:' + this.lostDog!.picture!.fileType + ';base64,' + this.lostDog!.picture!.data;
    });
  }

  onBackToListClick(): void {
    this.router.navigate(['/search']);
  }

  onAddCommentClick(): void {

  }
}
