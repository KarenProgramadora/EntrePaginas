import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { LoanService } from '../../services/loan';
import { BookService } from '../../services/book';
import { MemberService } from '../../services/member';
import { Loan, Book, Member } from '../../models/models';

@Component({
  selector: 'app-loans',
  imports: [
    CommonModule, ReactiveFormsModule,
    MatTableModule, MatCardModule, MatButtonModule, MatIconModule,
    MatFormFieldModule, MatInputModule, MatSelectModule
  ],
  templateUrl: './loans.html',
  styles: [`
    .page-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px; }
    .form-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 12px; }
    .form-actions { display: flex; gap: 8px; justify-content: flex-end; margin-top: 8px; }
    table { width: 100%; }
    .chip-active   { background: #c8e6c9; color: #1b5e20; padding: 2px 8px; border-radius: 12px; font-size: 0.8rem; }
    .chip-overdue  { background: #ffcdd2; color: #b71c1c; padding: 2px 8px; border-radius: 12px; font-size: 0.8rem; }
    .chip-returned { background: #e0e0e0; color: #424242; padding: 2px 8px; border-radius: 12px; font-size: 0.8rem; }
  `]
})
export class Loans implements OnInit {
  loans: Loan[] = [];
  books: Book[] = [];
  members: Member[] = [];
  displayedColumns = ['bookTitle', 'memberFullName', 'loanDate', 'dueDate', 'statusName', 'actions'];
  showForm = false;
  form!: FormGroup;

  constructor(
    private loanSvc: LoanService,
    private bookSvc: BookService,
    private memberSvc: MemberService,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.load();
    this.bookSvc.getAvailable().subscribe(d => { this.books = d; this.cdr.detectChanges(); });
    this.memberSvc.getAll().subscribe(d => { this.members = d.filter(m => m.status === 0); this.cdr.detectChanges(); });
    this.buildForm();
  }

  load() { this.loanSvc.getAll().subscribe(d => { this.loans = d; this.cdr.detectChanges(); }); }

  buildForm() {
    this.form = this.fb.group({
      memberId: [null, Validators.required],
      bookId:   [null, Validators.required],
      loanDays: [14,   [Validators.required, Validators.min(1), Validators.max(30)]],
    });
  }

  chipClass(status: number) {
    return status === 0 ? 'chip-active' : status === 2 ? 'chip-overdue' : 'chip-returned';
  }

  openCreate() { this.buildForm(); this.showForm = true; }
  cancel() { this.showForm = false; }

  save() {
    if (this.form.invalid) return;
    this.loanSvc.create(this.form.value).subscribe({
      next: () => { this.load(); this.showForm = false; this.bookSvc.getAvailable().subscribe(d => this.books = d); },
      error: err => alert(err.error?.message ?? 'Error al crear préstamo')
    });
  }

  return(id: number) {
    if (!confirm('¿Registrar devolución de este préstamo?')) return;
    this.loanSvc.returnLoan(id).subscribe({
      next: () => this.load(),
      error: err => alert(err.error?.message ?? 'Error al devolver')
    });
  }

  markOverdue() {
    this.loanSvc.markOverdue().subscribe(() => this.load());
  }
}
