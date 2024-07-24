import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { SidebarComponent } from './shared/components/sidebar/sidebar.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { ToastrComponent } from './shared/components/toastr/toastr.component';
import { ToastrService } from './shared/services/toastr.service';
import { AuthenticationComponent } from './pages/authentication/authentication.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterModule, AuthenticationComponent,
            SidebarComponent, DashboardComponent, ToastrComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements AfterViewInit{
  title = 'ToDoApp';
  @ViewChild(ToastrComponent) toastrComponent!: ToastrComponent;

  constructor(private toastrService: ToastrService) {}

  ngAfterViewInit() {
    this.toastrService.setToastrComponent(this.toastrComponent);
  }
}
