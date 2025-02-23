import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class DebitService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;

  // Todo: Update these with returning types
  public getCategories(): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl + 'categories');
  }

  public getDescriptions(): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl + 'descriptions');
  }

  public getAccounts(): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl + 'accounts');
  }

  public saveDebit(debit: any): Observable<any> {
    console.log('Sending payload:', debit); // Debugging line
    return this.http.post<any>(`${this.baseUrl}debits/add`, debit);
  }
}
