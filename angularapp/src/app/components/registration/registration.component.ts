import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from 'src/environments/environment';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  // Registration form fields
  username: string = '';
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  mobileNumber: string = '';
  userRole: string = '';
  adminCode: string = ''; // New field for admin code

  // OTP state
  otpSent: boolean = false;
  otp: string = '';

  // For password visibility toggling
  passwordFieldType: string = 'password';
  confirmPasswordFieldType: string = 'password';

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {}

  // Trigger registration which sends an OTP.
  onRegister(): void {
    if (this.password !== this.confirmPassword) {
      Swal.fire('Error', 'Passwords do not match', 'error');
      return;
    }

    if (this.userRole === 'Admin') {
      const correctAdminCode = environment.Akey; // Hardc oded admin code
      if (this.adminCode !== correctAdminCode) {
        Swal.fire('Error', 'Invalid admin code', 'error');
        return;
      }
    }
    

    // Normalize email to ensure consistency
    const normalizedEmail = this.email.trim().toLowerCase();

    // Prepare the registration data using normalized email.
    const registrationData = {
      Username: this.username,
      Email: normalizedEmail,
      Password: this.password,
      MobileNumber: this.mobileNumber,
      UserRole: this.userRole,
      AdminCode: this.adminCode // Include admin code in registration data------
    };

    console.log('Submitting registration data:', registrationData);

    this.authService.register(registrationData).subscribe({
      next: (response: any) => {
        Swal.fire('Success', response.Message, 'success');
        this.otpSent = true;
      },
      error: (err: any) => {
        let errorMsg = 'Registration failed. Please try again.';
        if (err.error && err.error.Message) {
          errorMsg = err.error.Message;
        }
        Swal.fire('Error', errorMsg, 'error');
      }
    });
  }

  // Verify OTP to complete registration--------
  onVerifyOtp(): void {
    // Normalize email to ensure that the same value is used for verification.
    const normalizedEmail = this.email.trim().toLowerCase();
    const otpPayload = { email: normalizedEmail, otp: this.otp }; // Using lower-case keys

    console.log('Verifying OTP with payload:', otpPayload);

    this.authService.verifyRegistrationOtp(otpPayload).subscribe({
      next: (response: any) => {
        Swal.fire('Success', response.Message, 'success').then(() => {
          this.router.navigate(['/login']);
        });
      },
      error: (err: any) => {
        let errorMsg = 'OTP verification failed. Please try again.';
        if (err.error && err.error.Message) {
          errorMsg = err.error.Message;
        }
        Swal.fire('Error', errorMsg, 'error');
      }
    });
  }

  // Toggle password or confirm password visibility.
  togglePasswordVisibility(field: string): void {
    if (field === 'password') {
      this.passwordFieldType = this.passwordFieldType === 'password' ? 'text' : 'password';
    } else if (field === 'confirmPassword') {
      this.confirmPasswordFieldType = this.confirmPasswordFieldType === 'password' ? 'text' : 'password';
    }
  }
}
