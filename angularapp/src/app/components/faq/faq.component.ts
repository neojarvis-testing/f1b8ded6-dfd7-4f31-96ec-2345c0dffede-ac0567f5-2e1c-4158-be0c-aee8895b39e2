import { Component } from '@angular/core';

@Component({
  selector: 'app-faq',
  templateUrl: './faq.component.html',
  styleUrls: ['./faq.component.css']
})
export class FaqComponent {
  faqs = [
    {
      question: 'What is BlogPortal?',
      answer: 'BlogPortal is a platform where you can create, view, and share blogs.',
      showAnswer: false
    },
    {
      question: 'How do I create a blog?',
      answer: 'To create a blog, go to the "Create Blog" section and fill out the form.',
      showAnswer: false
    },
    {
      question: 'How can I view announcements?',
      answer: 'You can view announcements in the "View Announcements" section.',
      showAnswer: false
    }
  ];

  toggleAnswer(faq: any) {
    faq.showAnswer = !faq.showAnswer;
  }
}
