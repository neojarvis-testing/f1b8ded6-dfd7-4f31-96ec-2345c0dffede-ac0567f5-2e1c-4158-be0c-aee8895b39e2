import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { UserViewFeedbackComponent } from './user-view-feedback.component';

describe('UserViewFeedbackComponent', () => {
  let component: UserViewFeedbackComponent;
  let fixture: ComponentFixture<UserViewFeedbackComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, RouterTestingModule, HttpClientTestingModule, FormsModule],
      declarations: [ UserViewFeedbackComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserViewFeedbackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  fit('Frontend_should_create_user_view_feedback_component', () => {
    expect(component).toBeTruthy();
  });

  fit('Frontend_should_contain_my_feedback_heading_in_the_user_view_feedback_component', () => {
    const componentHTML = fixture.debugElement.nativeElement.outerHTML;
    expect(componentHTML).toContain('My Feedback');
  });
});
