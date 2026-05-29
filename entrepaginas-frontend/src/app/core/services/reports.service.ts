import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api-service';
import {
  LibraryStats,
  BooksByCategory,
  MostLoanedBook,
  MemberActivity,
} from '../models/reports.model';

@Injectable({ providedIn: 'root' })
export class ReportsService {
  private api = inject(ApiService);

  getStats(): Observable<LibraryStats> { return this.api.get<LibraryStats>('reports/stats'); }
  getBooksByCategory(): Observable<BooksByCategory[]> { return this.api.get<BooksByCategory[]>('reports/books-by-category'); }
  getMostLoaned(top = 5): Observable<MostLoanedBook[]> { return this.api.get<MostLoanedBook[]>('reports/most-loaned', { top }); }
  getMemberActivity(): Observable<MemberActivity[]> { return this.api.get<MemberActivity[]>('reports/member-activity'); }
}
