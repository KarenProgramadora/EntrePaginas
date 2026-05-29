import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthorService } from '../../core/services/author.service';
import { Author } from '../../core/models/author.model';

@Component({
  selector: 'app-authors',
  imports: [
    CommonModule, ReactiveFormsModule,
    MatTableModule, MatCardModule, MatButtonModule, MatIconModule,
    MatFormFieldModule, MatInputModule
  ],
  templateUrl: './authors.html',
  styles: [`
    .page-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px; }
    .form-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 12px; }
    .form-actions { display: flex; gap: 8px; justify-content: flex-end; margin-top: 8px; }
    table { width: 100%; }
  `]
})
export class Authors implements OnInit {
  authors: Author[] = [];
  displayedColumns = ['fullName', 'nationality', 'birthDate', 'actions'];
  showForm = false;
  editingId: number | null = null;
  form!: FormGroup;

  constructor(private svc: AuthorService, private fb: FormBuilder, private cdr: ChangeDetectorRef) {}

  ngOnInit() { this.load(); this.buildForm(); }

  load() { this.svc.getAll().subscribe(d => { this.authors = d; this.cdr.detectChanges(); }); }

  buildForm(a?: Author) {
    this.form = this.fb.group({
      firstName:   [a?.firstName ?? '',   Validators.required],
      lastName:    [a?.lastName ?? '',    Validators.required],
      nationality: [a?.nationality ?? '', Validators.required],
      birthDate:   [a?.birthDate ? a.birthDate.substring(0, 10) : ''],
      biography:   [a?.biography ?? ''],
    });
  }

  openCreate() { this.editingId = null; this.buildForm(); this.showForm = true; }
  openEdit(a: Author) { this.editingId = a.id; this.buildForm(a); this.showForm = true; }
  cancel() { this.showForm = false; }

  save() {
    if (this.form.invalid) return;
    const dto = this.form.value;
    if (this.editingId) {
      this.svc.update(this.editingId, dto).subscribe(() => { this.load(); this.showForm = false; });
    } else {
      this.svc.create(dto).subscribe(() => { this.load(); this.showForm = false; });
    }
  }

  delete(id: number) {
    if (!confirm('¿Eliminar este autor?')) return;
    this.svc.delete(id).subscribe(() => this.load());
  }
}
