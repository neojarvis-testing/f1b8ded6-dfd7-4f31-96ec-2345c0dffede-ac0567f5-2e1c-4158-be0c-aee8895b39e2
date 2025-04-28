import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { Login } from 'src/app/models/login.model';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  user: Login = new Login();
  loginError: string | null = null;
  loginSuccess: boolean = false;

  // New properties for OTP verification
  otpVerificationRequired: boolean = false;  // Controls whether to show OTP form
  userOtp: string = '';                      // Holds the OTP entered by the user

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {}

  // Called when the login form is submitted (for email and password).
  login(loginForm: any): void {
    // Validate the form fields.
    if (!loginForm.valid) {
      this.loginError = 'Please fill in all required fields correctly.';
      return;
    }

    // Validate email with our helper.
    if (!this.validateEmail(this.user.Email)) {
      this.loginError = 'Invalid email format';
      return;
    }

    // Validate password. In this example, we require a minimum of 6 characters.
    if (!this.user.Password || this.user.Password.trim().length < 6) {
      this.loginError = 'Password must be at least 6 characters long';
      return;
    }

    // If validations pass, request an OTP for login.
    this.authService.requestLoginOtp(this.user).subscribe(
      response => {
        console.log('OTP sent successfully', response);
        Swal.fire('Success', 'OTP sent to your email', 'success');
        this.loginError = null;
        // Set flag to show the OTP verification form.
        this.otpVerificationRequired = true;
      },
      error => {
        console.error('Error requesting OTP', error);
        // The API error message—such as "Invalid password"—will be passed back in error.error?.Message
        this.loginError = error.error?.Message || 'Failed to send OTP. Please try again.';
      }
    );
  }

  // // Called when the OTP verification form is submitted.
  // verifyOtp(): void {
  //   if (!this.user.Email) {
  //     this.loginError = 'Email is required for OTP verification';
  //     return;
  //   }
  //   if (!this.userOtp) {
  //     this.loginError = 'Please enter the OTP sent to your email';
  //     return;
  //   }
  //   const payload = { email: this.user.Email, otp: this.userOtp }; // Keys in lower-case as expected by the API.
  //   this.authService.verifyLoginOtp(payload).subscribe(
  //     response => {
  //       console.log('OTP verified successfully', response);
  //       Swal.fire('Success', 'Login successful', 'success');
  //       this.loginError = null;
  //       this.loginSuccess = true;
  //       // Redirect to home (or any other desired page).
  //       this.router.navigate(['/home']);
  //     },
  //     error => {
  //       console.error('OTP verification error', error);
  //       this.loginError = error.error?.Message || 'OTP verification failed. Please try again.';
  //     }
  //   );
  // }

  // Resets the login error message.
  resetLoginError(): void {
    this.loginError = null;
    this.loginSuccess = false;
  }

  // Navigates to the registration page.
  register(): void {
    this.router.navigate(['/register']);
  }

  // Validates email format.
  validateEmail(email: string): boolean {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    return emailRegex.test(email);
  }
}
