import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [CommonModule, FormsModule]
})
export class LoginComponent {
  email = '';
  password = '';
  errorMessage = '';

  constructor(private http: HttpClient, private router: Router) {}

  login() {
    const credentials = { email: this.email, password: this.password };
    this.http.post('/api/user/login', credentials, { responseType: 'text' }).subscribe({
      next: token => {
        localStorage.setItem('jwt', token);
        this.router.navigate(['/dashboard']);
      },
      error: err => this.errorMessage = err.error || 'Login failed'
    });
  }
}
