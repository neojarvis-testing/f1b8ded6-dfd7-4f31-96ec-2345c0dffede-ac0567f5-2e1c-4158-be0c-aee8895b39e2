import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})
export class ErrorComponent implements OnInit {
  errorMessage: string = 'Something Went Wrong';


  constructor(private route:ActivatedRoute) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const errorCode = params['code'];
      this.setErrorMessage(errorCode);
    });
  }
  //error
  setErrorMessage(code: string): void {
    switch (code) {
      case '404':
        this.errorMessage = 'The page you are looking for might have been removed, had its name changed, or is temporarily unavailable.';
        break;
      case '500':
        this.errorMessage = 'There was an internal server error. Please try again later or contact support if the problem persists.';
        break;
      default:
        this.errorMessage = 'An unexpected error occurred. Please try again later.';
    }
  }

}
