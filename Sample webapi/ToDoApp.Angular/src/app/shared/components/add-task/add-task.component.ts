import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { TaskService } from '../../services/task.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastrService } from '../../services/toastr.service';
import { Router } from '@angular/router';
import { Tasks } from '../../../utils/models/tasks.model';
import { ApiResponse } from '../../../utils/models/api-response.model';

@Component({
  selector: 'app-add-task',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-task.component.html',
  styleUrl: './add-task.component.css'
})
export class AddTaskComponent implements OnInit{
  @Output() closeAddTask = new EventEmitter<void>();
  @Output() taskAdded = new EventEmitter<Tasks>();
  @Input() task: Tasks = new Tasks();
  showAddTask: boolean = true;
  taskTitle: string = "";
  taskDescription: string | undefined;
  
  constructor(private taskService: TaskService,
      private toastrService: ToastrService,
      private router: Router) {}

  ngOnInit(): void {
    if (this.task) { 
      this.taskTitle = this.task.title;
      this.taskDescription = this.task.description;
    }
  }

  reload(): void {
    const currentUrl = this.router.url;
    this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
      this.router.navigate([currentUrl]);
      });
  }

  saveTask(): void {
    const taskToSave: Tasks = {
      ...this.task,
      title: this.taskTitle,
      description: this.taskDescription,
      updatedAt: new Date()
    };

    if (!taskToSave.id) {
      this.taskService.addTask(taskToSave).subscribe({
        next: (response: ApiResponse<number>) => {
          taskToSave.id = response.data; 
          this.taskAdded.emit(taskToSave);
          this.close();
          this.toastrService.showSuccess("New task added Successfully");
          setTimeout(() => {
            this.reload();
          }, 500);
        },
        error: () => {
          this.toastrService.showError("Error occured while adding a new task");
        }
      });      
    } 
    else {
      this.taskService.updateTask(taskToSave).subscribe({
        next: () => {
          this.taskAdded.emit(taskToSave);
          this.close();
          this.toastrService.showSuccess("Task updated Successfully");
          setTimeout(() => {
            this.reload();
          }, 500);
        },
        error: () => {
          this.toastrService.showError("Error while updating task");
        }
      });
    }
  }

  close(): void {
    this.showAddTask = false;
    this.closeAddTask.emit();
  }
}
