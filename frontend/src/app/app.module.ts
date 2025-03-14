import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'; 
import { RouterModule, Routes } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi, withFetch  } from '@angular/common/http'; 

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TodoListComponent } from './todo-list/todo-list.component';
import { HomeComponent } from './home/home.component';
import { ArchivedTaskComponent } from './archived-task/archived-task.component';

const appRoutes: Routes = [
  { path: '', component: HomeComponent }, 
  { path: 'todo', component: TodoListComponent },
  { path: 'archived', component: ArchivedTaskComponent },
];

@NgModule({
  declarations: [
    AppComponent,
    TodoListComponent,
    ArchivedTaskComponent
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
