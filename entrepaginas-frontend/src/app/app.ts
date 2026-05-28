import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet, RouterLink, RouterLinkActive,
    MatToolbarModule, MatSidenavModule, MatListModule,
    MatIconModule, MatButtonModule
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  title = 'Entre Páginas';

  navItems = [
    { label: 'Dashboard',  icon: 'dashboard',    route: '/dashboard' },
    { label: 'Libros',     icon: 'menu_book',     route: '/books' },
    { label: 'Autores',    icon: 'person',        route: '/authors' },
    { label: 'Miembros',   icon: 'people',        route: '/members' },
    { label: 'Préstamos',  icon: 'swap_horiz',    route: '/loans' },
  ];
}
