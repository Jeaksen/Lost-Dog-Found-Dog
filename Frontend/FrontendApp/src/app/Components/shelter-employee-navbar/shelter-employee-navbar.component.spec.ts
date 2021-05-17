import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShelterEmployeeNavbarComponent } from './shelter-employee-navbar.component';

describe('ShelterEmployeeNavbarComponent', () => {
  let component: ShelterEmployeeNavbarComponent;
  let fixture: ComponentFixture<ShelterEmployeeNavbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShelterEmployeeNavbarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShelterEmployeeNavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
