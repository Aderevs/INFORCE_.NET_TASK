import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../../account/auth.service';
import { Observable } from 'rxjs';

interface ShortenedUrl {
  id: string;
  originalUrl: string;
  shortUrl: string;
}

@Component({
  selector: 'app-all',
  templateUrl: './all.component.html',
  styleUrl: './all.component.css'
})
export class AllComponent implements OnInit {
  public allUrls: ShortenedUrl[] = [];
  constructor(private http: HttpClient, private authService: AuthService) { }

  ngOnInit(): void {
    this.http.get<ShortenedUrl[]>('/api/url/GetAllUrls').subscribe(
      (result) => {
        this.allUrls = result;
        console.log(this.authService.isLoggedIn());
        if (this.authService.isLoggedIn()) {
          let containerDiv = document.getElementById('form-container');
          let form = document.createElement("form");

          let input = document.createElement("input");
          input.type = "text";
          input.name = "originalUrl";
          input.placeholder = "enter your url";

          let hiddenTime = document.createElement("input");
          hiddenTime.type = "hidden";
          hiddenTime.name = "createdDate";
          const currentTime: Date = new Date();
          const dateString: string = currentTime.toISOString().split('T')[0];
          hiddenTime.value = dateString;

          let submitButton = document.createElement("button");
          submitButton.type = "submit";
          submitButton.textContent = "Submit";

          // Додавання поля вводу та кнопки до форми
          form.appendChild(input);
          form.appendChild(hiddenTime);
          form.appendChild(submitButton);

          form.addEventListener('submit', (event) => {
            event.preventDefault(); // Запобігає стандартній поведінці submit (перезавантаження сторінки)
            let formValues: any = {};
            // Отримання всіх елементів input в формі
            const inputs = form.querySelectorAll('input');
            // Проходження по кожному елементу input та додавання його значення в об'єкт formValues
            inputs.forEach((input: HTMLInputElement) => {
              formValues[input.name] = input.value;
            });

            this.onSubmit(formValues); // Виклик методу з об'єктом значень форми
          });
          // Додавання форми всередину formContainer
          containerDiv?.appendChild(form);
        }

      },
      (error) => {
        console.error(error);
      }
    );


  }
  onSubmit(value: any) {
    this.addUrl(value).subscribe(
      response => {
        console.log('url successfully added');
        this.allUrls.push(response);
      },
      error => {
        console.error("error during add url", error);
      }
    )
  }
  addUrl(data: any): Observable<any> {
    const apiUrl = '/api/url/CreateShortenedUrl';
    return this.http.post<any>(apiUrl, data);
  }
}
