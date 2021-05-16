import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSelectChange } from '@angular/material/select';
import { Router } from '@angular/router';
import { LostDogService } from 'src/app/services/lost-dog-service';
import { ImageSnippet } from '../../models/image-snippet';

import { DogColorSelector } from '../../selectors/dog-color-selector';
import { DogEarsSelector } from '../../selectors/dog-ears-selector';
import { DogHairSelector } from '../../selectors/dog-hair-selector';
import { DogSizeSelector } from '../../selectors/dog-size-selector';
import { DogTailSelector } from '../../selectors/dog-tail-selector';
import { LostDog } from 'src/app/models/lost-dog';
import { Location } from 'src/app/models/location';

@Component({
  selector: 'app-register-lost-dog',
  templateUrl: './register-lost-dog.component.html',
  styleUrls: ['./register-lost-dog.component.css']
})
export class RegisterLostDogComponent implements OnInit {
  registerLostDogForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    breed: new FormControl('', [Validators.required]),
    age: new FormControl('', [Validators.required]),
    size: new FormControl('', [Validators.required]),
    color: new FormControl('', [Validators.required]),
    hairLength: new FormControl('', [Validators.required]),
    tailLength: new FormControl('', [Validators.required]),
    earsType: new FormControl('', [Validators.required]),
    dateLost: new FormControl('', [Validators.required]),
    specialMarks: new FormControl('', [Validators.required]),
    behavior: new FormControl('', [Validators.required]),
    locationCity: new FormControl('', [Validators.required]),
    locationDistrict: new FormControl('', [Validators.required]),
  });

  selectedFile!: ImageSnippet;
  url!: any;
  dogColors: string[] = DogColorSelector;
  dogEars: string[] = DogEarsSelector;
  dogHair: string[] = DogHairSelector;
  dogSizes: string[] = DogSizeSelector;
  dogTails: string[] = DogTailSelector;
  
  constructor(
    private router: Router,
    private lostDogService: LostDogService,
    private datepipe: DatePipe) { }

  ngOnInit(): void {
  }

  onSubmit() {
    console.log(this.registerLostDogForm);
    this.lostDogService.postLostDog(this.constructLostDogForm()).subscribe(response => {
      console.log(response)
      this.router.navigate(['/home']);
    });
  }

  private constructLostDogForm(): FormData {
    const location = new Location(
      this.registerLostDogForm.get('locationCity')?.value,
      this.registerLostDogForm.get('locationDistrict')?.value
    );
    const lostDog = new LostDog(
      this.registerLostDogForm.get('name')?.value, 
      this.registerLostDogForm.get('breed')?.value,
      this.registerLostDogForm.get('age')?.value, 
      this.registerLostDogForm.get('size')?.value,
      this.registerLostDogForm.get('color')?.value,
      this.registerLostDogForm.get('specialMarks')?.value, 
      this.registerLostDogForm.get('hairLength')?.value,
      this.registerLostDogForm.get('earsType')?.value, 
      this.registerLostDogForm.get('tailLength')?.value,
      [ this.registerLostDogForm.get('behavior')?.value ],
      location, 
      this.datepipe.transform(this.registerLostDogForm.get('dateLost')?.value, 'yyyy-MM-dd')!
    );
    let data = new FormData();
    data.append('dog', JSON.stringify(lostDog));
    data.append('picture', this.selectedFile.file);
    console.log(data.forEach(val => console.log(val)));
    return data;
  }

  onCancel() {
    this.router.navigate(['/home']);
  }

  onOptionSetChangedHandler(event: MatSelectChange, controlName: string) {
    this.registerLostDogForm.get(controlName)?.setValue(event.value);
  }

  processFile(event: any) {
    const file: File = event.target.files[0];
    const reader = new FileReader();

    reader.addEventListener('load', (even: any) => {
      console.log(file);
      this.selectedFile = new ImageSnippet(even.target.result, file);
    });
    
    reader.readAsDataURL(file);
    reader.onload = (event: any) => {
      this.url = reader.result;
    }
  }

}