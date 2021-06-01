import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterShelterDogComponent } from './register-shelter-dog.component';

describe('RegisterShelterDogComponent', () => {
  let component: RegisterShelterDogComponent;
  let fixture: ComponentFixture<RegisterShelterDogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegisterShelterDogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterShelterDogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
