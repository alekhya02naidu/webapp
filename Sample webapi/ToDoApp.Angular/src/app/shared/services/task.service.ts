import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';
import { constants } from '../../utils/constants/app.contants';
import { Tasks } from '../../utils/models/tasks.model';
import { ApiResponse } from '../../utils/models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private baseUrl: string = `${environment.apiUrl}/Task`;

  constructor(private http: HttpClient) { }
  
  getAllTasks(): Observable<Tasks[]> {
    return this.http.get<Tasks[]>(this.baseUrl);
  }

  addTask(task: Tasks): Observable<ApiResponse<number>> {
    const headers = this.getHeaders();
    return this.http.post<ApiResponse<number>>(this.baseUrl, task, { headers });
  }

  getUserSpecificTask(): Observable<ApiResponse<Tasks[]>> {
    const headers = this.getHeaders();
    return this.http.get<ApiResponse<Tasks[]>>(`${this.baseUrl}/${constants.AUTHTOKEN_KEY}`, { headers });
  }

  updateTask(task: Tasks): Observable<ApiResponse<number>> {
    const headers = this.getHeaders();
    return this.http.put<ApiResponse<number>>(`${this.baseUrl}/${task.id}`, task, { headers });
  }

  deleteTasks(ids: number[]): Observable<ApiResponse<number>> {
    const headers = this.getHeaders();
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/delete-multiple`, ids, { headers });
  }

  private getHeaders(): HttpHeaders {
    const authToken = localStorage.getItem('authToken');
    if (authToken) {
      return new HttpHeaders().set('Authorization', `Bearer ${authToken}`);
    } else {
      return new HttpHeaders();
    }
  }
}
