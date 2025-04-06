import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GLOBAL_CONFIG } from '../config/config.global';


@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  getAll(): Observable<any[]> {
    const url = `${GLOBAL_CONFIG.apiBaseUrl}/users`;
    return this.http.get<any[]>(url);
  }

  getById(id:number): Observable<any> {
    const url = `${GLOBAL_CONFIG.apiBaseUrl}/users/${id}`;
    return this.http.get<any>(url);
  }

  add(user: any): Observable<any> {
    const url = `${GLOBAL_CONFIG.apiBaseUrl}/users`;
    return this.http.post(url, user);
  }

  delete(userId: number): Observable<any> {
    const url = `${GLOBAL_CONFIG.apiBaseUrl}/users/${userId}`;
    return this.http.delete<any>(url);
  }

  upadate(user: any, id:number): Observable<any> {
    const url = `${GLOBAL_CONFIG.apiBaseUrl}/users/${id}`;
    return this.http.put(url, user);
  }
}
