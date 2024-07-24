import { Routes } from '@angular/router';
import { AuthenticationComponent } from './pages/authentication/authentication.component';
import { mainRoutes } from './layout/layout.routes';

export const routes: Routes = [
  {
    path: 'auth',
    component: AuthenticationComponent,
    children: [
      { path: 'sign-in', component: AuthenticationComponent, data: { mode: 'sign-in' } },
      { path: 'sign-up', component: AuthenticationComponent, data: { mode: 'sign-up' } }
    ]
  },
  { path: '', redirectTo: 'auth/sign-in', pathMatch: 'full' },
  {
    path: '',
    children: [...mainRoutes]
  },
];
