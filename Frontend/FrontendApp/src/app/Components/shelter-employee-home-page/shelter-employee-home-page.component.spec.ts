import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShelterEmployeeHomePageComponent } from './shelter-employee-home-page.component';

describe('ShelterEmployeeHomePageComponent', () => {
  let component: ShelterEmployeeHomePageComponent;
  let fixture: ComponentFixture<ShelterEmployeeHomePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShelterEmployeeHomePageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShelterEmployeeHomePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
