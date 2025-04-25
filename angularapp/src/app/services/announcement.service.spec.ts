import { TestBed } from '@angular/core/testing';

import { AnnouncementService } from './announcement.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('AnnouncementService', () => {
  let service: AnnouncementService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule ],

    });
    service = TestBed.inject(AnnouncementService);
  });

  fit('Frontend_should_create_announcement_service', () => {
    expect(service).toBeTruthy();
  });
});
