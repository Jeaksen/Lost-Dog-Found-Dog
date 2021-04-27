import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FilterLostDogsComponent } from './filter-lost-dogs.component';

describe('FilterLostDogsComponent', () => {
  let component: FilterLostDogsComponent;
  let fixture: ComponentFixture<FilterLostDogsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FilterLostDogsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FilterLostDogsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
