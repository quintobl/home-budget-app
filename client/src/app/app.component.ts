import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  http = inject(HttpClient);
  title = 'client';
  debits: any;

  ngOnInit(): void {
    this.http.get('http://localhost:5112/api/debits').subscribe({
      next: response => this.debits = response,
      error: error => console.log(error),
      complete: () => console.log('Request has completed')
    })
  }
}
