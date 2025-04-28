import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Feedback } from '../models/feedback.model';
import { AuthService } from './auth.service';
import { environment } from 'src/environments/environment';
import { User } from '../models/user.model';
 
@Injectable({
  providedIn: 'root'
})
export class FeedbackService {
  private apiUrl = environment.apiUrl;
 
  constructor(private http: HttpClient, private authService: AuthService) {}
  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({ 'Authorization': `Bearer ${token}` });
  }
  sendFeedback(feedback: Feedback): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/api`, feedback, { headers: this.getHeaders(), responseType: 'text' as 'json' }).pipe(
      catchError(this.handleError)
    );
  }
  getAllFeedbacksByUserId(userId: number): Observable<Feedback[]> {
    return this.http.get<Feedback[]>(`${this.apiUrl}/api/user/${userId}`, { headers: this.getHeaders() }).pipe(
      catchError(this.handleError)
    );
  }
  getUserbyId(): Observable<User[]>{
    return this.http.get<User[]>(`${this.apiUrl}/api/users`, { headers: this.getHeaders() }).pipe(
      catchError(this.handleError));
  }
 
  deleteFeedback(feedbackId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/api/${feedbackId}`, { headers: this.getHeaders(), responseType: 'text' as 'json'  }).pipe(
      catchError(this.handleError)
    );
  }
  getFeedbacks(): Observable<Feedback[]> {
    return this.http.get<Feedback[]>(`${this.apiUrl}/api`, { headers: this.getHeaders() }).pipe(
      catchError(this.handleError)
    );
  }
  private handleError(error: any): Observable<never> {
    console.error('An error occurred:', error);
    return throwError(error);
  }
}