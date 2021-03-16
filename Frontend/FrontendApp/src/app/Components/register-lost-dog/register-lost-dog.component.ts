import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSelectChange } from '@angular/material/select';
import { Router } from '@angular/router';

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
    earsType: new FormControl('', [Validators.required])
  });

  dogColors: string[] = DogColorSelector;
  dogEars: string[] = DogEarsSelector;
  dogHair: string[] = DogHairSelector;
  dogSizes: string[] = DogSizeSelector;
  dogTails: string[] = DogTailSelector;
  
  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  onSubmit() {
    console.log(this.registerLostDogForm.get('size')?.value);
    this.router.navigate(['/home']);
  }

  onOptionSetChangedHandler(event: MatSelectChange, controlName: string) {
    this.registerLostDogForm.get(controlName)?.setValue(event.value);
  }

}
