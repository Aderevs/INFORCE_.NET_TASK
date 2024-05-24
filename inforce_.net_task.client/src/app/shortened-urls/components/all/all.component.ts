import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../../account/auth.service';
import { Observable } from 'rxjs';

interface ShortenedUrl {
  id: string;
  originalUrl: string;
  shortUrl: string;
  showDeleteButton: boolean;
  user: User;
}
interface User {
  login: string;
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

        if (this.authService.isLoggedIn()) {
          if (this.authService.isUserAdmin) {
            this.allUrls.forEach(url => {
              url.showDeleteButton = true;
            });
          } else {
            this.allUrls.forEach(url => {
              if (url.user.login === this.authService.userLogin) {
                url.showDeleteButton = true;
              }
            });
          }

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

          form.appendChild(input);
          form.appendChild(hiddenTime);
          form.appendChild(submitButton);

          form.addEventListener('submit', (event) => {
            event.preventDefault();
            let formValues: any = {};
            const inputs = form.querySelectorAll('input');

            inputs.forEach((input: HTMLInputElement) => {
              formValues[input.name] = input.value;
            });

            this.onSubmit(formValues);
          });
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
        let newUrl:ShortenedUrl = response;
        newUrl.showDeleteButton = true;
        
        this.allUrls.push(response);
      },
      error => {
        console.error("error during add url", error);
        let containerDiv = document.getElementById('new-url-error-container');
        if (containerDiv != null) {
          containerDiv.textContent = error.error.error;
        }
      }
    )
  }
  addUrl(data: any): Observable<any> {
    const apiUrl = '/api/url/CreateShortenedUrl';
    return this.http.post<any>(apiUrl, data);
  }

  OnUrlDelete(event: Event, urlId: string): void {
    event.preventDefault();
    this.http.delete('/api/url/deleteUrl/' + urlId).subscribe(
      response => {
        console.log('url successfully deleted', response);
        console.log('urls before', this.allUrls);
        console.log('id', urlId);

        const index = this.allUrls.findIndex(url => url.id === urlId);

        if (index !== -1) {
          this.allUrls.splice(index, 1);
        }
        console.log('urls after', this.allUrls);
      },
      error => {
        console.log('error while deleting url', error);
      }
    )
  }
}
