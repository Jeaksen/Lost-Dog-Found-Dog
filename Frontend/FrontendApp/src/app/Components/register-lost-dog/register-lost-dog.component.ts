import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSelectChange } from '@angular/material/select';
import { Router } from '@angular/router';
import { LostDogService } from 'src/app/services/lost-dog-service';

import { DogColorSelector } from '../../selectors/dog-color-selector';
import { DogEarsSelector } from '../../selectors/dog-ears-selector';
import { DogHairSelector } from '../../selectors/dog-hair-selector';
import { DogSizeSelector } from '../../selectors/dog-size-selector';
import { DogTailSelector } from '../../selectors/dog-tail-selector';

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
    location: new FormControl('', [Validators.required]),
  });

  dogColors: string[] = DogColorSelector;
  dogEars: string[] = DogEarsSelector;
  dogHair: string[] = DogHairSelector;
  dogSizes: string[] = DogSizeSelector;
  dogTails: string[] = DogTailSelector;
  
  constructor(
    private router: Router,
    private lostDogService: LostDogService) { }

  ngOnInit(): void {
  }

  onSubmit() {
    this.lostDogService.postLostDog(this.constructLostDogForm()).subscribe(response => console.log(response));
    this.router.navigate(['/home']);
  }

  private constructLostDogForm(): FormData {
    const data = new FormData();
    data.append('name', this.registerLostDogForm.get('name')?.value);
    data.append('breed', this.registerLostDogForm.get('breed')?.value);
    data.append('age', this.registerLostDogForm.get('age')?.value);
    data.append('size', this.registerLostDogForm.get('size')?.value);
    data.append('color', this.registerLostDogForm.get('color')?.value);
    data.append('hairLength', this.registerLostDogForm.get('hairLength')?.value);
    data.append('tailLength', this.registerLostDogForm.get('tailLength')?.value);
    data.append('earsType', this.registerLostDogForm.get('earsType')?.value);
    data.append('dateLost', this.registerLostDogForm.get('dateLost')?.value);
    data.append('specialMark', '');
    data.append('behaviors', '');
    data.append('location.City', '');
    data.append('location.District', '');
    data.append('ownerId', '1');
    

    return data;
  }

  onCancel() {
    this.router.navigate(['/home']);
  }

  onOptionSetChangedHandler(event: MatSelectChange, controlName: string) {
    this.registerLostDogForm.get(controlName)?.setValue(event.value);
  }

}