import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ToastrMessage } from '../../../utils/models/toastr-msg.model';

@Component({
  selector: 'app-toastr',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './toastr.component.html',
  styleUrl: './toastr.component.css'
})
export class ToastrComponent {
  messages: ToastrMessage[] = [];

  showToastr(message: string, type: 'success' | 'error'): void {
    const toastrMessage: ToastrMessage = { message, type };
    this.messages.push(toastrMessage);

    setTimeout(() => {
      this.removeToastr(toastrMessage);
    }, 3000);
  }

  removeToastr(toastrMessage: ToastrMessage): void {
    this.messages = this.messages.filter(msg => msg !== toastrMessage);
  }
}
