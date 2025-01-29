import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from './nav/nav.component';
import { AccountService } from './_services/account.service';
import { HomeComponent } from './home/home.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, NavComponent, HomeComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  http = inject(HttpClient);
  private accountService = inject(AccountService);
  title = 'client';
  debits: any;
  users: any;

  ngOnInit(): void {
    //debugger;
    this.getUsers();
    this.setCurrentUser();
    this.getDebits();
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (!userString) return;
    const user = JSON.parse(userString);
    this.accountService.currentUser.set(user);
  }

  getUsers() {
    this.http.get('http://localhost:5112/api/users').subscribe({
      next: (response) => (this.users = response),
      error: (error) => console.log(error),
      complete: () => console.log('Request has completed'),
    });
  }

  getDebits() {
    this.http.get('http://localhost:5112/api/debits').subscribe({
      next: (response) => (this.debits = response),
      error: (error) => console.log(error),
      complete: () => console.log('Request has completed'),
    });
  }
}
