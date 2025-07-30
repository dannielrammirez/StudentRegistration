import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '@environments/environment';
import { Student } from '@shared/models/student';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = `${environment.apiUrl}/Auth`;
  private tokenKey = 'authToken';

  constructor(private http: HttpClient) { }

  register(studentData: Partial<Student>): Observable<any> {
    return this.http.post(`${this.baseUrl}/register`, studentData);
  }

  login(credentials: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/login`, credentials).pipe(
      tap(response => {
        // La respuesta es un objeto, accedemos a su propiedad 'accessToken'
        if (response && response.accessToken) {
          this.saveToken(response.accessToken);
        }
      })
    );
  }

  saveToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
  }
}