import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { Address } from 'src/app/models/address';
import { Shelter } from 'src/app/models/shelter';
import { SheltersService } from 'src/app/services/shelters-service';

@Component({
  selector: 'app-shelter-register',
  templateUrl: './shelter-register.component.html',
  styleUrls: ['./shelter-register.component.css']
})
export class ShelterRegisterComponent implements OnInit {

  registerShelterForm = this.fb.group({
    name: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    phoneNumber: ['', [Validators.required, Validators.pattern('^\\d{9}$')]],
    city: ['', [Validators.required]],
    street: ['', [Validators.required]],
    postCode: ['', [Validators.required]],
    buildingNumber: ['', [Validators.required]],
    additionalAddressLine: ['', [Validators.required]]
  });

  isRegistering: boolean = true;
  successfullyRegistered: boolean = false;
  unsuccessfullyRegistered: boolean = false;
  message: string = ''

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private sheltersService: SheltersService) { }

  ngOnInit(): void {
  }

  private constructRegisterForm(): FormData {
    const address = new Address(
      this.registerShelterForm.get('city')?.value,
      this.registerShelterForm.get('street')?.value,
      this.registerShelterForm.get('postCode')?.value,
      this.registerShelterForm.get('buildingNumber')?.value,
      this.registerShelterForm.get('additionalAddressLine')?.value
    );
    const shelter = new Shelter(
      this.registerShelterForm.get('name')?.value,
      address,
      this.registerShelterForm.get('phoneNumber')?.value,
      this.registerShelterForm.get('email')?.value
    )
    let data = new FormData();
    data.append('shelter', JSON.stringify(shelter));
    return data;
  }

  onSubmit() {
    this.sheltersService.registerNewShelter(this.constructRegisterForm())
      .pipe(first())
      .subscribe(
        data => {
          if (data.successful) {
            this.successfullyRegistered = true;
            this.isRegistering = false;
          }
          else {
            this.unsuccessfullyRegistered = true;
            this.isRegistering = false;
            this.message = data.message;
          }
        },
        error => {

        });
  }

  onBackClick() {
    this.router.navigate(['/login']);
  }
}
