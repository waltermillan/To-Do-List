import { Component, OnInit } from '@angular/core';
import { TaskService } from '../services/task.service';  // Importa el servicio TaskService

@Component({
  selector: 'app-archived-task',
  templateUrl: './archived-task.component.html',
  styleUrl: './archived-task.component.css'
})
export class ArchivedTaskComponent implements OnInit {

  tasks: { name: string, completed: boolean, id: number, stateId: number, initialDate: string, finishDate: string }[] = [];
  newTaskName: string = '';  // Variable para almacenar el nombre de la nueva tarea
  startDate: string = '';    // Variable para almacenar la fecha de inicio
  endDate: string = '';      // Variable para almacenar la fecha de fin

  constructor(private taskService: TaskService) { }

  ngOnInit(): void {
    this.loadTasks();
  }

  // Método para cargar las tareas desde la API
  loadTasks(): void {
    this.taskService.getAllTasks().subscribe(
      (data) => {
        this.tasks = data.filter(t => t.stateId == 3);  // Asigna las tareas obtenidas desde la API a la variable 'tasks'
      },
      (error) => {
        console.error('Error al obtener tareas:', error);
      }
    );
  }

  // Método para eliminar una tarea
  deleteTask(taskId: number): void {
    this.taskService.deleteTask(taskId).subscribe(
      (response) => {
        console.log('Tarea eliminada con éxito', response);
        this.loadTasks();  // Recargar las tareas después de eliminar una
      },
      (error) => {
        console.error('Error al eliminar tarea:', error);
      }
    );
  }

  // Función para convertir el stateId en una cadena
  getStateName(stateId: number): string {
    switch (stateId) {
      case 1:
        return 'Pendiente';
      case 2:
        return 'Completada';
      case 3:
        return 'Archivada';
      default:
        return 'Desconocido';  // En caso de que haya un estado desconocido
    }
  }
}
