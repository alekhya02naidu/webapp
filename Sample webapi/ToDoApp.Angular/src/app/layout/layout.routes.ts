import { Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { DashboardComponent } from '../pages/dashboard/dashboard.component';
import { TaskOverviewComponent } from '../pages/task-overview/task-overview.component';

export const mainRoutes: Routes = [
  {
    path: 'main',
    component: MainComponent,
    children: [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'active', component: TaskOverviewComponent, data: { view: 'Active' } },
      { path: 'completed', component: TaskOverviewComponent, data: { view: 'Completed' } },
    ]
  },
];
