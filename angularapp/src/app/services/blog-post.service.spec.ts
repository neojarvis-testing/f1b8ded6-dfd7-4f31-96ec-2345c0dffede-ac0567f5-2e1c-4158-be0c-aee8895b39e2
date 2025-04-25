import { TestBed } from '@angular/core/testing';

import { BlogPostService } from './blog-post.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('BlogPostService', () => {
  let service: BlogPostService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule ],

    });
    service = TestBed.inject(BlogPostService);
  });

  fit('Frontend_should_create_blog_post_service', () => {
    expect(service).toBeTruthy();
  });
});
