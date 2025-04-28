import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    console.log('AuthGuard invoked');
    
    // Check if the user is logged in
    if (!this.authService.isLoggedIn()) {
      console.log('User not logged in, redirecting to login');
      this.router.navigate(['/login'], { queryParams: { message: 'User not logged in' } });
      return false;
    }

    // Retrieve user's role
    const userRole = this.authService.getUserRole();
    const requiredRole = route.data['role']; // Fetch expected role from route metadata
    const targetUrl = state.url;
    console.log(`User role: ${userRole}, Required role: ${requiredRole}, Target URL: ${targetUrl}`);

    // Handle missing required role metadata
    if (!requiredRole) {
      console.log('No role defined for this route. Access Denied.');
      this.router.navigate(['/error'], { queryParams: { message: 'Access Denied: No role defined' } });
      return false;
    }

    // Role-based access logic
    if (userRole !== requiredRole) {
      console.log('Access Denied: Role mismatch');
      this.router.navigate(['/error'], { queryParams: { message: 'Access Denied: Role mismatch' } });
      return false;
    }

    console.log('Access Granted');
    return true;
  }
}