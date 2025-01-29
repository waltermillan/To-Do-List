import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-add-task',
  templateUrl: './add-task.component.html',
  styleUrls: ['./add-task.component.css']
})
export class AddTaskComponent {

  newTaskName: string = '';  // Variable para almacenar el nombre de la nueva tarea

  @Output() taskAdded = new EventEmitter<{ name: string, completed: boolean }>();  // Evento para enviar la tarea al componente padre

  addTask(): void {
    if (this.newTaskName.trim()) {
      // Emitir el evento con la nueva tarea
      this.taskAdded.emit({ name: this.newTaskName, completed: false });
      this.newTaskName = '';  // Limpiar el campo de entrada
    }
  }
}
