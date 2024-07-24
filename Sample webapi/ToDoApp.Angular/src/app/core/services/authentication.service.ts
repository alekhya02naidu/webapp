import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Users } from '../../utils/models/users.model';
import { ApiResponse } from '../../utils/models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private baseUrl = `${environment.apiUrl}/Account`; 

  constructor(private http: HttpClient) {
  }

  register(users: Users): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/SignUp`, users, { headers });
  }

  login(username: string, passwordHash: string): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(`${this.baseUrl}/SignIn`, { username, passwordHash });
  }
}
