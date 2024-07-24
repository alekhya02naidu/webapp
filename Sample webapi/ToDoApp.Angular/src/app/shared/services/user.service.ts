import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Users } from '../../utils/models/users.model';
import { ApiResponse } from '../../utils/models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = `${environment.apiUrl}/User`;

  constructor(private http: HttpClient) { }

  getUserByName(username: string): Observable<ApiResponse<Users>> {
    return this.http.get<ApiResponse<Users>>(`${this.baseUrl}/getUser/${username}`);
  }
}
