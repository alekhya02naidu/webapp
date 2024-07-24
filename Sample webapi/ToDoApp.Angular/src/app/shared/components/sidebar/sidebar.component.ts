import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';
import { AddTaskComponent } from '../add-task/add-task.component';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, AddTaskComponent],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  navOptions: string[] = ["Dashboard", "Active", "Completed"];
  navOptionSelected: boolean[] = [false, false, false];
  @Output() addTask = new EventEmitter<void>();

  constructor(private route: Router) {}

  addTaskClicked(): void {
    this.addTask.emit();
  }

  onNavOptionSelected(index: number): void {
    if(index == 0) {
      this.route.navigate(['/main/dashboard']);
    }
    if(index == 1) {
      this.route.navigate(['/main/active']);
    }
    if( index == 2) {
    this.route.navigate(['/main/completed']);
  }
    this.navOptionSelected.fill(false);
    this.navOptionSelected[index] = true; 
  }
}
