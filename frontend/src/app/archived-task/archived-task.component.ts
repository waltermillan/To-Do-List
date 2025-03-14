import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { TaskService } from '../services/task.service';
import { Task } from '../models/task.model';

@Component({
  selector: 'app-archived-task',
  templateUrl: './archived-task.component.html',
  styleUrl: './archived-task.component.css'
})
export class ArchivedTaskComponent implements OnInit {

  @ViewChild('startDate') sStartDate: ElementRef<HTMLInputElement> | undefined;
  @ViewChild('endDate') sEndDate: ElementRef<HTMLInputElement> | undefined;

  tasks:Task[] = [];
  newTaskName: string = ''; 
  startDate: string = '';
  endDate: string = ''; 

  constructor(private taskService: TaskService) { }

  ngOnInit(): void {
    this.gesAllArchivedTasks();
  }

  gesAllArchivedTasks(): void {
    this.taskService.getAllTasks().subscribe({
      next: (data) => {
        this.tasks = data.filter(t => t.stateId == 3);
      },
      error: (error) => {
        console.error('Error al obtener tareas:', error);
      },
      complete: () => {
        console.log('La carga de tareas ha finalizado');
      }
    });
  }

  deleteTask(taskId:number):void{
    this.taskService.deleteTask(taskId).subscribe({
      next: (data) => {
        console.log('Tarea eliminada con éxito', data);
        this.gesAllArchivedTasks();
      },
      error: (error) => {
        console.error('Error al eliminar tarea:', error);
      }
    });
  }

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

  getDoneState(done:boolean):string
  {
    return (done == true) ? "SI" : "NO";
  }

  filterData(startDate: string, endDate: string): void{

    if (startDate == '' || endDate == ''){
      alert('Los filtros están vacios');
      return;
    }

    this.taskService.getAllTasks().subscribe({

      next: (data) => {
        this.tasks = data.filter(t => t.initialDate >= startDate && t.initialDate <= endDate);
      },
      error: (error) => {
        console.error('Error al obtener tareas:', error);
      },
      complete: () => {
        console.log('La carga de tareas ha finalizado');
      }
    });
  }

  cleanData(startDate: string, endDate: string): void {
    this.gesAllArchivedTasks();

    this.startDate = '';
    this.endDate = '';
      
    if (this.sStartDate && this.sStartDate.nativeElement) {
      this.sStartDate.nativeElement.value = '';
    }
  
    if (this.sEndDate && this.sEndDate.nativeElement) {
      this.sEndDate.nativeElement.value = '';
    }
  }
}
