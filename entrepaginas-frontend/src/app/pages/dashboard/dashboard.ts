import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { ReportsService } from '../../services/reports';
import { LibraryStats, BooksByCategory, MostLoanedBook } from '../../models/models';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, MatCardModule, MatIconModule, MatTableModule],
  templateUrl: './dashboard.html',
  styles: [`
    .stats-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(180px, 1fr)); gap: 16px; margin-bottom: 24px; }
    .stat-card { text-align: center; }
    .stat-value { font-size: 2rem; font-weight: 700; color: #1a237e; }
    .stat-label { color: #666; font-size: 0.85rem; }
    .tables-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
    h2 { margin: 0 0 16px; font-size: 1.1rem; color: #1a237e; }
  `]
})
export class Dashboard implements OnInit {
  stats: LibraryStats | null = null;
  byCategory: BooksByCategory[] = [];
  mostLoaned: MostLoanedBook[] = [];

  categoryColumns = ['categoryName', 'bookCount', 'availableCopies'];
  loanedColumns = ['title', 'authorName', 'loanCount'];

  constructor(private reports: ReportsService, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.reports.getStats().subscribe(s => { this.stats = s; this.cdr.detectChanges(); });
    this.reports.getBooksByCategory().subscribe(d => { this.byCategory = d; this.cdr.detectChanges(); });
    this.reports.getMostLoaned(5).subscribe(d => { this.mostLoaned = d; this.cdr.detectChanges(); });
  }
}
