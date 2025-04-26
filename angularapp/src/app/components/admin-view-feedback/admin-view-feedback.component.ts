import { Component, OnInit } from '@angular/core';
import { Feedback } from 'src/app/models/feedback.model';

@Component({
  selector: 'app-admin-view-feedback',
  templateUrl: './admin-view-feedback.component.html',
  styleUrls: ['./admin-view-feedback.component.css']
})
export class AdminViewFeedbackComponent implements OnInit {
  feedbacks: Feedback[] = [];
  feedbackUsernames: { [key: number]: string } = {};
  selectedFeedback: Feedback | null = null;
  showProfileModal: boolean = false;
  showLogoutModal: boolean = false;
  errorMessage: string = '';
  Username: string = '';
 
  currentPage: number = 1;
  itemsPerPage: number = 5;

  constructor() { }

  ngOnInit(): void {
  }

}
