import { Component, OnInit } from '@angular/core';
import { TaskService } from '../services/task.service';  // Importa el servicio TaskService
import { HistoryTaskService } from '../services/history-task.service';  // Importa el servicio HistoryTaskService
import { Task } from '../models/task.model';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.css']
})
export class TodoListComponent implements OnInit {

  tasks:Task[] = [];
  
  newTaskName: string = '';  // Variable para almacenar el nombre de la nueva tarea
  startDate: string = '';    // Variable para almacenar la fecha de inicio
  endDate: string = '';      // Variable para almacenar la fecha de fin
  done: boolean = false;

  constructor(
    private taskService: TaskService, 
    private historyTaskService: HistoryTaskService  // Inyectar el HistoryTaskService
  ) { }

  ngOnInit(): void {
    this.loadTasks();
  }

  // Método para cargar las tareas desde la API
  loadTasks(): void {
    this.taskService.getAllTasks().subscribe(
      (data) => {
        this.tasks = data.filter(t => t.stateId == 1 || t.stateId == 2);  // Asigna las tareas obtenidas desde la API a la variable 'tasks'
      },
      (error) => {
        console.error('Error al obtener tareas:', error);
      }
    );
  }

  // Método para agregar una tarea
  addTask(): void {
    if (!this.newTaskName.trim()) {
      alert('Por favor ingresa un nombre para la tarea.');
      return;
    }

    if (!this.startDate || !this.endDate) {
      alert('Por favor ingresa las fechas de inicio y fin.');
      return;
    }

    if (new Date(this.startDate) > new Date(this.endDate)) {
      alert('La fecha de fin no puede ser anterior a la fecha de inicio.');
      return;
    }

    if (this.newTaskName.trim() && this.startDate && this.endDate) {
      const newTask = {
        name: this.newTaskName,
        stateId: 1,  // Asumimos que el estado es 1 (Pendiente) por defecto
        done: false,
        initialDate: this.startDate,
        finishDate: this.endDate
      };

      this.taskService.addTask(newTask).subscribe({
        next: (data) => {
          console.log('Tarea agregada con éxito', data);
          this.loadTasks();  // Recargar las tareas después de agregar una nueva
          this.newTaskName = '';  // Limpiar el campo de entrada
          this.startDate = '';     // Limpiar el campo de fecha de inicio
          this.endDate = '';       // Limpiar el campo de fecha de fin
          this.done = false
        },
        error: (error) => {
          console.error('Error al agregar tarea:', error);
        }
        });
    }
  }

  // Método para eliminar una tarea
  deleteTask(taskId: number): void {
    this.taskService.deleteTask(taskId).subscribe({
      next: (data) => {
        console.log('Tarea eliminada con éxito', data);
        this.loadTasks();  // Recargar las tareas después de eliminar una
      },
      error: (error) => {
        console.error('Error al eliminar tarea:', error);
      }
  });
  }

  // Método para actualizar el estado de la tarea y archivarla
  updateTaskState(newStateId: number, taskId: number) {
    const taskToUpdate = this.tasks.find(task => task.id === taskId);

    if (taskToUpdate) {
      const updatedTask = {
        id: taskId,
        name: taskToUpdate.name,
        stateId: newStateId,
        initialDate: taskToUpdate.initialDate,
        finishDate: taskToUpdate.finishDate,
        done: taskToUpdate.done
      };

      // Llamar a la API para actualizar la tarea
      this.taskService.updateTask(updatedTask).subscribe({
        next: (data) => {
          console.log('Tarea actualizada con éxito', data);
          this.loadTasks();  // Recargar las tareas después de la actualización

          // Si la tarea ha sido archivada, guardamos la acción en TaskHistory
          if (newStateId === 3) {
            const taskHistory = {
              id: 0,  // Se puede dejar en 0 si se auto-genera en la base de datos
              taskId: taskId,
              stateId: 3,  // Archivado
              changedDate: new Date().toISOString(),
            };

            // Llamar a la API para guardar la tarea archivada
            this.historyTaskService.archiveTask(taskHistory).subscribe({
              next: (historyResponse) => {
                console.log('Tarea archivada con éxito en TaskHistory', historyResponse);
              },
              error: (error) => {
                console.error('Error al archivar tarea en TaskHistory:', error);
              }
          });
          }
        },
        error: (error) => {
          console.error('Error al actualizar tarea:', error);
        }
    });
    }
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

  onCheckboxChange(event: Event, task: any) {
    const updatedTask = { 
      ...task, // Conserva todas las propiedades de la tarea
      done: (event.target as HTMLInputElement).checked  // Actualiza el estado 'done'
    };
  
    // Llamar al servicio para actualizar la tarea
    this.taskService.updateTask(updatedTask).subscribe({
      next: (response) => {
        console.log('Tarea actualizada con éxito', response);
        this.loadTasks();  // Recargar las tareas después de la actualización
      },
      error: (error) => {
        console.error('Error al actualizar tarea:', error);
      }
  });
  }
  
}
