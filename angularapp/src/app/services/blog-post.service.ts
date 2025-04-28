import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BlogPost } from '../models/blog-post.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BlogPostService {

  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  getAllBlogPosts(): Observable<BlogPost[]> {
    return this.http.get<BlogPost[]>(`${this.baseUrl}/api/blogposts`, { headers: this.getAuthHeaders() });
  }

  getBlogPostById(id: number): Observable<BlogPost> {
    return this.http.get<BlogPost>(`${this.baseUrl}/api/blogposts/${id}`, { headers: this.getAuthHeaders()});
  }

  addBlogPost(blogPost: BlogPost): Observable<any> {
    return this.http.post(`${this.baseUrl}/api/blogposts`, blogPost, { headers: this.getAuthHeaders(), responseType: 'text' as 'json' });
  }

  updateBlogPost(id: number, blogPost: BlogPost): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/blogposts/${id}`, blogPost, { headers: this.getAuthHeaders(), responseType: 'text' as 'json' });
  }

  deleteBlogPost(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/api/blogposts/${id}`, { headers: this.getAuthHeaders(), responseType: 'text' as 'json' });
  }
}