import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { Login } from '../models/login.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public apiUrl = environment.apiUrl;
  private currentUserRole = new BehaviorSubject<string | null>(null);

  constructor(private http: HttpClient) {
    const token = localStorage.getItem('token');
    if (token) {
      this.currentUserRole.next(this.getUserRoleFromToken(token));
    }
  }

  // ----- Login Flow -----

  // Request login OTP using the user's credentials.
  requestLoginOtp(credentials: Login): Observable<any> {
    // This endpoint sends an OTP to the given email.
    return this.http.post<any>(`${this.apiUrl}/api/login`, credentials);
  }

  // Verify the OTP and, on success, store the token.
  verifyLoginOtp(data: { email: string; otp: string }): Observable<any> {
    return new Observable(observer => {
      this.http.post<any>(`${this.apiUrl}/api/verify-otp`, data).subscribe(
        response => {
          if (response.token) {
            const token = response.token;
            localStorage.setItem('token', token);

            const role = this.getUserRoleFromToken(token);
            const userId = this.getUserIdFromToken(token);
            const userName = this.getUserNameFromToken(token);
            const userEmailAddress = this.getUserEmailFromToken(token);
            // const userMobileNumber = this.getUserMobileFromToken(token);

            localStorage.setItem('userRole', role);
            localStorage.setItem('userId', userId);
            localStorage.setItem('userName', userName);
            localStorage.setItem('userEmailAddress', userEmailAddress);
            // localStorage.setItem('userMobile', userMobileNumber);
            this.currentUserRole.next(role);
          }
          observer.next(response);
          observer.complete();
        },
        error => observer.error(error)
      );
    });
  }

  // ----- Registration Flow -----

  // Call registration endpoint to send an OTP to the provided email.
  register(user: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/api/register`, user, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    });
  }

  // Verify the registration OTP to complete the registration.
  verifyRegistrationOtp(data: { email: string; otp: string }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/api/verify-registration-otp`, data, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    });
  }

  // ----- Auth Helpers -----

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  logout(): void {
    localStorage.clear();
    this.currentUserRole.next(null);
  }

  getUserRole(): string | null {
    return localStorage.getItem('userRole');
  }
  getUserId(){
    return localStorage.getItem('userId');
  }
  getUserName(){
    return localStorage.getItem('userName');
  }
  getEmailAddress(){
    return localStorage.getItem('userEmailAddress');
  }
  // getUserMobileFromToken(token:string){
  //   try {
  //     const payload = JSON.parse(atob(token.split('.')[1]));
  //     return payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilenumber"];
  //   } catch (error) {
  //     console.error("Error decoding token Mobile Number:", error);
  //     return null;
  //   }
  // }

  setUserRole(role: string): void {
    localStorage.setItem('userRole', role);
  }

  getUserEmailFromToken(token: string): string | null {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
    } catch (error) {
      console.error("Error decoding token email address:", error);
      return null;
    }
  }

  getUserRoleFromToken(token: string): string | null {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    } catch (error) {
      console.error("Error decoding token role:", error);
      return null;
    }
  }

  getUserIdFromToken(token: string): string | null {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
    } catch (error) {
      console.error("Error decoding token userId:", error);
      return null;
    }
  }

  getUserNameFromToken(token: string): string | null {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
    } catch (error) {
      console.error("Error decoding token userName:", error);
      return null;
    }
  }

  getCurrentUserRole(): Observable<string | null> {
    return this.currentUserRole.asObservable();
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isAdmin(): boolean {
    return this.getUserRole() === 'Admin';
  }

  isUser(): boolean {
    return this.getUserRole() === 'User';
  }
}
