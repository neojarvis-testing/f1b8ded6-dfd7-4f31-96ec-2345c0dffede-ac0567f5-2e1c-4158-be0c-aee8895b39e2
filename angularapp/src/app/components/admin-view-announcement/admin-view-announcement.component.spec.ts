import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { AdminViewAnnouncementComponent } from './admin-view-announcement.component';

describe('AdminViewAnnouncementComponent', () => {
  let component: AdminViewAnnouncementComponent;
  let fixture: ComponentFixture<AdminViewAnnouncementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, RouterTestingModule, HttpClientTestingModule, FormsModule],
      declarations: [ AdminViewAnnouncementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminViewAnnouncementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  fit('Frontend_should_create_admin_view_announcement_component', () => {
    expect(component).toBeTruthy();
  });

  fit('Frontend_should_contain_announcements_heading_in_the_admin_view_announcement_component', () => {
    const componentHTML = fixture.debugElement.nativeElement.outerHTML;
    expect(componentHTML).toContain('Announcements');
  });
});
