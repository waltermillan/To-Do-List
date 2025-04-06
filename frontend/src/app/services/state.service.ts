import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GLOBAL_CONFIG } from '../config/config.global';

@Injectable({
  providedIn: 'root'
})
export class StateService {

  constructor(private http: HttpClient) { }

  getAll(): Observable<any[]> {
    const url = `${GLOBAL_CONFIG.apiBaseUrl}/states`;
    return this.http.get<any[]>(url);
  }

  add(state: any): Observable<any> {
    const url = `${GLOBAL_CONFIG.apiBaseUrl}/states`;
    return this.http.post(url, state);
  }

  delete(stateId: number): Observable<any> {
    const url = `${GLOBAL_CONFIG.apiBaseUrl}/states/${stateId}`;
    return this.http.delete<any>(url);
  }

  upadate(state: any, id:number): Observable<any> {
    const url = `${GLOBAL_CONFIG.apiBaseUrl}/states/${id}`;
    return this.http.put(url, state);
  }
}
