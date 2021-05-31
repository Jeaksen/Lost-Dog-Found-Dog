import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LostDogDetailsComponent } from './lost-dog-details.component';

describe('LostDogDetailsComponent', () => {
  let component: LostDogDetailsComponent;
  let fixture: ComponentFixture<LostDogDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LostDogDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LostDogDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
