import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddTaskComponent } from '../add-task/add-task.component';
import { TaskService } from '../../services/task.service';
import { ConfirmationComponent } from '../confirmation/confirmation.component';
import { ToastrService } from '../../services/toastr.service';
import { Router } from '@angular/router';
import { Tasks } from '../../../utils/models/tasks.model';

@Component({
  selector: 'app-task-detail',
  standalone: true,
  imports: [CommonModule, AddTaskComponent, ConfirmationComponent],
  templateUrl: './task-detail.component.html',
  styleUrl: './task-detail.component.css'
})
export class TaskDetailComponent {
  @Input() task!: Tasks;
  @Output() editTaskClicked: EventEmitter<Tasks> = new EventEmitter<Tasks>();
  @Output() taskDeleted = new EventEmitter<number>();
  isEditMode: boolean = false;
  showCompletionConfirmation: boolean = false;
  showDeletionConfirmation: boolean = false;
  
  constructor(private taskService: TaskService, 
            private toastrService: ToastrService,
            private router: Router) {}

  getFormattedTimeDifference(date: Date | string): string {
    const now = new Date();
    const targetDate = new Date(date);
    const diffInSeconds = Math.floor((now.getTime() - targetDate.getTime()) / 1000);

    const days = Math.floor(diffInSeconds / (3600 * 24));
    const hours = Math.floor((diffInSeconds % (3600 * 24)) / 3600);
    const minutes = Math.floor((diffInSeconds % 3600) / 60);

    if (days > 0) {
      return `${days} day${days > 1 ? 's' : ''} ago`;
    } 
    else if (hours > 0) {
      return `${hours} hour${hours > 1 ? 's' : ''} ago`;
    } 
    else if (minutes > 0) {
      return `${minutes} minute${minutes > 1 ? 's' : ''} ago`;
    } 
    else {
      return 'just now';
    }
  }

  getBackgroundColor(): string {
    return this.task.isCompleted ? '#EDB046' : 'white';
  }
  
  getImageSrc(): string {
    return this.task.isCompleted ? 'assets/icons/completed-checkbox.svg' : 'assets/icons/checkbox.svg';
  }

  getImageAlt(): string {
    return this.task.isCompleted ? 'Icon of completed task' : 'Icon of active task';
  }

  onEditTaskClick(): void {
    this.editTaskClicked.emit(this.task);
  }

  openEditTask(): void {
    this.isEditMode = true;
  }

  onCloseEdit(): void {
    this.isEditMode = false;
  }

  selfReload(): void {
    const currentUrl = this.router.url;
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate([currentUrl]);
    }); 
  }

  onTaskUpdated(updatedTask: Tasks): void {
    this.task = updatedTask;
    this.isEditMode = false;
  }

  showDeleteConfirmation(): void {
    this.showDeletionConfirmation = true;
  }

  deleteConfirmedTask(): void {
    if (this.task && this.task.id) {
      this.taskService.deleteTasks([this.task.id]).subscribe({
        next: () => {
          this.taskDeleted.emit(this.task.id);
          this.toastrService.showSuccess("deleted Successfully");
          setTimeout(() => {
            this.selfReload();
          }, 500);
        },
        error: () => {
          this.toastrService.showError("Error while deleting the task");
        }
      });
    }
  }

  cancelDeletion(): void {
    this.showDeletionConfirmation = false;
  }

  onCompletionCheckboxChange(event: Event): void {
    const checkbox = event.target as HTMLInputElement;
    if (checkbox.checked) {
      this.showCompletionConfirmation = true;
    }
  }

  markTaskAsCompleted(): void {
    this.task.isCompleted = true;
    this.taskService.updateTask(this.task).subscribe({
      next: () => {
        this.toastrService.showSuccess("Task marked as completed");
      },
      error: () => {
        this.toastrService.showError("Error while marking the task as completed");
      }
    });
    this.showCompletionConfirmation = false;
    this.router.navigate(['main/completed']);
  }

  cancelCompletion(): void {
    this.showCompletionConfirmation = false;
    setTimeout(() => {
      this.selfReload();
    }, 500);
  }
}
