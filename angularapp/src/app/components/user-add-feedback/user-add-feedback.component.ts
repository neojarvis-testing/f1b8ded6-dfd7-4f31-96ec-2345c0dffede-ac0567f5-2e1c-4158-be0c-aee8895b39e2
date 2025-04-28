import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FeedbackService } from 'src/app/services/feedback.service';
import { AuthService } from 'src/app/services/auth.service';
import { Feedback } from 'src/app/models/feedback.model';
@Component({
  selector: 'app-user-add-feedback',
  templateUrl: './user-add-feedback.component.html',
  styleUrls: ['./user-add-feedback.component.css']
})
export class UserAddFeedbackComponent implements OnInit {
  feedback: Feedback = {
    UserId: 0,
    FeedbackText: '',
    Date: new Date()
  };
  showPopup: boolean = false;
  constructor(
    private feedbackService: FeedbackService,
    private authService: AuthService,
    private router: Router
  ) {}
  ngOnInit(): void {
    const userId = this.authService.getUserIdFromToken(this.authService.getToken()!);
    if (userId) {
      this.feedback.UserId = parseInt(userId, 10);
    }
  }
  onSubmit(feedbackForm: any): void {
    if (!this.feedback.FeedbackText) {
      feedbackForm.form.markAllAsTouched();
      return;
    }
    this.feedbackService.sendFeedback(this.feedback).subscribe(() => {
      this.showPopup = true;
    });
  }
  closePopup(): void {
    this.showPopup = false;
    this.router.navigate(['/userviewfeedback']);
  }
}

