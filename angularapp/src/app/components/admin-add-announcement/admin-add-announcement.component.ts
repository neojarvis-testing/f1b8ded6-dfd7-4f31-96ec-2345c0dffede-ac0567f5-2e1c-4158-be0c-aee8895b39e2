import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AnnouncementService } from 'src/app/services/announcement.service';
import { Announcement } from 'src/app/models/announcement.model';

@Component({
  selector: 'app-admin-add-announcement',
  templateUrl: './admin-add-announcement.component.html',
  styleUrls: ['./admin-add-announcement.component.css']
})
export class AdminAddAnnouncementComponent implements OnInit {
  announcement: Announcement = {
    Title: '',
    Content: '',
    Category: '',
    Priority: '',
    Status: '',
    PublishedDate: new Date()
  };

  isEditMode: boolean = false; //Flag to determine edit or add mode 
  announcementId: number | null = null;
  successMessage = '';
  errorMessage = '';

  priorities: string[] = ['High', 'Medium', 'Low'];
  statuses: string[] = ['Active', 'Inactive'];

  constructor(
    private route: ActivatedRoute,
    private announcementService: AnnouncementService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['id']) {
        this.announcementId = +params['id'];
        this.isEditMode = true;
        this.loadAnnouncement(this.announcementId);
      }
    });
  }

  loadAnnouncement(id: number): void {
    this.announcementService.getAnnouncementById(id).subscribe({
      next: (data) => {
        this.announcement = data;
      },
      error: () => {
        this.errorMessage = 'Failed to load announcement';
      }
    });
  }

  onSubmit(form: NgForm): void {
    if (form.invalid) {
      this.errorMessage = 'All fields are required';
      return;
    }

    // Trim whitespace from all input fields
    this.announcement.Title = this.announcement.Title.trim();
    this.announcement.Content = this.announcement.Content.trim();
    this.announcement.Category = this.announcement.Category.trim();
    this.announcement.Priority = this.announcement.Priority.trim();
    this.announcement.Status = this.announcement.Status.trim();

    // Check if any required field is empty after trimming
    if (
      !this.announcement.Title ||
      !this.announcement.Content ||
      !this.announcement.Category ||
      !this.announcement.Priority ||
      !this.announcement.Status
    ) {
      this.errorMessage = 'All fields are required';
      return;
    }

    this.announcement.PublishedDate = new Date();

    if (this.isEditMode && this.announcementId !== null) {
      this.announcementService.updateAnnouncement(this.announcementId, this.announcement).subscribe({
        next: () => {
          this.successMessage = 'Announcement Updated Successfully!';
          this.router.navigate(['/adminviewannouncement']);
          this.errorMessage = '';
        },
        error: (err) => this.handleError(err)
      });
    } else {
      this.announcementService.addAnnouncement(this.announcement).subscribe({
        next: () => {
          this.successMessage = 'Announcement Added Successfully!';
          this.router.navigate(['/adminviewannouncement']);
          this.errorMessage = '';
          form.resetForm();
        },
        error: (err) => this.handleError(err)
      });
    }
  }

  private handleError(err: any): void {
    if (err.error && typeof err.error === 'string' && err.error.includes('Title already exists')) {
      this.errorMessage = 'Title already exists';
    } else {
      this.errorMessage = 'Something went wrong. Please try again.';
    }
  }
}

