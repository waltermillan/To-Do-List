import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';  // Importa FormsModule
import { RouterModule, Routes } from '@angular/router'; // Importa RouterModule y Routes
import { provideHttpClient, withInterceptorsFromDi, withFetch  } from '@angular/common/http';  // Importa HttpClientModule

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TodoListComponent } from './todo-list/todo-list.component';
import { AddTaskComponent } from './add-task/add-task.component';
import { HomeComponent } from './home/home.component';
import { ArchivedTaskComponent } from './archived-task/archived-task.component';
import { VoiceToTextComponent } from './voice-to-text/voice-to-text.component'; // Importa el componente de la página de inicio

// Define las rutas
const appRoutes: Routes = [
  { path: '', component: HomeComponent },  // Ruta para la página de inicio
  { path: 'todo', component: TodoListComponent },  // Ruta para la To-Do List
  { path: 'archived', component: ArchivedTaskComponent },  // Ruta para la To-Do List
  { path: 'voice', component: VoiceToTextComponent },  // Ruta para la To-Do List
];

@NgModule({
  declarations: [
    AppComponent,
    TodoListComponent,
    AddTaskComponent,
    ArchivedTaskComponent,
    VoiceToTextComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    RouterModule.forRoot(appRoutes)
  ],
  providers: [
    provideHttpClient(withInterceptorsFromDi(), withFetch())
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
