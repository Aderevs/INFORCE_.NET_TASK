import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthService } from './account/auth.service';

interface User{
  id:string;
  login:string;
  isAdmin:boolean;
}
interface ServerResponse{
  isLoggedIn:boolean;
  user:User;
}
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {

  constructor(private http: HttpClient, private authService: AuthService) {}

  ngOnInit() {
    
  }

  checkIfAuthorized(){
    this.http.get<ServerResponse>('/api/account/checkIfLoggedIn').subscribe(
      response=>{
        if(response.isLoggedIn){
          this.authService.login(response.user);
        }else{
          this.authService.logout();
        }
      }
    )
  }

  title = 'inforce_.net_task.client';
}
