import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { TaskService } from '../services/task.service';
import { Task } from '../models/task.model';
import { MatDialog } from '@angular/material/dialog';
import { SuccessDialogComponent } from '../modals/success-dialog/success-dialog.component';
import { FailureDialogComponent } from '../modals/failure-dialog/failure-dialog.component';

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

  constructor(private taskService: TaskService,
              private dialog: MatDialog) { 
  }

  ngOnInit(): void {
    this.gesAllArchivedTasks();
  }

  gesAllArchivedTasks(): void {
    this.taskService.getAll().subscribe({
      next: (data) => {
        this.tasks = data.filter(t => t.stateId == 3);
      },
      error: (error) => {
        console.error('Error getting tasks:', error);
        this.dialog.open(FailureDialogComponent, {
          data: {
            message: 'Error getting tasks'
          }
        });
      },
      complete: () => {
        console.log('Loading tasks successfully');
      }
    });
  }

  deleteTask(taskId:number):void{
    this.taskService.delete(taskId).subscribe({
      next: (data) => {
        console.log('Task deleted successfully', data);
        this.gesAllArchivedTasks();

        this.dialog.open(SuccessDialogComponent, {
          data: {
            message: 'Task deleted successfully'
          }
        });

      },
      error: (error) => {
        console.error('Error deleting tasks:', error);

        this.dialog.open(FailureDialogComponent, {
          data: {
            message: 'Error deleting tasks.'
          }
        });
      }
    });
  }

  getStateName(stateId: number): string {
    switch (stateId) {
      case 1:
        return 'Pending';
      case 2:
        return 'Completed';
      case 3:
        return 'Archived';
      default:
        return 'Unknown';
    }
  }

  getDoneState(done:boolean):string
  {
    return (done == true) ? "YES" : "NO";
  }

  filterData(startDate: string, endDate: string): void{

    if (startDate == '' || endDate == ''){
      this.dialog.open(FailureDialogComponent, {
        data: {
          message: 'Filters empty.'
        }
      });
      return;
    }

    this.taskService.getAll().subscribe({

      next: (data) => {
        this.tasks = data.filter(t => t.initialDate >= startDate && t.initialDate <= endDate && t.stateId == 3);
      },
      error: (error) => {
        this.dialog.open(FailureDialogComponent, {
          data: {
            message: 'Error getting tasks.'
          }
        });
      },
      complete: () => {
        this.dialog.open(SuccessDialogComponent, {
          data: {
            message: 'Load tasks successfully.'
          }
        });
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
