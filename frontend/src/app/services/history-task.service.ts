import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HistoryTaskService {
  private apiUrl = 'http://localhost:5184/api'; // Ajusta la URL base de tu API

  constructor(private http: HttpClient) { }

  // Método para agregar una tarea a TaskHistory
  archiveTask(taskHistory: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/TasksHistory`, taskHistory);
  }
}
