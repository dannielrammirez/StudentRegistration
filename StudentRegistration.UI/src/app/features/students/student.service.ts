import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Student } from '@shared/models/student';
import { environment } from '@environments/environment';
import { EnrolledCourseDto } from '@shared/models/enrolled-course-dto';

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  private baseUrl = `${environment.apiUrl}/students`;

  constructor(private http: HttpClient) { }

  getClassmates(): Observable<Student[]> {
    return this.http.get<Student[]>(`${this.baseUrl}/classmates`);
  }

  getStudentEnrollments(studentId: string): Observable<EnrolledCourseDto[]> {
    return this.http.get<EnrolledCourseDto[]>(`${this.baseUrl}/${studentId}/enrollments`);
  }
}