import { Component, OnInit } from '@angular/core';
import { TaskService } from '../services/task.service';
import { HistoryTaskService } from '../services/history-task.service';
import { Task } from '../models/task.model';

import { MatDialog } from '@angular/material/dialog';
import { SuccessDialogComponent } from '../modals/success-dialog/success-dialog.component';
import { FailureDialogComponent } from '../modals/failure-dialog/failure-dialog.component';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.css']
})
export class TodoListComponent implements OnInit {
  tasks: Task[] = [];
  newTaskName: string = ''; 
  startDate: string = '';    
  endDate: string = '';      
  done: boolean = false;

  recognition: any;
  isRecognizing = false; 
  errorMessage: string | null = null;

  constructor(
    private taskService: TaskService, 
    private historyTaskService: HistoryTaskService,
    private dialog: MatDialog) {   
  }

  ngOnInit(): void {
    this.loadTasks();
    this.setupRecognition();
  }

  loadTasks(): void {
    this.taskService.getAll().subscribe({
      next: (data) => {
        this.tasks = data.filter(t => t.stateId == 1 || t.stateId == 2);
      },
      error: (error) => {
        console.error('Error getting tasks:', error);
      }
    });
  }

  addTask(): void {
    if (!this.newTaskName.trim()) {
      this.dialog.open(FailureDialogComponent, {
        data: {
          message: 'Please enter a task\'s name.'
        }
      });
      return;
    }

    if (!this.startDate || !this.endDate) {
      this.dialog.open(FailureDialogComponent, {
        data: {
          message: 'Please enter start and end dates.'
        }
      });
      return;
    }

    if (new Date(this.startDate) > new Date(this.endDate)) {
      this.dialog.open(FailureDialogComponent, {
        data: {
          message: 'The end date cannot be earlier than the start date.'
        }
      });
      return;
    }

    if (this.newTaskName.trim() && this.startDate && this.endDate) {
      const newTask = {
        name: this.newTaskName,
        stateId: 1,
        done: false,
        initialDate: this.startDate,
        finishDate: this.endDate
      };

      this.taskService.add(newTask).subscribe({
        next: (data) => {
          console.log('Task added successfully', data);
          this.loadTasks();  
          this.newTaskName = ''; 
          this.startDate = '';
          this.endDate = ''; 
          this.done = false;

          this.dialog.open(SuccessDialogComponent, {
            data: {
              message: 'Task added successfully'
            }
          });
          
        },
        error: (error) => {
          console.error('Error addedd task:', error);

          this.dialog.open(FailureDialogComponent, {
            data: {
              message: 'Error addedd task'
            }
          });
        }
      });
    }
  }

  deleteTask(taskId: number): void {
    this.taskService.delete(taskId).subscribe({
      next: (data) => {
        console.log('Task deleted successfully', data);
        this.loadTasks();

        this.dialog.open(SuccessDialogComponent, {
          data: {
            message: 'Task deleted successfully'
          }
        });
      },
      error: (error) => {
        console.error('Error deleting task:', error);

        this.dialog.open(FailureDialogComponent, {
          data: {
            message: 'Error deleting task'
          }
        });
      }
    });
  }

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
          console.log('Task updated successfully', data);
          this.loadTasks();

          this.dialog.open(SuccessDialogComponent, {
            data: {
              message: 'Task updated successfully'
            }
          });

          if (newStateId === 3) {
            const taskHistory = {
              id: 0,
              taskId: taskId,
              stateId: 3,
              changedDate: new Date().toISOString(),
            };

            this.historyTaskService.add(taskHistory).subscribe({
              next: (historyResponse) => {
                console.log('Task archiving successfully in Task-History', historyResponse);

                this.dialog.open(SuccessDialogComponent, {
                  data: {
                    message: 'Task archiving successfully in Task-History'
                  }
                });

              },
              error: (error) => {
                console.error('Error archiving task in Task-History:', error);

                this.dialog.open(FailureDialogComponent, {
                  data: {
                    message: 'Error archiving task in Task-History'
                  }
                });
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

  onCheckboxChange(event: Event, task: any) {
    const updatedTask = { 
      ...task, 
      done: (event.target as HTMLInputElement).checked 
    };

    this.taskService.updateTask(updatedTask, updatedTask.id).subscribe({
      next: (response) => {
        console.log('Task updated successfully', response);

        this.dialog.open(SuccessDialogComponent, {
          data: {
            message: 'Task updated successfully'
          }
        });

        this.loadTasks();
      },
      error: (error) => {
        console.error('Error updating task:', error);

        this.dialog.open(SuccessDialogComponent, {
          data: {
            message: 'Error updating task'
          }
        });
      }
    });
  }

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
        this.newTaskName = finalTranscript.trim();
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
