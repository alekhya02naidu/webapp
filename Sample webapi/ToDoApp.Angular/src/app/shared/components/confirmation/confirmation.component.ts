import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ToastrComponent } from '../toastr/toastr.component';

@Component({
  selector: 'app-confirmation',
  standalone: true,
  imports: [ToastrComponent],
  templateUrl: './confirmation.component.html',
  styleUrl: './confirmation.component.css'
})
export class ConfirmationComponent {
  @Input() message: string = '';
  @Output() confirm: EventEmitter<void> = new EventEmitter<void>();
  @Output() cancel: EventEmitter<void> = new EventEmitter<void>();

  constructor() { }

  onConfirm(): void {
    this.confirm.emit();
  }

  onCancel(): void {
    this.cancel.emit();
  }
}
