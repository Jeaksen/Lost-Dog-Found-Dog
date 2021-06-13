import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSelectChange } from '@angular/material/select';
import { DomSanitizer } from '@angular/platform-browser';
import { Router, ActivatedRoute } from '@angular/router';
import { LostDogService } from 'src/app/services/lost-dog-service';
import { ImageSnippet } from '../../models/image-snippet';
import { LostDog } from 'src/app/models/lost-dog';
import { LostDogFromBackend } from '../../models/lost-dog-from-backend';
import { Location } from 'src/app/models/location';
 
import { DogColorSelector } from '../../selectors/dog-color-selector';
import { DogEarsSelector } from '../../selectors/dog-ears-selector';
import { DogHairSelector } from '../../selectors/dog-hair-selector';
import { DogSizeSelector } from '../../selectors/dog-size-selector';
import { DogTailSelector } from '../../selectors/dog-tail-selector';

@Component({
  selector: 'app-edit-lost-dog',
  templateUrl: './edit-lost-dog.component.html',
  styleUrls: ['./edit-lost-dog.component.css']
})
export class EditLostDogComponent implements OnInit {
  editLostDogForm = new FormGroup({
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
    behavior: new FormControl('', []),
    locationCity: new FormControl('', [Validators.required]),
    locationDistrict: new FormControl('', [Validators.required]),
  });

  selectedFile!: ImageSnippet;
  isNewPictureChosen: boolean = false;
  url!: any;
  lostDogID!: number;
  lostDog?: LostDogFromBackend;
  dogColors: string[] = DogColorSelector;
  dogEars: string[] = DogEarsSelector;
  dogHair: string[] = DogHairSelector;
  dogSizes: string[] = DogSizeSelector;
  dogTails: string[] = DogTailSelector;

  constructor(
    private router: Router,
    private sanitizer: DomSanitizer,
    private activatedRoute: ActivatedRoute,
    private lostDogService: LostDogService,
    private datepipe: DatePipe) { }

  ngOnInit(): void {
    this.lostDogID = parseInt(this.activatedRoute.snapshot.paramMap.get('dogId')!);
    this.lostDogService.getLostDogByID(this.lostDogID).subscribe(response => {
      this.lostDog = response.data;
      this.url = 'data:' + this.lostDog!.picture!.fileType + ';base64,' + this.lostDog!.picture!.data;
      this.mapLostDogDataIntoForm();
    });
  }

  onSubmit() {
    this.lostDogService.putLostDog(this.constructForm(), this.lostDogID).subscribe(response => {
      this.router.navigate(['/home']);
    });
  }

  private constructForm(): FormData {
    const location = new Location(
      this.editLostDogForm.get('locationCity')?.value,
      this.editLostDogForm.get('locationDistrict')?.value
    );
    const lostDog = new LostDog(
      this.editLostDogForm.get('name')?.value,
      this.editLostDogForm.get('breed')?.value,
      this.editLostDogForm.get('age')?.value,
      this.editLostDogForm.get('size')?.value,
      this.editLostDogForm.get('color')?.value,
      this.editLostDogForm.get('specialMarks')?.value, 
      this.editLostDogForm.get('hairLength')?.value,
      this.editLostDogForm.get('earsType')?.value,
      this.editLostDogForm.get('tailLength')?.value,
      [ this.editLostDogForm.get('behavior')?.value ],
      location,
      this.datepipe.transform(this.editLostDogForm.get('dateLost')?.value, 'yyyy-MM-dd')!
    );
      
    let data = new FormData();
    lostDog.ownerId = localStorage.getItem('userId')!;
    // works only with our Backend
    // data.append('dog', JSON.stringify(lostDog));
    data.append("dog", new Blob([JSON.stringify(lostDog)], { type: "application/json", }), "");
    if (this.isNewPictureChosen) {
      data.append('picture', this.selectedFile.file);
    }
    return data;
  }

  onCancel() {
    this.router.navigate(['/home']);
  }

  onOptionSetChangedHandler(event: MatSelectChange, controlName: string) {
    this.editLostDogForm.get(controlName)?.setValue(event.value);
  }

  processFile(event: any) {
    const file: File = event.target.files[0];
    const reader = new FileReader();

    reader.addEventListener('load', (even: any) => {
      this.selectedFile = new ImageSnippet(even.target.result, file);
      this.isNewPictureChosen = true;
    });
    
    reader.readAsDataURL(file);
    reader.onload = (event: any) => {
      this.url = reader.result;
    }
  }

  mapLostDogDataIntoForm() {
    this.editLostDogForm.get('breed')?.setValue(this.lostDog?.breed);
    this.editLostDogForm.get('age')?.setValue(this.lostDog?.age);
    this.editLostDogForm.get('size')?.setValue(this.lostDog?.size);
    this.editLostDogForm.get('color')?.setValue(this.lostDog?.color);
    this.editLostDogForm.get('name')?.setValue(this.lostDog?.name);
    this.editLostDogForm.get('hairLength')?.setValue(this.lostDog?.hairLength);
    this.editLostDogForm.get('tailLength')?.setValue(this.lostDog?.tailLength);
    this.editLostDogForm.get('earsType')?.setValue(this.lostDog?.earsType);
    this.editLostDogForm.get('specialMarks')?.setValue(this.lostDog?.specialMark);
    this.editLostDogForm.get('behavior')?.setValue(this.lostDog?.behaviors[0]);
    this.editLostDogForm.get('locationCity')?.setValue(this.lostDog?.location.city);
    this.editLostDogForm.get('locationDistrict')?.setValue(this.lostDog?.location.district);
    this.editLostDogForm.get('dateLost')?.setValue(this.lostDog?.dateLost);
  }
}
