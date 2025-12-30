import { Component } from '@angular/core';
import { RegisterRequest } from '../../model/register-request.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  registerForm!: FormGroup;
  hide1 = true;
  hide2 = true;
  passwordsMatchError = false;
  passwordLengthError = false;
  passwordCharacterError = false;
  
  
  constructor(private service: AuthService, private formBuilder: FormBuilder){}

  ngOnInit(): void {
      this.registerForm = this.formBuilder.group({
        name: [null, [Validators.required]],
        surname: [null, Validators.required],
        email: [null, [Validators.required, Validators.email]],
        password1: [null, Validators.required],
        password2: [null, Validators.required]
      });
  }

  async submit(): Promise<void> {
    // if (!this.checkPasswords()){
    //   return;
    // }

    // if (!this.checkPasswordLength()){
    //   return;
    // }

    // if (!this.checkSpecialCharacter()){
    //   return;
    // }
    
    const registerRequest : RegisterRequest = {
      name: this.registerForm.value.name || "",
      surname: this.registerForm.value.surname || "",
      email: this.registerForm.value.email || "",
      password: this.registerForm.value.password || ""
    };

    this.service.register(registerRequest);
  }

  checkPasswords(): boolean{
    const passwordsMatch = this.registerForm.value.password === this.registerForm.value.password2;
    this.passwordsMatchError = !passwordsMatch;
    return passwordsMatch;
  }

  checkPasswordLength(): boolean {
    const password = this.registerForm.value.password1;
    let isLengthValid = true;
    if (password != null && password.length != null) {
      isLengthValid = password.length >= 8;
    }
    this.passwordLengthError = !isLengthValid;
    return isLengthValid;
  }
  
  checkSpecialCharacter(): boolean {
    const specialCharacters = /[!@#$%^&*(),.?":{}|<>]/;
    let isValid = specialCharacters.test(this.registerForm.value.password1!);
    this.passwordCharacterError = !isValid;
    return isValid;
  }
}
