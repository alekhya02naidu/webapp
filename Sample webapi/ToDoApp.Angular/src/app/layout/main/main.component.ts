import { Component } from '@angular/core';
import { SidebarComponent } from '../../shared/components/sidebar/sidebar.component';
import { RouterModule, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AddTaskComponent } from '../../shared/components/add-task/add-task.component';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [SidebarComponent, RouterOutlet, RouterModule, CommonModule, AddTaskComponent],
  templateUrl: './main.component.html',
  styleUrl: './main.component.css'
})
export class MainComponent {
  showAddTask = false;

  onAddTask(): void {
    this.showAddTask = true;
  }

  closeAddTask(): void {
    this.showAddTask = false;
  }
}
