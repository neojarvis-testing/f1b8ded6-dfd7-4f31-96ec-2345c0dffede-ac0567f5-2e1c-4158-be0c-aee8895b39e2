import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { UserAddFeedbackComponent } from './user-add-feedback.component';

describe('UserAddFeedbackComponent', () => {
  let component: UserAddFeedbackComponent;
  let fixture: ComponentFixture<UserAddFeedbackComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, RouterTestingModule, HttpClientTestingModule, FormsModule],
      declarations: [ UserAddFeedbackComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserAddFeedbackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });


  fit('Frontend_should_create_user_add_feedback_component', () => {
    expect(component).toBeTruthy();
  });
  fit('Frontend_should_contain_add_feedback_heading_in_the_user_add_feedback_component', () => {
    const componentHTML = fixture.debugElement.nativeElement.outerHTML;
    expect(componentHTML).toContain('Add Feedback');
  });
});
