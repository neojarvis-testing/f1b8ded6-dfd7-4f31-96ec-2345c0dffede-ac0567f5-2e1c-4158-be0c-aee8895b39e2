import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { FeedbackService } from 'src/app/services/feedback.service';

@Component({
  selector: 'app-admin-navbar',
  templateUrl: './admin-navbar.component.html',
  styleUrls: ['./admin-navbar.component.css']
})
export class AdminNavbarComponent implements OnInit {
  isAdmin: boolean = true; // Set true if the user is an admin
  Username: string = '';   // Placeholder for the actual username
  role: string = this.isAdmin ? 'Admin' : 'User'; // Role based on admin status
  userId: number;          // To store the user ID
  showLogoutModel: boolean = false; // Manages logout modal visibility
  showMenu: boolean = false;        // Toggles hamburger menu visibility
  showProfileDropdown: boolean = false;
  userProfile: any = {
    Username: '',
    Email: '',
    MobileNumber: '',
    UserRole: ''
  };

  constructor(
    private authService: AuthService,
    private router: Router,
    private feedbackService: FeedbackService
  ) {}

  ngOnInit(): void {
    this.userId = parseInt(localStorage.getItem('userId')!, 10);
    this.Username = localStorage.getItem('userName') || 'User';

    // Fetch user data from localStorage
    this.userProfile.Username = this.authService.getUserName();
    this.userProfile.Email = this.authService.getEmailAddress();
    this.userProfile.MobileNumber = '1234567890';
    this.userProfile.UserRole = this.authService.getUserRole();
  }

  logout(): void {
    this.showLogoutModel = true;
  }

  confirmLogout(): void {
    this.showLogoutModel = false;
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  cancelLogout(): void {
    this.showLogoutModel = false;
  }

  toggleDropdown(event: Event): void {
    event.preventDefault();
    const dropdown = (event.currentTarget as HTMLElement).closest('.dropdown');
    if (dropdown) {
      dropdown.classList.toggle('show');
    }
  }
  
  toggleMenu(): void {
    this.showMenu = !this.showMenu;
  }
  
  // New helper method to close the mobile menu when a link is clicked.
  closeMenu(): void {
    this.showMenu = false;
  }

  toggleProfileDropdown(): void {
    this.showProfileDropdown = !this.showProfileDropdown;
  }

  closeProfileDropdown(): void {
    this.showProfileDropdown = false;
  }
}
// done