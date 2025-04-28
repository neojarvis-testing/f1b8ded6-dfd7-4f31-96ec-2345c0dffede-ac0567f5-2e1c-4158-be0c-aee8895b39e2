import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BlogPost } from 'src/app/models/blog-post.model';
import { BlogPostService } from 'src/app/services/blog-post.service';

@Component({
  selector: 'app-user-add-blog',
  templateUrl: './user-add-blog.component.html',
  styleUrls: ['./user-add-blog.component.css']
})
export class UserAddBlogComponent implements OnInit {
  blogPost: BlogPost = {
    UserId: 0,
    Title: '',
    Content: '',
    Status: '',
    PublishedDate: new Date()
  };
  submitted = false;
  errorMessage = '';
  isEditMode = false;
  blogIdToEdit: number | null = null;

  constructor(
    private blogService: BlogPostService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const id = params['id'];
      if (id) {
        this.isEditMode = true;
        this.blogIdToEdit = +id;
        this.blogService.getBlogPostById(this.blogIdToEdit).subscribe({
          next: (data) => {
            this.blogPost = data;
          },
          error: (err) => {
            console.error('Error fetching blog:', err);
          }
        });
      }
    });
  }

  onSubmit(form: any): void {
    this.submitted = true;
    this.errorMessage = '';

    if (form.invalid) return;

    const updatedBlog: BlogPost = {
      ...this.blogPost,
      UserId: parseInt(localStorage.getItem('userId') || '0', 10),
      Title: this.blogPost.Title.trim(),
      Content: this.blogPost.Content.trim(),
      Status: this.isEditMode ? this.blogPost.Status : 'Pending',
      PublishedDate: this.isEditMode ? this.blogPost.PublishedDate : new Date()
    };

    if (this.isEditMode && this.blogIdToEdit !== null) {
      this.blogService.updateBlogPost(this.blogIdToEdit, updatedBlog).subscribe({
        next: () => {
          alert('Blog Post Updated Successfully!');
          this.router.navigate(['/userviewblog']);
        },
        error: () => {
          this.errorMessage = 'An error occurred while updating the blog post';
        }
      });
    } else {
      this.blogService.addBlogPost(updatedBlog).subscribe({
        next: () => {
          alert('Blog Post Added Successfully!');
          this.router.navigate(['/userviewblog']);
        },
        error: (error) => {
          if (error.status === 400 || error.error === 'Blog with this title already exists') {
            this.errorMessage = 'A blog with the title already exists';
          } else {
            this.errorMessage = 'An error occurred while adding the blog post';
          }
        }
      });
    }
  }
}

//corrected