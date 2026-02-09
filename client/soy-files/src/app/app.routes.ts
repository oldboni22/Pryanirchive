import { Routes } from '@angular/router';
import {LoginScreen} from './features/login-screen/login-screen';
import {Desktop} from './features/desktop/desktop';

export const routes: Routes = [
  { path: 'login', component: LoginScreen},
  { path: 'desktop', component: Desktop},
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];
