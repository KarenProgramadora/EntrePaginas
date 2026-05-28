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
import { MemberService } from '../../services/member';
import { Member } from '../../models/models';

@Component({
  selector: 'app-members',
  imports: [
    CommonModule, ReactiveFormsModule,
    MatTableModule, MatCardModule, MatButtonModule, MatIconModule,
    MatFormFieldModule, MatInputModule, MatSelectModule, MatChipsModule
  ],
  templateUrl: './members.html',
  styles: [`
    .page-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px; }
    .form-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 12px; }
    .form-actions { display: flex; gap: 8px; justify-content: flex-end; margin-top: 8px; }
    table { width: 100%; }
    .chip-active    { background: #c8e6c9; color: #1b5e20; padding: 2px 8px; border-radius: 12px; font-size: 0.8rem; }
    .chip-suspended { background: #ffcdd2; color: #b71c1c; padding: 2px 8px; border-radius: 12px; font-size: 0.8rem; }
    .chip-expired   { background: #ffe0b2; color: #e65100; padding: 2px 8px; border-radius: 12px; font-size: 0.8rem; }
  `]
})
export class Members implements OnInit {
  members: Member[] = [];
  displayedColumns = ['fullName', 'email', 'phone', 'statusName', 'membershipDate', 'actions'];
  showForm = false;
  editingId: number | null = null;
  form!: FormGroup;

  statuses = [
    { value: 0, label: 'Activo' },
    { value: 1, label: 'Suspendido' },
    { value: 2, label: 'Expirado' },
  ];

  constructor(private svc: MemberService, private fb: FormBuilder, private cdr: ChangeDetectorRef) {}

  ngOnInit() { this.load(); this.buildForm(); }

  load() { this.svc.getAll().subscribe(d => { this.members = d; this.cdr.detectChanges(); }); }

  buildForm(m?: Member) {
    this.form = this.fb.group({
      firstName: [m?.firstName ?? '', Validators.required],
      lastName:  [m?.lastName ?? '',  Validators.required],
      email:     [m?.email ?? '',     [Validators.required, Validators.email]],
      phone:     [m?.phone ?? '',     Validators.required],
      status:    [m?.status ?? 0,     Validators.required],
    });
  }

  chipClass(status: number) {
    return status === 0 ? 'chip-active' : status === 1 ? 'chip-suspended' : 'chip-expired';
  }

  openCreate() { this.editingId = null; this.buildForm(); this.showForm = true; }
  openEdit(m: Member) { this.editingId = m.id; this.buildForm(m); this.showForm = true; }
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
    if (!confirm('¿Eliminar este miembro?')) return;
    this.svc.delete(id).subscribe(() => this.load());
  }
}
