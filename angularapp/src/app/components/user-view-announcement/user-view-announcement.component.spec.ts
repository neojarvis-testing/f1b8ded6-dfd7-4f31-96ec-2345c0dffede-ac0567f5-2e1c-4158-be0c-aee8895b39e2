import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { UserViewAnnouncementComponent } from './user-view-announcement.component';

describe('UserViewAnnouncementComponent', () => {
  let component: UserViewAnnouncementComponent;
  let fixture: ComponentFixture<UserViewAnnouncementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, RouterTestingModule, HttpClientTestingModule, FormsModule],

      declarations: [ UserViewAnnouncementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserViewAnnouncementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  fit('Frontend_should_create_user_view_announcement_component', () => {
    expect(component).toBeTruthy();
  });

  fit('Frontend_should_contain_announcements_heading_in_the_user_view_announcement_component', () => {
    const componentHTML = fixture.debugElement.nativeElement.outerHTML;
    expect(componentHTML).toContain('Announcements');
  });
});
