import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArchivedTaskComponent } from './archived-task.component';

describe('ArchivedTaskComponent', () => {
  let component: ArchivedTaskComponent;
  let fixture: ComponentFixture<ArchivedTaskComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ArchivedTaskComponent]
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
