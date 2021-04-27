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
  allDogsCount?: number;;
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
    private authenticationService: AuthenticationService) { }

  getLostDogs(): void {
    console.log(localStorage.getItem('userId')!)
    this.lostDogService.getAllLostDogs()
      .subscribe(response => {
        this.lostDogs = response.data;
        this.allDogsCount = this.lostDogs?.length;
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
      this.lostDogs = response.data;
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
    //filter += '&size=5&page=2'
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

  minValue: number = 0;
  maxValue: number = 20;
  public getPaginatorData(event: PageEvent): PageEvent {
    this.minValue = event.pageIndex * event.pageSize;
    this.maxValue = this.minValue + event.pageSize;
    return event;
  }

//   private loadPage(page) {
//     // get page of items from api
//     this.lostDogService.getFilteredLostDogs().subscribe(response => {
//       console.log(response)
//       this.pager = response.pager;
//       this.lostDogs = response.data;
//     });
//     this.http.get<any>(`/api/items?page=${page}`).subscribe(x => {
//         this.pager = x.pager;
//         this.pageOfDogs = x.pageOfItems;
//     });
// }

}
