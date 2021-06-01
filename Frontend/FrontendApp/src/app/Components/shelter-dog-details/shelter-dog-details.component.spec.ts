import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShelterDogDetailsComponent } from './shelter-dog-details.component';

describe('ShelterDogDetailsComponent', () => {
  let component: ShelterDogDetailsComponent;
  let fixture: ComponentFixture<ShelterDogDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShelterDogDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShelterDogDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
