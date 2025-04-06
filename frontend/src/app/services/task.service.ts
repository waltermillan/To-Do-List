import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GLOBAL_CONFIG } from '../config/config.global';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  url: string = ''


  constructor(private http: HttpClient) { 

    this.url = `${GLOBAL_CONFIG.apiBaseUrl}/tasks`;
  }

  getAll(): Observable<any[]> {
    return this.http.get<any[]>(this.url);
  }

  add(task: { name: string, stateId: number, initialDate: string, finishDate: string }): Observable<any> {
    return this.http.post<any>(this.url, task);
  }

  delete(taskId: number): Observable<any> {
    const url = `${this.url}/${taskId}`
    return this.http.delete<any>(url);
  }

  updateTask(task: any, id:number): Observable<any> {
    const url = `${this.url}/${task.id}`
    return this.http.put(url, task);
  }
}
