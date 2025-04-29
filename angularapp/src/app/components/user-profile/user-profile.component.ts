import { Component, OnInit } from '@angular/core';
import { BlogPostService } from 'src/app/services/blog-post.service';
import { BlogPost } from 'src/app/models/blog-post.model';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  userProfile: any = {
    Username: '',
    Email: '',
    MobileNumber: '',
    UserRole: ''
  };

  userBlogs: BlogPost[] = [];
  showDeletePopup: boolean = false;
  blogToDelete: BlogPost | null = null;

  constructor(
    private blogPostService: BlogPostService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadUserProfile();
    this.loadUserBlogs();
  }

  loadUserProfile(): void {
    this.userProfile.Username = this.authService.getUserName();
    this.userProfile.Email = this.authService.getEmailAddress();
    this.userProfile.MobileNumber = '1234567890';
    this.userProfile.UserRole = this.authService.getUserRole();
  }

  loadUserBlogs(): void {
    this.blogPostService.getAllBlogPosts().subscribe((blogs: BlogPost[]) => {
      const userId = this.authService.getUserId(); // Make sure this function exists in authService
      this.userBlogs = blogs.filter(blog => blog.UserId === Number(userId));
    });
  }

  editBlog(blogId: number): void {
    this.router.navigate(['/useraddblog'], { queryParams: { id: blogId } });
  }

  confirmDelete(blog: BlogPost): void {
    this.blogToDelete = blog;
    this.showDeletePopup = true;
  }

  deleteBlog(): void {
    if (this.blogToDelete) {
      this.blogPostService.deleteBlogPost(this.blogToDelete.BlogPostId!).subscribe(() => {
        this.loadUserBlogs();
        this.showDeletePopup = false;
        this.blogToDelete = null;
      });
    }
  }

  cancelDelete(): void {
    this.showDeletePopup = false;
    this.blogToDelete = null;
  }
}
