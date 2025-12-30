import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { LoginRequest } from '../../model/login-request.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {

  loginForm!: FormGroup;

  constructor(private service: AuthService, private formBuilder: FormBuilder) {}

  ngOnInit(): void {
      this.loginForm = this.formBuilder.group({
        email: [null, [Validators.required, Validators.email]],
        password: [null, Validators.required]
      })
  }

  async submit(): Promise<void> {
    const loginRequest : LoginRequest = {
      email: this.loginForm.value.email || "",
      password: this.loginForm.value.password || "",
    };
  
    this.service.login(loginRequest).subscribe({
      next: (response) => {
        console.log('Login successful', response);
        // maybe navigate or save token
      },
      error: (err) => {
        console.error('Login failed', err);
      }
    });
  }
}
