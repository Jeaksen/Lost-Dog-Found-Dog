import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSelectChange } from '@angular/material/select';
import { LostDogService } from 'src/app/services/lost-dog-service';
import { AuthenticationService } from '../../services/authentication-service';
import { LostDogFromBackend } from '../../models/lost-dog-from-backend';

import { DogColorSelector } from '../../selectors/dog-color-selector';
import { DogSizeSelector } from '../../selectors/dog-size-selector';
import { LostDog } from 'src/app/models/lost-dog';
import { Location } from 'src/app/models/location';

@Component({
  selector: 'app-filter-lost-dogs',
  templateUrl: './filter-lost-dogs.component.html',
  styleUrls: ['./filter-lost-dogs.component.css']
})
export class FilterLostDogsComponent implements OnInit {
  filterForm = new FormGroup({
    name: new FormControl('', []),
    breed: new FormControl('', []),
    ageFrom: new FormControl('', []),
    ageTo: new FormControl('', []),
    size: new FormControl('', []),
    color: new FormControl('', []),
    dateLostBefore: new FormControl('', []),
    dateLostAfter: new FormControl('', []),
    locationCity: new FormControl('', []),
    locationDistrict: new FormControl('', []),
  });

  lostDogs?: LostDogFromBackend[];
  dogColors: string[] = DogColorSelector;
  dogSizes: string[] = DogSizeSelector;

  constructor(
    private router: Router,
    private datepipe: DatePipe,
    private lostDogService: LostDogService,
    private authenticationService: AuthenticationService) { }

  getLostDogs(): void {
    console.log(localStorage.getItem('userId')!)
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
  
  onSubmit() {
    console.log(this.filterForm);
    this.lostDogService.getFilteredLostDogs(this.constructFilterString()).subscribe(response => {
      console.log(response)
      //this.router.navigate(['/home']);
    });
  }

  constructFilterString(): string {
    return '';
  }

  onClearForm(): void {

  }

  onOptionSetChangedHandler(event: MatSelectChange, controlName: string) {
    this.filterForm.get(controlName)?.setValue(event.value);
  }

}
