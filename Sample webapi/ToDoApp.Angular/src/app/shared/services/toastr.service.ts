import { Injectable } from '@angular/core';
import { ToastrComponent } from '../components/toastr/toastr.component';

@Injectable({
  providedIn: 'root'
})
export class ToastrService {
  private toastrComponent!: ToastrComponent;

  constructor() { }

  setToastrComponent(toastrComponent: ToastrComponent) {
    this.toastrComponent = toastrComponent;
  }

  showSuccess(message: string) {
    this.toastrComponent.showToastr(message, 'success');
  }

  showError(message: string) {
    this.toastrComponent.showToastr(message, 'error');
  }
}
