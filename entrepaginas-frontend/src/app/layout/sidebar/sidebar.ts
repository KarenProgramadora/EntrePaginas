import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-sidebar',
  imports: [RouterLink, RouterLinkActive, MatListModule, MatIconModule],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.scss',
})
export class Sidebar {
  navItems = [
    { label: 'Dashboard', icon: 'dashboard', route: '/dashboard' },
    { label: 'Libros', icon: 'menu_book', route: '/books' },
    { label: 'Autores', icon: 'person', route: '/authors' },
    { label: 'Miembros', icon: 'people', route: '/members' },
    { label: 'Préstamos', icon: 'swap_horiz', route: '/loans' },
  ];
}
