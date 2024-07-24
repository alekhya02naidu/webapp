import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from '../../services/toastr.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  @Output() addTask = new EventEmitter<void>();
  @Input() headerTitle: string = "";

  constructor(private router: Router, private toastrService: ToastrService) {
  }

  toggleAddTask(): void {
    this.addTask.emit();
  }

  signOut(): void {
    localStorage.removeItem('token');
    this.router.navigate(['/auth/sign-in']);
    this.toastrService.showSuccess("Logged out successfully");
  }
}
