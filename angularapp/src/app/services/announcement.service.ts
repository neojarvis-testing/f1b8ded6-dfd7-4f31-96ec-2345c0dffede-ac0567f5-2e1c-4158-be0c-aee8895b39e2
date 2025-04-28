import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Announcement } from '../models/announcement.model';
import { environment } from 'src/environments/environment';
 
@Injectable({
  providedIn: 'root'
})
export class AnnouncementService {



  private apiUrl = environment.apiUrl;
 
  constructor(private http: HttpClient) {}
 
  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }
 
  getAllAnnouncements(): Observable<Announcement[]> {
    return this.http.get<Announcement[]>(`${this.apiUrl}/api/announcements`, {
      headers: this.getAuthHeaders(),
    });
  }
 
  getAnnouncementById(id: number): Observable<Announcement> {
    return this.http.get<Announcement>(`${this.apiUrl}/api/Announcements/${id}`, {
      headers: this.getAuthHeaders()
    });
  }
 
  addAnnouncement(announcement: Announcement): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/api/announcements`, announcement, {
      headers: this.getAuthHeaders(), responseType: 'text' as 'json'
    });
  }
 
  updateAnnouncement(id: number, announcement: Announcement): Observable<any> {
    return this.http.put(`${this.apiUrl}/api/announcements/${id}`, announcement, {
      headers: this.getAuthHeaders(), responseType: 'text' as 'json'
    });
  }
 
  deleteAnnouncement(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/api/announcements/${id}`, {
      headers: this.getAuthHeaders(), responseType: 'text' as 'json'
    });
  }
}