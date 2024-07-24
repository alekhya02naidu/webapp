import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskDetailComponent } from '../task-detail/task-detail.component';
import { Tasks } from '../../../utils/models/tasks.model';

@Component({
  selector: 'app-task',
  standalone: true,
  imports: [CommonModule, TaskDetailComponent],
  templateUrl: './task.component.html',
  styleUrl: './task.component.css'
})
export class TaskComponent {
  @Input() imageType: 'active' | 'completed' = 'active';
  @Input() task!: Tasks;
  @Output() taskClicked: EventEmitter<Tasks> = new EventEmitter<Tasks>();
  @Output() taskDeleted = new EventEmitter<number>();
  showTaskDetail: boolean = false;
  selectedTask: Tasks | null = null;

  constructor() { }

  getBackgroundColor(): string {
    return this.task.isCompleted ? '#EDB046' : 'white';
  }

  getImageSrc(): string {
    return this.task.isCompleted ? 'assets/icons/completed-checkbox.svg' : 'assets/icons/checkbox.svg';
  }

  getImageAlt(): string {
    return this.task.isCompleted ? 'Icon of completed task' : 'Icon of active task';
  }

  onTaskClicked(): void {
    this.showTaskDetail = !this.showTaskDetail;
    this.taskClicked.emit(this.task);
  }

  onTaskDeleted(taskId: number): void {
    this.taskDeleted.emit(taskId);
    this.showTaskDetail = false;
  }
}
