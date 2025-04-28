
import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
 // Define the component decorator with its metadata
@Component({ 
  selector: 'app-root',// The component's CSS element selector
  templateUrl: './app.component.html',// The location of the component's template file
  styleUrls: ['./app.component.css']// The location of the component's private CSS 
})

// Define the AppComponent class
export class AppComponent implements OnInit {
  title:'angularapp';// The title property used in the template
  isLoggedIn = false;
  userRole: string | null = null;
 
   // Constructor for the AppComponent class

  constructor(private router: Router) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.userRole = localStorage.getItem('userRole');
        this.isLoggedIn = !!this.userRole;
      }
    });
  }
 // this will initialize the value 
  ngOnInit(): void {
    this.userRole = localStorage.getItem('userRole');
    this.isLoggedIn = !!this.userRole;
  }
}