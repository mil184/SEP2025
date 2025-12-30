import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { LoginRequest } from '../model/login-request.model';
import { RegisterRequest } from '../model/register-request.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7133/auth/';

  constructor(private http: HttpClient, private router: Router, @Inject(PLATFORM_ID) private platformId: Object) { }

  private isBrowser(): boolean {
    return isPlatformBrowser(this.platformId);
  }

  login(loginRequest: LoginRequest) {
    return this.http.post<any>(this.apiUrl + 'login', loginRequest).pipe(
      tap(response => {
        if (this.isBrowser()) {
          localStorage.setItem('accessToken', response.accessToken);
          localStorage.setItem('refreshToken', response.refreshToken);
        }

        this.router.navigate(['/']);
      })
    );
  }

  register(registerRequest: RegisterRequest) {
    return this.http.post<any>(this.apiUrl + 'register', registerRequest).subscribe({
      next: () => {
        this.router.navigate(['/']);
      },
      error: (err) => {
        console.error('Registration failed:', err);
      }
    });
  }
}
