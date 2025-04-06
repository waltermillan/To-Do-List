import { TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { AuthService } from './services/auth.service';
import { TaskService } from './services/task.service';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterModule.forRoot([]),
        MatCardModule,
        HttpClientTestingModule,
      ],
      providers: [AuthService, TaskService],
      declarations: [AppComponent, LoginComponent],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
