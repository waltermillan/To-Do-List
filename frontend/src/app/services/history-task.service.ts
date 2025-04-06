import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GLOBAL_CONFIG } from '../config/config.global';

@Injectable({
  providedIn: 'root'
})
export class HistoryTaskService {

  constructor(private http: HttpClient) { }

  getAll(): Observable<any[]> {
    const url = `${GLOBAL_CONFIG.apiBaseUrl}/taskshistory`;
    return this.http.get<any[]>(url);
  }

  add(taskHistory: any): Observable<any> {
    const url = `${GLOBAL_CONFIG.apiBaseUrl}/taskshistory`;
    return this.http.post(url, taskHistory);
  }

  delete(taskHistoryId: number): Observable<any> {
    const url = `${GLOBAL_CONFIG.apiBaseUrl}/taskshistory/${taskHistoryId}`;
    return this.http.delete<any>(url);
  }

  upadate(taskHistory: any, id:number): Observable<any> {
    const url = `${GLOBAL_CONFIG.apiBaseUrl}/taskshistory/${id}`;
    return this.http.put(url, taskHistory);
  }
}
