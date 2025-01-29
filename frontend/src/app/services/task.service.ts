import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  private apiUrl = 'http://localhost:5184/api/Task/GetAll';  // URL para obtener todas las tareas
  private addTaskUrl = 'http://localhost:5184/api/Task/Add';  // URL para agregar tareas
  private deleteTaskUrl = 'http://localhost:5184/api/Task/Delete';  // URL para eliminar tareas
  private updateTaskUrl = 'http://localhost:5184/api/Task/Update';

  constructor(private http: HttpClient) { }

  // Método para obtener todas las tareas
  getAllTasks(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  // Método para agregar una nueva tarea
  addTask(task: { name: string, stateId: number, initialDate: string, finishDate: string }): Observable<any> {
    return this.http.post<any>(this.addTaskUrl, task);
  }

  // Método para eliminar una tarea
  deleteTask(taskId: number): Observable<any> {
    return this.http.delete<any>(`${this.deleteTaskUrl}/${taskId}`);
  }

  // Método para actualizar una tarea
  updateTask(task: any): Observable<any> {
    return this.http.put(this.updateTaskUrl, task);
  }
}
