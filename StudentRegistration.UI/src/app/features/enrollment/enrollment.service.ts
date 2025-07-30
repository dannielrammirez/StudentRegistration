import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EnrollmentService {
  private baseUrl = `${environment.apiUrl}/enrollments`;

  constructor(private http: HttpClient) { }

  createEnrollment(courseIds: string[]): Observable<any> {
    const payload = { courseIds };
    return this.http.post(this.baseUrl, payload);
  }
}