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
import { MatChipsModule } from '@angular/material/chips';
import { BookService } from '../../core/services/book.service';
import { CategoryService } from '../../core/services/category.service';
import { PublisherService } from '../../core/services/publisher.service';
import { Book } from '../../core/models/book.model';
import { Category } from '../../core/models/category.model';
import { Publisher } from '../../core/models/publisher.model';

@Component({
  selector: 'app-books',
  imports: [
    CommonModule, ReactiveFormsModule,
    MatTableModule, MatCardModule, MatButtonModule, MatIconModule,
    MatFormFieldModule, MatInputModule, MatSelectModule, MatChipsModule
  ],
  templateUrl: './books.html',
  styles: [`
    .page-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px; }
    .form-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 12px; }
    .form-actions { display: flex; gap: 8px; justify-content: flex-end; margin-top: 8px; }
    table { width: 100%; }
    .available { color: #2e7d32; font-weight: 600; }
    .unavailable { color: #c62828; font-weight: 600; }
  `]
})
export class Books implements OnInit {
  books: Book[] = [];
  categories: Category[] = [];
  publishers: Publisher[] = [];
  displayedColumns = ['title', 'isbn', 'categoryName', 'publisherName', 'availableCopies', 'conditionName', 'actions'];
  showForm = false;
  editingId: number | null = null;
  form!: FormGroup;

  conditions = [
    { value: 0, label: 'Nuevo' },
    { value: 1, label: 'Bueno' },
    { value: 2, label: 'Regular' },
    { value: 3, label: 'Malo' },
  ];

  constructor(
    private bookSvc: BookService,
    private catSvc: CategoryService,
    private pubSvc: PublisherService,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadBooks();
    this.catSvc.getAll().subscribe(d => { this.categories = d; this.cdr.detectChanges(); });
    this.pubSvc.getAll().subscribe(d => { this.publishers = d; this.cdr.detectChanges(); });
    this.buildForm();
  }

  loadBooks() { this.bookSvc.getAll().subscribe(d => { this.books = d; this.cdr.detectChanges(); }); }

  buildForm(book?: Book) {
    this.form = this.fb.group({
      title:           [book?.title ?? '',          Validators.required],
      isbn:            [book?.isbn ?? '',           Validators.required],
      publicationYear: [book?.publicationYear ?? new Date().getFullYear(), Validators.required],
      totalCopies:     [book?.totalCopies ?? 1,     [Validators.required, Validators.min(1)]],
      availableCopies: [book?.availableCopies ?? 1, [Validators.required, Validators.min(0)]],
      condition:       [book?.condition ?? 0,        Validators.required],
      coverUrl:        [book?.coverUrl ?? ''],
      categoryId:      [book?.categoryId ?? null,    Validators.required],
      publisherId:     [book?.publisherId ?? null,   Validators.required],
    });
  }

  openCreate() { this.editingId = null; this.buildForm(); this.showForm = true; }

  openEdit(book: Book) {
    this.editingId = book.id;
    this.buildForm(book);
    this.showForm = true;
  }

  save() {
    if (this.form.invalid) return;
    const dto = this.form.value;
    if (this.editingId) {
      this.bookSvc.update(this.editingId, dto).subscribe(() => { this.loadBooks(); this.showForm = false; });
    } else {
      this.bookSvc.create(dto).subscribe(() => { this.loadBooks(); this.showForm = false; });
    }
  }

  delete(id: number) {
    if (!confirm('¿Eliminar este libro?')) return;
    this.bookSvc.delete(id).subscribe(() => this.loadBooks());
  }

  cancel() { this.showForm = false; }
}
