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
    specialMarks: new FormControl('', []),
    behaviour: new FormControl('', []),
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
    this.lostDogService.postLostDog(this.constructLostDogForm()).subscribe(response => console.log(response));
    this.router.navigate(['/home']);
  }

  private constructLostDogForm(): FormData {
    const location = new Location(this.registerLostDogForm.get('locationCity')?.value,
      this.registerLostDogForm.get('locationDistrict')?.value);
    const lostDog = new LostDog(this.registerLostDogForm.get('name')?.value, this.registerLostDogForm.get('breed')?.value,
      this.registerLostDogForm.get('age')?.value, this.registerLostDogForm.get('size')?.value,
      this.registerLostDogForm.get('color')?.value, 'costam', this.registerLostDogForm.get('hairLength')?.value,
      this.registerLostDogForm.get('earsType')?.value, this.registerLostDogForm.get('tailLength')?.value,
      ['behav1', 'behav2'], location, this.datepipe.transform(this.registerLostDogForm.get('dateLost')?.value, 'yyyy-MM-dd')!);
    let data = new FormData();

    // u better not change the order
    //data.append('breed', this.registerLostDogForm.get('breed')?.value);
    //data.append('breed', 'Doo');
    //data.append('age', this.registerLostDogForm.get('age')?.value);
    //data.append('age', '18');
    //data.append('size', this.registerLostDogForm.get('size')?.value);
    //data.append('size', 'Enormous');
    //data.append('color', this.registerLostDogForm.get('color')?.value);
    //data.append('color', 'Black');
    //data.append('specialMark', 'costam');
    //data.append('specialMark', 'Lacks one leg');
    //data.append('name', this.registerLostDogForm.get('name')?.value);
    //data.append('name', 'Scooby');
    //data.append('hairLength', this.registerLostDogForm.get('hairLength')?.value);
    //data.append('hairLength', 'Long');
    //data.append('tailLength', this.registerLostDogForm.get('tailLength')?.value);
    //data.append('tailLength', 'Short');
    //data.append('earsType', this.registerLostDogForm.get('earsType')?.value);
    //data.append('earsType', 'He doesn\'t have'); 
    //data.append('behaviors', 'Depression');
    //data.append('behaviors', 'Prosze zaakceptuj to');
    //data.append('location.City', this.registerLostDogForm.get('locationCity')?.value);
    //data.append('location.City', 'Chrzęszczyszczeborzyce');
    //data.append('location.District', this.registerLostDogForm.get('locationDistrict')?.value);
    //data.append('location.District', 'Łękoboldy');
    //data.append('dateLost', this.datepipe.transform(this.registerLostDogForm.get('dateLost')?.value, 'yyyy-MM-dd')!);
    //data.append('dateLost', '2021-03-23');
    //data.append('ownerId', localStorage.getItem('userId')!);
    data.append('picture', this.selectedFile.file);
    console.log(data.forEach(val => console.log(val)));
    //console.log(data.get('image'));
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