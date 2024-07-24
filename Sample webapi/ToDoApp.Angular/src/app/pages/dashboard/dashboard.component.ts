import { CommonModule, DatePipe } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { TaskComponent } from '../../shared/components/task/task.component';
import { TaskDetailComponent } from '../../shared/components/task-detail/task-detail.component';
import { HeaderComponent } from '../../shared/components/header/header.component';
import { DropdownComponent } from '../../shared/components/dropdown/dropdown.component';
import { TaskService } from '../../shared/services/task.service';
import { AddTaskComponent } from '../../shared/components/add-task/add-task.component';
import { ToastrService } from '../../shared/services/toastr.service';
import { constants } from '../../utils/constants/app.contants';
import { Tasks } from '../../utils/models/tasks.model';
import { ApiResponse } from '../../utils/models/api-response.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, TaskComponent, TaskDetailComponent, HeaderComponent, 
            DropdownComponent, AddTaskComponent],
  providers: [DatePipe],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit{
  format: string = 'EEEE, d MMMM yyyy';
  todayTasks: Tasks[] = [];
  selectedTask: Tasks | null = null;
  navOptions: string[] = ['Dashboard', 'Active', 'Completed'];
  errorMessage: string = "";
  completedPercentage: number = 0;
  activePercentage: number = 0;
  idsParam: number[] | undefined;
  showAddTask: boolean = false;
  username: string | null = null;
  currentDate: string | null = null;

  constructor(private taskService: TaskService, private toastrService: ToastrService, private datePipe: DatePipe) {}

  ngOnInit(): void {
    this.username = localStorage.getItem(constants.USERNAME);
    this.currentDate = this.getCurrentDate();
    this.loadTodaysTasks();
  }

  getCurrentDate(): string | null {
    const today = new Date();
    return this.datePipe.transform(today, this.format);
  }

  onTaskClicked(taskDetail: Tasks): void {
    this.selectedTask = taskDetail;
  }

  closeTaskDetail(): void {
    this.selectedTask = null;
  }

  loadTodaysTasks(): void {
    this.taskService.getUserSpecificTask().subscribe({
      next: (response: ApiResponse<Tasks[]>) => {
        this.todayTasks = response.data;
        this.calculateCompletionStatus();
      },
      error: (err: any) => {
        this.errorMessage = err;
      }
    });
  }

  calculateCompletionStatus(): void {
    const totalTasks = this.todayTasks.length;
    const completedTasks = this.todayTasks.filter(task => task.isCompleted).length;
    const activeTasks = totalTasks - completedTasks;

    this.completedPercentage = totalTasks > 0 ? Math.round((completedTasks / totalTasks) * 100) : 0;
    this.activePercentage = totalTasks > 0 ? Math.round((activeTasks / totalTasks) * 100) : 0;
  }

  deleteAllTasks(): void {
    const taskIds = this.todayTasks.map(task => task.id).filter(id => id !== undefined) as number[];
    if (taskIds.length === 0) {
      return;
    }
    this.idsParam = taskIds.join('&ids=').split('&ids=').map(Number);
  
    this.taskService.deleteTasks(this.idsParam).subscribe({
      next: () => {
        this.toastrService.showSuccess("Tasks deleted Successfully");
        this.todayTasks = []; 
        this.calculateCompletionStatus(); 
      },
      error: () => {
        this.toastrService.showError("Error deleting tasks, Please try again");
      }
    });
  }
  
  onAddTask(): void {
    this.showAddTask = true;
  }

  closeAddTask(): void {
    this.showAddTask = false;
  }
}
