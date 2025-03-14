import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  private apiUrl = 'http://localhost:5184/api/tasks/';

  constructor(private http: HttpClient) { }

  getAllTasks(): Observable<any[]> {
    const url = this.apiUrl;
    return this.http.get<any[]>(url);
  }

  addTask(task: { name: string, stateId: number, initialDate: string, finishDate: string }): Observable<any> {
    const url = this.apiUrl;
    return this.http.post<any>(url, task);
  }

  deleteTask(taskId: number): Observable<any> {
    const url = `${this.apiUrl }${taskId}`;
    return this.http.delete<any>(url);
  }

  updateTask(task: any, id:number): Observable<any> {
    const url = this.apiUrl + id;
    return this.http.put(url, task);
  }
}
