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
  tasks: Task[] = [];
  newTaskName: string = '';  // Variable para almacenar el nombre de la nueva tarea
  startDate: string = '';    // Variable para almacenar la fecha de inicio
  endDate: string = '';      // Variable para almacenar la fecha de fin
  done: boolean = false;

  recognition: any;
  isRecognizing = false; // Indicador de si está reconociendo
  errorMessage: string | null = null;  // Mensaje de error para mostrar en la UI

  constructor(
    private taskService: TaskService, 
    private historyTaskService: HistoryTaskService  // Inyectar el HistoryTaskService
  ) { }

  ngOnInit(): void {
    this.loadTasks();
    this.setupRecognition();  // Configurar el reconocimiento de voz
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

      this.taskService.updateTask(updatedTask, updatedTask.id).subscribe({
        next: (data) => {
          console.log('Tarea actualizada con éxito', data);
          this.loadTasks();

          if (newStateId === 3) {
            const taskHistory = {
              id: 0,
              taskId: taskId,
              stateId: 3,  // Archivado
              changedDate: new Date().toISOString(),
            };

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
        return 'Desconocido';
    }
  }

  // Método de cambio de estado al marcar la tarea como realizada
  onCheckboxChange(event: Event, task: any) {
    const updatedTask = { 
      ...task, 
      done: (event.target as HTMLInputElement).checked 
    };

    this.taskService.updateTask(updatedTask, updatedTask.id).subscribe({
      next: (response) => {
        console.log('Tarea actualizada con éxito', response);
        this.loadTasks();
      },
      error: (error) => {
        console.error('Error al actualizar tarea:', error);
      }
    });
  }

  // Configuración del reconocimiento de voz
  setupRecognition(): void {
    if (typeof window !== 'undefined' && ('SpeechRecognition' in window || 'webkitSpeechRecognition' in window)) {
      this.recognition = new (window.SpeechRecognition || window.webkitSpeechRecognition)();
      this.recognition.lang = 'es-ES';
      this.recognition.continuous = false;
      this.recognition.interimResults = false;

      this.recognition.onresult = (event: any) => {
        let finalTranscript = '';
        for (let i = event.resultIndex; i < event.results.length; i++) {
          const transcript = event.results[i][0].transcript;
          if (event.results[i].isFinal) {
            finalTranscript += transcript + ' ';
          }
        }
        console.log('Texto final transcrito:', finalTranscript);
        this.newTaskName = finalTranscript.trim();  // Actualiza el texto
      };

      this.recognition.onerror = (event: any) => {
        this.errorMessage = `Error: ${event.error}`;
        this.isRecognizing = false;
      };

      this.recognition.onend = () => {
        console.log('Reconocimiento detenido.');
        this.isRecognizing = false;
      };
    } else {
      this.errorMessage = 'La API de reconocimiento de voz no está soportada en este navegador.';
    }
  }

  // Alterna el estado de reconocimiento de voz
  toggleRecognition(): void {
    if (this.isRecognizing) {
      this.stopRecognition();
    } else {
      this.startRecognition();
    }
  }

  startRecognition(): void {
    this.isRecognizing = true;
    this.recognition.start();
    this.errorMessage = null;
    console.log('Iniciando grabación...');
  }

  stopRecognition(): void {
    if (this.isRecognizing) {
      this.recognition.stop();
      console.log('Deteniendo grabación...');
      this.isRecognizing = false;
    }
  }
}
