import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSelectChange } from '@angular/material/select';
import { Router, ActivatedRoute } from '@angular/router';
import { SheltersService } from 'src/app/services/shelters-service';
import { ImageSnippet } from '../../models/image-snippet';

import { DogColorSelector } from '../../selectors/dog-color-selector';
import { DogEarsSelector } from '../../selectors/dog-ears-selector';
import { DogHairSelector } from '../../selectors/dog-hair-selector';
import { DogSizeSelector } from '../../selectors/dog-size-selector';
import { DogTailSelector } from '../../selectors/dog-tail-selector';
import { ShelterDog } from 'src/app/models/shelter-dog';
import { Shelter } from 'src/app/models/shelter';

@Component({
  selector: 'app-register-shelter-dog',
  templateUrl: './register-shelter-dog.component.html',
  styleUrls: ['./register-shelter-dog.component.css']
})
export class RegisterShelterDogComponent implements OnInit {
  registerShelterDogForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    breed: new FormControl('', [Validators.required]),
    age: new FormControl('', [Validators.required]),
    size: new FormControl('', [Validators.required]),
    color: new FormControl('', [Validators.required]),
    hairLength: new FormControl('', [Validators.required]),
    tailLength: new FormControl('', [Validators.required]),
    earsType: new FormControl('', [Validators.required]),
    specialMarks: new FormControl('', [Validators.required]),
    behavior: new FormControl('', [Validators.required]),
  });

  selectedFile!: ImageSnippet;
  url!: any;
  shelterID!: number;
  shelter? : Shelter;
  dogColors: string[] = DogColorSelector;
  dogEars: string[] = DogEarsSelector;
  dogHair: string[] = DogHairSelector;
  dogSizes: string[] = DogSizeSelector;
  dogTails: string[] = DogTailSelector;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private shelterService: SheltersService,) { }

  ngOnInit(): void {
    this.shelterID = parseInt(this.activatedRoute.snapshot.paramMap.get('shelterId')!);
    this.shelterService.getShelterByID(this.shelterID).subscribe(response => {
      this.shelter = response.data;
    });
    this.registerShelterDogForm.get('specialMarks')?.setValue("None");
  }

  onSubmit() {
    this.shelterService.postShelterDog(this.shelterID, this.constructShelterDogForm()).subscribe(response => {
      this.router.navigate(['/shelter-employee-home']);
    });
  }

  private constructShelterDogForm(): FormData {
    const shelterDog = new ShelterDog(
      this.registerShelterDogForm.get('name')?.value, 
      this.registerShelterDogForm.get('breed')?.value,
      this.registerShelterDogForm.get('age')?.value, 
      this.registerShelterDogForm.get('size')?.value,
      this.registerShelterDogForm.get('color')?.value,
      this.registerShelterDogForm.get('specialMarks')?.value, 
      this.registerShelterDogForm.get('hairLength')?.value,
      this.registerShelterDogForm.get('earsType')?.value, 
      this.registerShelterDogForm.get('tailLength')?.value,
      [ this.registerShelterDogForm.get('behavior')?.value ],   
    );
    let data = new FormData();
    // works only with our Backend
    // data.append('dog', JSON.stringify(shelterDog));
    data.append("dog", new Blob([JSON.stringify(shelterDog)], { type: "application/json", }), "");
    data.append('picture', this.selectedFile.file);
    return data;
  }

  onCancel() {
    this.router.navigate(['/shelter-employee-home']);
  }

  onOptionSetChangedHandler(event: MatSelectChange, controlName: string) {
    this.registerShelterDogForm.get(controlName)?.setValue(event.value);
  }

  processFile(event: any) {
    const file: File = event.target.files[0];
    const reader = new FileReader();

    reader.addEventListener('load', (even: any) => {
      this.selectedFile = new ImageSnippet(even.target.result, file);
    });
    
    reader.readAsDataURL(file);
    reader.onload = (event: any) => {
      this.url = reader.result;
    }
  }

}
