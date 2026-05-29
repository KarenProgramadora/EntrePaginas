import { Routes } from '@angular/router';
import { Dashboard } from './features/dashboard/dashboard';
import { Books } from './features/books/books';
import { Authors } from './features/authors/authors';
import { Members } from './features/members/members';
import { Loans } from './features/loans/loans';

export const routes: Routes = [
  { path: '',          redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: Dashboard },
  { path: 'books',     component: Books },
  { path: 'authors',   component: Authors },
  { path: 'members',   component: Members },
  { path: 'loans',     component: Loans },
];
