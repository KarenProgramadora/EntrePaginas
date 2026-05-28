import { Routes } from '@angular/router';
import { Dashboard } from './pages/dashboard/dashboard';
import { Books } from './pages/books/books';
import { Authors } from './pages/authors/authors';
import { Members } from './pages/members/members';
import { Loans } from './pages/loans/loans';

export const routes: Routes = [
  { path: '',          redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: Dashboard },
  { path: 'books',     component: Books },
  { path: 'authors',   component: Authors },
  { path: 'members',   component: Members },
  { path: 'loans',     component: Loans },
];
