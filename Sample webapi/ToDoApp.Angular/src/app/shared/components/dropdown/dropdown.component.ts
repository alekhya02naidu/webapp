import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dropdown',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dropdown.component.html',
  styleUrl: './dropdown.component.css'
})
export class DropdownComponent {
  @Input() currentPage: string = 'Dashboard';
  @Input() navOptions: string[] = ['Dashboard', 'Active', 'Completed'];
  @Output() viewChanged = new EventEmitter<string>();
  
  dropdownOpen: boolean = false;
  filteredNavOptions: string[] = [];

  constructor(private router: Router) {
    this.updateFilteredOptions();
  }

  ngOnChanges(): void {
    this.updateFilteredOptions();
  }

  toggleDropdown(): void {
    this.dropdownOpen = !this.dropdownOpen;
  }

  onNavOptionSelected(option: string): void {
    this.setCurrentPage(option);
    this.viewChanged.emit(option);
    this.dropdownOpen = false;
    const path = this.getPathForOption(option);
    if (path) {
      this.router.navigate([path]);
    }
  }

  setCurrentPage(page: string): void {
    this.currentPage = page;
    this.updateFilteredOptions();
  }

  private updateFilteredOptions(): void {
    this.filteredNavOptions = this.navOptions.filter(option => option !== this.currentPage);
  }

  private getPathForOption(option: string): string {
    switch (option) {
      case 'Dashboard': return 'main/dashboard';
      case 'Active': return 'main/active';
      case 'Completed': return 'main/completed';
      default: return '';
    }
  }
}
