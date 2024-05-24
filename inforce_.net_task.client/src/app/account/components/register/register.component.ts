import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators, AbstractControl, ValidatorFn  } from '@angular/forms';
import { mustMatch } from './customValidator';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../../auth.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})

export class RegisterComponent {
  registerForm: FormGroup;
  constructor(private http: HttpClient, private authService: AuthService, private router: Router) {
    this.registerForm = new FormGroup({
      login: new FormControl('', Validators.required),
      password: new FormControl('', [Validators.required, Validators.minLength(6)]),
      passwordConfirm: new FormControl('', Validators.required)
    }, { validators: mustMatch('password', 'passwordConfirm') });
  }
  OnSubmit(): void {
    if (this.registerForm.invalid) {
      return;
    }
    this.register(this.registerForm.value).subscribe(
      response => {
        this.authService.login(response);
        console.log('Registration successful', response);
        this.router.navigate(['/urls/all']);
      },
      error => {
        console.error('Registration error', error);
        let containerDiv = document.getElementById('registration-error-container');
        if (containerDiv != null) {
          containerDiv.textContent = error.error.error;
        }
      }
    );
  }
  register(data: any): Observable<any> {
    const apiUrl = '/api/account/register'; 
    return this.http.post<any>(apiUrl, data);
  }
}