import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ErrorComponent } from './components/error/error.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { AdminAddAnnouncementComponent } from './components/admin-add-announcement/admin-add-announcement.component';
import { AdminNavbarComponent } from './components/admin-navbar/admin-navbar.component';
import { AdminViewAnnouncementComponent } from './components/admin-view-announcement/admin-view-announcement.component';
import { AdminViewBlogComponent } from './components/admin-view-blog/admin-view-blog.component';
import { AdminViewFeedbackComponent } from './components/admin-view-feedback/admin-view-feedback.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { UserAddBlogComponent } from './components/user-add-blog/user-add-blog.component';
import { UserAddFeedbackComponent } from './components/user-add-feedback/user-add-feedback.component';
import { UserNavbarComponent } from './components/user-navbar/user-navbar.component';
import { UserViewAnnouncementComponent } from './components/user-view-announcement/user-view-announcement.component';
import { UserViewBlogComponent } from './components/user-view-blog/user-view-blog.component';
import { UserViewFeedbackComponent } from './components/user-view-feedback/user-view-feedback.component';
import { AuthGuard } from './components/authguard/auth.guard';
// import { UserProfileComponent } from './components/user-profile/user-profile.component';
// import { FaqComponent } from './components/faq/faq.component';
// import { FooterComponent } from './components/footer/footer.component';



const routes: Routes = [
  {path:'home',component:HomeComponent},
  {path:'login',component:LoginComponent},
  {path:'adminaddannouncement',component:AdminAddAnnouncementComponent,canActivate:[AuthGuard], data:{role : 'Admin'}},
  {path:'adminnavbar',component:AdminNavbarComponent,canActivate:[AuthGuard], data:{role : 'Admin'}},
  {path:'adminviewannouncement',component:AdminViewAnnouncementComponent,canActivate:[AuthGuard], data:{role : 'Admin'}},
  {path:'admineditannouncement',component:AdminViewAnnouncementComponent,canActivate:[AuthGuard], data:{role : 'Admin'}},
  {path:'admindeleteannouncement',component:AdminViewAnnouncementComponent,canActivate:[AuthGuard], data:{role : 'Admin'}},
  {path:'adminviewblog',component:AdminViewBlogComponent,canActivate:[AuthGuard], data:{role : 'Admin'}},
  {path:'adminupdateblog',component:AdminViewBlogComponent,canActivate:[AuthGuard], data:{role : 'Admin'}},
  {path:'adminviewfeedback',component:AdminViewFeedbackComponent,canActivate:[AuthGuard], data:{role : 'Admin'}},
  {path:'register',component:RegistrationComponent},
  {path:'useraddblog',component:UserAddBlogComponent,canActivate:[AuthGuard], data: {role: 'User'}},
  {path:'useraddfeedback',component:UserAddFeedbackComponent,canActivate:[AuthGuard], data: {role: 'User'}},
  {path:'userdeletefeedback',component:UserViewFeedbackComponent,canActivate:[AuthGuard], data:{role:'User'}},
  {path:'usernavbar',component:UserNavbarComponent,canActivate:[AuthGuard], data: {role: 'User'}},
  {path:'userviewannouncement',component:UserViewAnnouncementComponent,canActivate:[AuthGuard], data: {role: 'User'}},
  {path:'userviewblog',component:UserViewBlogComponent,canActivate:[AuthGuard], data: {role: 'User'}},
  // {path:'userprofile',component:UserProfileComponent,canActivate:[AuthGuard], data: {role: 'User'}},
  {path:'usereditblog',component:UserViewBlogComponent,canActivate:[AuthGuard],data:{role:'User'}},
  {path:'userdeleteblog',component:UserViewBlogComponent,canActivate:[AuthGuard],data:{role:'User'}},
  {path:'userviewfeedback',component:UserViewFeedbackComponent,canActivate:[AuthGuard], data: {role: 'User'}},
  // {path:'footer',component:FooterComponent},
  // {path:'faq',component:FaqComponent},
  { path: 'error', component: ErrorComponent },
  // { path: '**', redirectTo: '/error?code=404' },
  {path:'', redirectTo:'home', pathMatch:'full'}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }