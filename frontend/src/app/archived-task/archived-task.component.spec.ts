import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ArchivedTaskComponent } from './archived-task.component';
import { TaskService } from '../services/task.service';

describe('ArchivedTaskComponent', () => {
  let component: ArchivedTaskComponent;
  let fixture: ComponentFixture<ArchivedTaskComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterModule.forRoot([]),
        FormsModule,
      ],
      declarations: [ArchivedTaskComponent],
      providers: [
        TaskService,
        provideHttpClient()
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(ArchivedTaskComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
