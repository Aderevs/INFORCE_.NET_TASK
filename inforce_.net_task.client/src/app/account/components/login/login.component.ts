import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../../auth.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm: FormGroup;
  constructor(private http: HttpClient, private authService: AuthService, private router: Router) {
    this.loginForm = new FormGroup({
      login: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    })
  }

  OnSubmit(): void {
    if (this.loginForm.invalid) {
      return;
    }
    this.register(this.loginForm.value).subscribe(
      response => {
        this.authService.login(response);
        console.log('Login successful', response);
        this.router.navigate(['/urls/all']);
      },
      error => {
        console.error('Login error', error);
        let containerDiv = document.getElementById('login-error-container');
        if (containerDiv != null) {
          containerDiv.textContent = error.error.error;
        }
      }
    );
  }
  register(data: any): Observable<any> {
    const apiUrl = '/api/account/login';
    return this.http.post<any>(apiUrl, data);

  }
}
