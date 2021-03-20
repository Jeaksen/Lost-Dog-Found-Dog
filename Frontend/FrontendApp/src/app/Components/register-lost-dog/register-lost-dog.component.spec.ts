import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterLostDogComponent } from './register-lost-dog.component';

describe('RegisterLostDogComponent', () => {
  let component: RegisterLostDogComponent;
  let fixture: ComponentFixture<RegisterLostDogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegisterLostDogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterLostDogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
