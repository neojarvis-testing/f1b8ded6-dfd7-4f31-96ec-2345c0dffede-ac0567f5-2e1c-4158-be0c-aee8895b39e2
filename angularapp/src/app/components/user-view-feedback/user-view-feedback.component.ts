import { Component, OnInit } from '@angular/core';
import { Feedback } from 'src/app/models/feedback.model';
import { AuthService } from 'src/app/services/auth.service';
import { FeedbackService } from 'src/app/services/feedback.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-user-view-feedback',
  templateUrl: './user-view-feedback.component.html',
  styleUrls: ['./user-view-feedback.component.css']
})
export class UserViewFeedbackComponent implements OnInit {
  feedbacks: Feedback[] = [];
  selectedFeedback: Feedback | null = null;
  showDeleteModal: boolean = false;
  showLogoutModal: boolean = false;
  errorMessage: string = '';
  currentPage = 1;
  itemsPerPage = 5;

  constructor(private feedbackService: FeedbackService, private authService: AuthService, private router: Router) {}

  // This is an angular hook and it executed when there is any changes in input fields .
  ngOnInit(): void {
    this.loadFeedbacks();
    // this function will load when component is loaded 
  }

  loadFeedbacks(): void {
    const userId = parseInt(localStorage.getItem('userId') || '0');
    if (userId) {
      this.feedbackService.getAllFeedbacksByUserId(userId).subscribe(
        (data) => {
          this.feedbacks = data;
          if (this.feedbacks.length === 0) {
            this.errorMessage = 'No data found';
          }
        },
        (error) => {
          console.error('Error fetching feedbacks:', error);
          this.errorMessage = 'No Feedbacks Found.';
        }
      );
    }
  }

  confirmDelete(feedback: Feedback): void {
    this.selectedFeedback = feedback;
    this.showDeleteModal = true;
  }

  deleteFeedback(): void {
    if (this.selectedFeedback) {
      this.feedbackService.deleteFeedback(this.selectedFeedback.FeedbackId!).subscribe(
        () => {
          this.showDeleteModal = false;
          this.loadFeedbacks();
         
          Swal.fire({
            title: 'Feedback Deleted',
            text: 'The feedback has been successfully deleted!',
            icon: 'success',
            confirmButtonText: 'OK'
          });
          this.router.navigate(['/userviewfeedback']);
        },
        (error) => {
          console.error('Error deleting feedback:', error);
          this.errorMessage = 'Failed to delete feedback.';
        }
      );
    }
  }

  logout(): void {
    this.showLogoutModal = true;
  }

  confirmLogout(): void {
    this.showLogoutModal = false;
    localStorage.clear();
    this.router.navigate(['/login']);
  }

  cancelLogout(): void {
    this.showLogoutModal = false;
  }

  get paginatedFeedbacks() {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    return this.feedbacks.slice(startIndex, endIndex);
  }

  get totalPages() {
    return Math.ceil(this.feedbacks.length / this.itemsPerPage);
  }

  changePage(page: number) {
    this.currentPage = page;
  }
}
