import { Component, Input } from '@angular/core';
import { Tasks } from '../../utils/models/tasks.model';
import { TaskService } from '../../shared/services/task.service';
import { ToastrService } from '../../shared/services/toastr.service';
import { ApiResponse } from '../../utils/models/api-response.model';
import { TaskComponent } from '../../shared/components/task/task.component';
import { DropdownComponent } from '../../shared/components/dropdown/dropdown.component';
import { AddTaskComponent } from '../../shared/components/add-task/add-task.component';
import { HeaderComponent } from '../../shared/components/header/header.component';
import { CommonModule, DatePipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-task-overview',
  standalone: true,
  imports: [CommonModule, TaskComponent, DropdownComponent, AddTaskComponent, HeaderComponent],
  providers: [DatePipe],
  templateUrl: './task-overview.component.html',
  styleUrl: './task-overview.component.css'
})
export class TaskOverviewComponent {
  format: string = 'EEEE, d MMMM yyyy';
  tasks: Tasks[] = [];
  currentDate: string | null = null;
  errorMessage: string = '';
  selectedTask: Tasks | null = null;
  showAddTask: boolean = false;
  navOptions: string[] = ['Dashboard', 'Active', 'Completed'];
  currentView: string = 'Active';

  constructor(private taskService: TaskService, 
            private toastrService: ToastrService,
            private route: ActivatedRoute,
            private datePipe: DatePipe) {}

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.currentView = data['view'];
      this.loadTasks();
    });
    this.currentDate = this.getCurrentDate();
  }

  getCurrentDate(): string | null {
    const today = new Date();
    return this.datePipe.transform(today, this.format);
  }

  loadTasks(): void {
    this.taskService.getUserSpecificTask().subscribe({
      next: (response: ApiResponse<Tasks[]>) => {
        this.tasks = response.data.filter(task => 
          this.currentView === 'Active' ? !task.isCompleted : task.isCompleted
        );
      },
      error: (err: any) => {
        this.errorMessage = err;
      }
    });
  }

  onTaskClicked(taskDetail: Tasks): void {
    this.selectedTask = taskDetail;
  }

  closeTaskDetail(): void {
    this.selectedTask = null;
  }

  onAddTask(): void {
    this.showAddTask = true;
  }

  closeAddTask(): void {
    this.showAddTask = false;
  }

  switchView(view: string): void {
    this.currentView = view;
    this.loadTasks();
  }

  deleteSelectedTasks(taskIds: number[]): void {
    this.taskService.deleteTasks(taskIds).subscribe({
      next: () => {
        this.toastrService.showSuccess("Tasks deleted successfully");
        this.loadTasks();
      },
      error: (error) => {
        this.toastrService.showError("Error while deleting the tasks");
        this.errorMessage = error.message;
      }
    });
  }
}
