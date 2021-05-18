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
import { PageEvent } from '@angular/material/paginator';

interface SortValues {
  value: string;
  viewValue: string;
}

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
  sortingForm = new FormGroup({
    sort: new FormControl('', []),
    option: new FormControl('', []),
  });

  lostDogs?: LostDogFromBackend[];
  dogColors: string[] = DogColorSelector;
  dogSizes: string[] = DogSizeSelector;
  allPagesCount?: number;
  currentPage: number;
  currentSize: number;
  sizeOptions: number[] = [5, 10, 15, 20, 25, 50];

  sortFeatures: SortValues[] = [
    {value: 'name', viewValue: 'Dog\'s Name'},
    {value: 'breed', viewValue: 'Breed'},
    {value: 'age', viewValue: 'Age'},
    {value: 'size', viewValue: 'Size'},
    {value: 'color', viewValue: 'Color'},
    {value: 'hairLength', viewValue: 'Hair Length'},
    {value: 'earsType', viewValue: 'Ears Type'},
    {value: 'tailLength', viewValue: 'Tail Length'},
    {value: 'dateLost', viewValue: 'Date Lost'},
    {value: 'specialMark', viewValue: 'Special Marks'},
    //{value: 'behaviors', viewValue: 'Behaviour'},
    {value: 'location.city', viewValue: 'City'},
    {value: 'location.district', viewValue: 'District'},
  ];

  constructor(
    private router: Router,
    private datepipe: DatePipe,
    private lostDogService: LostDogService,
    private authenticationService: AuthenticationService) { 
      this.currentPage = 1; 
      this.currentSize = 10;
    }

  getLostDogs(): void {
    console.log(localStorage.getItem('userId')!)
    this.lostDogService.getAllLostDogs()
      .subscribe(response => {
        this.lostDogs = response.data;
        this.allPagesCount = response.metadata;
      });
  }  

  ngOnInit(): void {
    if (!this.authenticationService.loggedIn) {
      this.router.navigate(['/login']);
    }
    this.getLostDogs();
    //this.sortingForm.get('sort')?.setValue(this.sortFeatures[8]); //-> psuje wysyłanie requesta :(
  }
  
  onSubmit() {
    console.log(this.filterForm);
    this.lostDogService.getFilteredLostDogs(this.constructFilterString()).subscribe(response => {
      console.log(response)
      this.lostDogs = response.data;
      this.allPagesCount = response.metadata;
    });
  }

  constructFilterString(): string {
    let filter = '';
    if(this.filterForm.get('name')?.value) 
        filter += "filter.name=" + this.filterForm.get('name')?.value + '&';
    if(this.filterForm.get('breed')?.value) 
        filter += "filter.breed=" + this.filterForm.get('breed')?.value + '&';
    if(this.filterForm.get('ageFrom')?.value) 
        filter += "filter.ageFrom=" + this.filterForm.get('ageFrom')?.value + '&';
    if(this.filterForm.get('ageTo')?.value) 
        filter += "filter.ageTo=" + this.filterForm.get('ageTo')?.value + '&';
    if(this.filterForm.get('size')?.value) 
        filter += "filter.size=" + this.filterForm.get('size')?.value + '&';
    if(this.filterForm.get('color')?.value) 
        filter += "filter.color=" + this.filterForm.get('color')?.value + '&';
    if(this.filterForm.get('locationCity')?.value) 
        filter += "filter.location.city=" + this.filterForm.get('locationCity')?.value + '&';
    if(this.filterForm.get('locationDistrict')?.value) 
        filter += "filter.location.district=" + this.filterForm.get('locationDistrict')?.value + '&';
    if(this.filterForm.get('dateLostBefore')?.value) 
        filter += "filter.dateLostBefore=" + this.datepipe.transform(this.filterForm.get('dateLostBefore')?.value, 'yyyy-MM-dd')! + '&';
    if(this.filterForm.get('dateLostAfter')?.value) 
        filter += "filter.dateLostAfter=" + this.datepipe.transform(this.filterForm.get('dateLostAfter')?.value, 'yyyy-MM-dd')! + '&';
    console.log(filter);
    if(this.sortingForm.get('sort')?.value) {
      filter += 'sort=' + this.sortingForm.get('sort')?.value;
      if(this.sortingForm.get('option')?.value) filter += ',' + this.sortingForm.get('option')?.value;
    }
    let tmp = this.currentPage - 1;
    filter += '&page=' + tmp;
    filter += '&size=' + this.currentSize;
    console.log(filter);
    return filter;
  }

  onClearForm(): void {
    this.filterForm.reset({
      'name': '',
      'breed': '',
      'ageFrom': '',
      'ageTo': '',
      'size': '',
      'color': '',
      'dateLostBefore':'',
      'dateLostAfter': '',
      'locationCity': '',
      'locationDistrict': '',
    });
    this.onSubmit();
  }

  onClearSortForm(): void {
    this.sortingForm.reset({
      'sort': '',
      'option': '',
    });
    this.onSubmit();
  }

  onOptionSetChangedHandler(event: MatSelectChange, controlName: string) {
    this.filterForm.get(controlName)?.setValue(event.value);
  }
  onSortingSetChangedHandler(event: MatSelectChange, controlName: string) {
    this.sortingForm.get(controlName)?.setValue(event.value);
  }
  onPageSizeChangedHandler(event: MatSelectChange, controlName: string) {
    this.currentSize = event.value;
    this.onSubmit();
  }

  onChangePageClick(pageNum: number, firstLast: boolean) {
    if (firstLast)
        this.currentPage = pageNum;
    else this.currentPage += pageNum;
    this.onSubmit();
  }

}