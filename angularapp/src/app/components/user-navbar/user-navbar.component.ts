import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-user-navbar',
  templateUrl: './user-navbar.component.html',
  styleUrls: ['./user-navbar.component.css']
})
export class UserNavbarComponent implements OnInit {
  isAdmin: boolean = false; // Set to false for user role
  Username: string = ''; // Placeholder for the actual username
  role: string = this.isAdmin ? 'Admin' : 'User'; // Role based on user status
  showLogoutModel: boolean = false; // To manage logout confirmation modal visibility
  showProfileDropdown: boolean = false;
  userProfile: any = {
    Username: '',
    Email: '',
    MobileNumber: '',
    UserRole: ''
  };
  showMenu: boolean = false; // Toggles hamburger menu visibility

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.Username = localStorage.getItem('userName') || 'Guest'; // Assuming username is stored in localStorage

    // Fetch user data from localStorage
    this.userProfile.Username = this.authService.getUserName();
    this.userProfile.Email = this.authService.getEmailAddress();
    this.userProfile.MobileNumber = '1234567890';
    this.userProfile.UserRole = this.authService.getUserRole();
  }

  logout(): void {
    this.showLogoutModel = true; // Show the logout confirmation modal
  }

  confirmLogout(): void {
    this.showLogoutModel = false; // Hide the modal
    this.authService.logout(); // Call the logout service
    this.router.navigate(['/login']); // Navigate to login page
  }

  cancelLogout(): void {
    this.showLogoutModel = false; // Hide the modal
  }

  toggleDropdown(event: Event): void {
    event.preventDefault();
    const dropdown = (event.currentTarget as HTMLElement).closest('.dropdown');
    if (dropdown) {
      dropdown.classList.toggle('show'); // Toggle dropdown visibility
    }
  }
  profileClick(){
    this.router.navigate([`/userprofile`]);
  }

  toggleProfileDropdown(): void {
    this.showProfileDropdown = !this.showProfileDropdown;
  }

  closeProfileDropdown(): void {
    this.showProfileDropdown = false;
  }
  
  toggleMenu(): void {
    this.showMenu = !this.showMenu; // Toggle hamburger menu visibility
  }

  closeMenu(): void {
    this.showMenu = false; // Close the menu
  }
}