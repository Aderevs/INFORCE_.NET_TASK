import { Injectable } from '@angular/core';

interface User {
  id: string;
  login: string;
  isAdmin: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private isAuthenticatedKey = 'isAuthenticated';
  private userKey = 'user';

  constructor() { }

  login(user: User): void {
    localStorage.setItem(this.isAuthenticatedKey, 'true');
    localStorage.setItem(this.userKey, JSON.stringify(user));
  }

  get userLogin(): string | undefined {
    const userData = localStorage.getItem(this.userKey);
    if (userData) {
      const user = JSON.parse(userData) as User;
      return user.login;
    }
    return undefined;
  }

  get isUserAdmin(): boolean | undefined {
    const userData = localStorage.getItem(this.userKey);
    if (userData) {
      const user = JSON.parse(userData) as User;
      return user.isAdmin;
    }
    return undefined;
  }

  logout(): void {
    localStorage.removeItem(this.isAuthenticatedKey);
    localStorage.removeItem(this.userKey);
  }

  isLoggedIn(): boolean {
    return localStorage.getItem(this.isAuthenticatedKey) === 'true';
  }
}
