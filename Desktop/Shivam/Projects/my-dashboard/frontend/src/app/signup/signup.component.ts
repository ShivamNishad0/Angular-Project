import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
  imports: [CommonModule, FormsModule]
})
export class SignupComponent {
  name = '';
  email = '';
  password = '';
  errorMessage = '';

  constructor(private http: HttpClient, private router: Router) {}

  signup() {
    const user = { name: this.name, email: this.email, password: this.password };
    this.http.post('/api/user/signup', user).subscribe({
      next: () => this.router.navigate(['/login']),
      error: err => this.errorMessage = err.error || 'Signup failed'
    });
  }
}
