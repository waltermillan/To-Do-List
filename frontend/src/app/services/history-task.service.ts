import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HistoryTaskService {
  private apiUrl = 'http://localhost:5184/api';

  constructor(private http: HttpClient) { }

  archiveTask(taskHistory: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/TasksHistory`, taskHistory);
  }
}
