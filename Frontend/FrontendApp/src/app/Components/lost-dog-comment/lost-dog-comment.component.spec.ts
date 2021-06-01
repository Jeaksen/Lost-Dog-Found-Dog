import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LostDogCommentComponent } from './lost-dog-comment.component';

describe('LostDogCommentComponent', () => {
  let component: LostDogCommentComponent;
  let fixture: ComponentFixture<LostDogCommentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LostDogCommentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LostDogCommentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
