import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSelectChange } from '@angular/material/select';
import { Router } from '@angular/router';

import { DogSizeSelector } from '../../selectors/dog-size-selector';

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

  dogSizes: string[] = DogSizeSelector;
  
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
