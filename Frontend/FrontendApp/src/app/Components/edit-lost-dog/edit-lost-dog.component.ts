import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSelectChange } from '@angular/material/select';
import { DomSanitizer } from '@angular/platform-browser';
import { Router, ActivatedRoute } from '@angular/router';
import { LostDogService } from 'src/app/services/lost-dog-service';
import { ImageSnippet } from '../../models/image-snippet';
import { LostDog } from '../../models/lost-dog';
 
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
    behaviour: new FormControl('', []),
    locationCity: new FormControl('', [Validators.required]),
    locationDistrict: new FormControl('', [Validators.required]),
  });

  selectedFile!: ImageSnippet;
  isNewPictureChosen: boolean = false;
  url!: any;
  lostDogID!: number;
  lostDog?: LostDog;
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
      this.url = 'data:' + this.lostDog!.picture.fileType + ';base64,' + this.lostDog!.picture.data;
      this.editLostDogForm.get("behaviour")?.setValue("Deppression");
    });
  }

  onSubmit() {
    console.log(this.editLostDogForm);
    this.lostDogService.putLostDog(this.constructForm(), this.lostDogID).subscribe(response => console.log(response));
    this.router.navigate(['/home']);
  }

  private constructForm(): FormData {
    let data = new FormData();

    // u better not change the order
    data.append('breed', this.editLostDogForm.get('breed')?.value);
    //data.append('breed', 'Doo');
    data.append('age', this.editLostDogForm.get('age')?.value);
    //data.append('age', '18');
    data.append('size', this.editLostDogForm.get('size')?.value);
    //data.append('size', 'Enormous');
    data.append('color', this.editLostDogForm.get('color')?.value);
    //data.append('color', 'Black');
    data.append('specialMark', 'costam');
    //data.append('specialMark', 'Lacks one leg');
    data.append('name', this.editLostDogForm.get('name')?.value);
    //data.append('name', 'Scooby');
    data.append('hairLength', this.editLostDogForm.get('hairLength')?.value);
    //data.append('hairLength', 'Long');
    data.append('tailLength', this.editLostDogForm.get('tailLength')?.value);
    //data.append('tailLength', 'Short');
    data.append('earsType', this.editLostDogForm.get('earsType')?.value);
    //data.append('earsType', 'He doesn\'t have'); 
    data.append('behaviors', 'Depression');
    data.append('behaviors', 'Prosze zaakceptuj to');
    data.append('location.City', this.editLostDogForm.get('locationCity')?.value);
    //data.append('location.City', 'Chrzęszczyszczeborzyce');
    data.append('location.District', this.editLostDogForm.get('locationDistrict')?.value);
    //data.append('location.District', 'Łękoboldy');
    data.append('dateLost', this.datepipe.transform(this.editLostDogForm.get('dateLost')?.value, 'yyyy-MM-dd')!);
    //data.append('dateLost', '2021-03-23');
    data.append('ownerId', localStorage.getItem('userId')!);
    if (this.isNewPictureChosen) {
      data.append('picture', this.selectedFile.file);
    }
    console.log(data.forEach(val => console.log(val)));
    //console.log(data.get('image'));
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
      console.log(file);
      this.selectedFile = new ImageSnippet(even.target.result, file);
      this.isNewPictureChosen = true;
    });
    
    reader.readAsDataURL(file);
    reader.onload = (event:any) => {
      this.url = reader.result;
      //console.log(reader.result);
    }
  }

}
