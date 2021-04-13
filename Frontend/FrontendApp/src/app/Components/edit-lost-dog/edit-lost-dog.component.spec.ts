import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditLostDogComponent } from './edit-lost-dog.component';

describe('EditLostDogComponent', () => {
  let component: EditLostDogComponent;
  let fixture: ComponentFixture<EditLostDogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditLostDogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditLostDogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
