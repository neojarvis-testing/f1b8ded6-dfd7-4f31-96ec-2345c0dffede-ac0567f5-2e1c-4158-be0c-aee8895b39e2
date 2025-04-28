import { Component, OnInit } from '@angular/core';
import { BlogPost } from 'src/app/models/blog-post.model';
import { BlogPostService } from 'src/app/services/blog-post.service';

@Component({
  selector: 'app-admin-view-blog',
  templateUrl: './admin-view-blog.component.html',
  styleUrls: ['./admin-view-blog.component.css']
})
export class AdminViewBlogComponent implements OnInit {

  blogPosts: BlogPost[] = [];
  filteredBlogPosts: BlogPost[] = [];
  currentPage: number = 1;
  itemsPerPage: number = 5;

  constructor(private blogService: BlogPostService) {}

  ngOnInit(): void {
    this.loadBlogPosts();
  }

  loadBlogPosts(): void {
    this.blogService.getAllBlogPosts().subscribe(data => {
      this.blogPosts = data;
      this.filteredBlogPosts = data;
    });
  }

  statusApproved(blog: BlogPost): void {
    blog.Status = "Approved";
    this.blogService.updateBlogPost(blog.BlogPostId, blog).subscribe(() => {
      this.loadBlogPosts();
    });
  }

  statusRejected(blog: BlogPost): void {
    blog.Status = "Rejected";
    this.blogService.updateBlogPost(blog.BlogPostId, blog).subscribe(() => {
      this.loadBlogPosts();
    });
  }

  // Pagination methods
  get paginatedBlogPosts(): BlogPost[] {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    return this.filteredBlogPosts.slice(startIndex, startIndex + this.itemsPerPage);
  }

  changePage(page: number): void {
    this.currentPage = page;
  }

  get totalPages(): number {
    return Math.ceil(this.filteredBlogPosts.length / this.itemsPerPage);
  }
}
