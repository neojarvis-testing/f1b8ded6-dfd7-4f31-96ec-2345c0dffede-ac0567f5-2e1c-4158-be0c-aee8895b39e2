import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { UserViewBlogComponent } from './user-view-blog.component';

describe('UserViewBlogComponent', () => {
  let component: UserViewBlogComponent;
  let fixture: ComponentFixture<UserViewBlogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, RouterTestingModule, HttpClientTestingModule, FormsModule],
      declarations: [ UserViewBlogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserViewBlogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  fit('Frontend_should_create_user_view_blog_component', () => {
    expect(component).toBeTruthy();
  });

  fit('Frontend_should_contain_blog_posts_heading_in_the_user_view_blog_component', () => {
    const componentHTML = fixture.debugElement.nativeElement.outerHTML;
    expect(componentHTML).toContain('Blog Posts');
  });
});
