import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SeeSheltersListComponent } from './see-shelters-list.component';

describe('SeeSheltersListComponent', () => {
  let component: SeeSheltersListComponent;
  let fixture: ComponentFixture<SeeSheltersListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SeeSheltersListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SeeSheltersListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
