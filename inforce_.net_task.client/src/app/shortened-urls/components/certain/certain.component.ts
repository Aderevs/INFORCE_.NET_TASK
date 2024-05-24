import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';


interface ShortenedUrl {
  originalUrl: string;
  shortUrl: string;
  createdDate: Date;
  user: User;
}
interface User {
  login: string;
}
@Component({
  selector: 'app-certain',
  templateUrl: './certain.component.html',
  styleUrl: './certain.component.css'
})
export class CertainComponent implements OnInit {
  id!: string;
  public certainUrl: ShortenedUrl | undefined;
  constructor(private route: ActivatedRoute, private http: HttpClient) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.id = params['id'];
      this.http.get<ShortenedUrl>('/api/url/getUrl/' + this.id).subscribe(
        response => {
          this.certainUrl = response;
          console.log('successfully get ceertain url', response);
        },
        error => {
          console.log('error while get certain url', error);
        }
      )
    });
  }
}
