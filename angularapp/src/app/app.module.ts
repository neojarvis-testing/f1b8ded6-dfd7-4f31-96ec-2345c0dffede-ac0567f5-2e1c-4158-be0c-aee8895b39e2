import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AdminAddAnnouncementComponent } from './components/admin-add-announcement/admin-add-announcement.component';
import { AdminNavbarComponent } from './components/admin-navbar/admin-navbar.component';
import { AdminViewAnnouncementComponent } from './components/admin-view-announcement/admin-view-announcement.component';
import { AdminViewBlogComponent } from './components/admin-view-blog/admin-view-blog.component';
import { AdminViewFeedbackComponent } from './components/admin-view-feedback/admin-view-feedback.component';
import { ErrorComponent } from './components/error/error.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { UserAddBlogComponent } from './components/user-add-blog/user-add-blog.component';
import { UserAddFeedbackComponent } from './components/user-add-feedback/user-add-feedback.component';
import { UserViewAnnouncementComponent } from './components/user-view-announcement/user-view-announcement.component';
import { UserViewBlogComponent } from './components/user-view-blog/user-view-blog.component';
import { UserViewFeedbackComponent } from './components/user-view-feedback/user-view-feedback.component';
import { UserNavbarComponent } from './components/user-navbar/user-navbar.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FooterComponent } from './components/footer/footer.component';
import { ChatComponent } from './components/chat/chat.component';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
import { FaqComponent } from './components/faq/faq.component';


@NgModule({
  declarations: [
    AppComponent,
    AdminAddAnnouncementComponent,
    AdminNavbarComponent,
    AdminViewAnnouncementComponent,
    AdminViewBlogComponent,
    AdminViewFeedbackComponent,
    ErrorComponent,
    HomeComponent,
    LoginComponent,
    NavbarComponent,
    RegistrationComponent,
    UserAddBlogComponent,
    UserAddFeedbackComponent,
    UserViewAnnouncementComponent,
    UserViewBlogComponent,
    UserViewFeedbackComponent,
    UserNavbarComponent,
    FooterComponent,
    ChatComponent,
    UserProfileComponent,
    FaqComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule, 
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
