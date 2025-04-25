import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { AdminViewFeedbackComponent } from './admin-view-feedback.component';

describe('AdminViewFeedbackComponent', () => {
  let component: AdminViewFeedbackComponent;
  let fixture: ComponentFixture<AdminViewFeedbackComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, RouterTestingModule, HttpClientTestingModule, FormsModule],
      declarations: [ AdminViewFeedbackComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminViewFeedbackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
  fit('Frontend_should_create_admin_view_feedback_component', () => {
    expect(component).toBeTruthy();
  });

  fit('Frontend_should_contain_feedback_details_in_the_admin_view_feedback_component', () => {
    const componentHTML = fixture.debugElement.nativeElement.outerHTML;
    expect(componentHTML).toContain('Feedback Details');
  });

});
