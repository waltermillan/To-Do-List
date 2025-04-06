import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { GLOBAL_CONFIG } from '../config/config.global';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loggedInKey = 'isLoggedIn';
  private userNameKey = 'userName';
  private userNameSubject = new BehaviorSubject<string>('');  // BehaviorSubject to store the user name
  userName$ = this.userNameSubject.asObservable();  // expose the observable for other components to subscribe to

  constructor(private http: HttpClient) {
    // Only access localStorage if we are in the browser
    if (typeof window !== 'undefined') {
      const storedUserName = localStorage.getItem(this.userNameKey);
      if (storedUserName) {
        this.userNameSubject.next(storedUserName);
      }
    }
  }

  login(username: string, password: string): Observable<any> {
    const url = `${GLOBAL_CONFIG.apiBaseUrl}/users/login`;
    const params = new HttpParams().set('usr', username).set('psw', password);
    
    return this.http.post(url, null, { params }).pipe(
      tap(() => {
        // Only interact with localStorage if in the browser
        if (typeof window !== 'undefined') {

          /*
            localStorage is a browser storage API that allows data to be stored persistently between 
            browser sessions. Data stored in localStorage is retained even when the browser is closed and 
            reopened or when the page is reloaded.
          */

          localStorage.setItem(this.loggedInKey, 'true');  // Save session state
          localStorage.setItem(this.userNameKey, username);  // Save user name
          this.userNameSubject.next(username);  // We Emitt the new user name
        }
      })
    );
  }

  logout() {
    // Only interact with localStorage if in the browser
    if (typeof window !== 'undefined') {
      localStorage.removeItem(this.loggedInKey);
      localStorage.removeItem(this.userNameKey);
      this.userNameSubject.next(''); 
    }
  }

  isLoggedIn(): boolean {
    // Only interact with localStorage if in the browser
    if (typeof window !== 'undefined') {
      return localStorage.getItem(this.loggedInKey) === 'true';
    }
    return false;
  }
}
