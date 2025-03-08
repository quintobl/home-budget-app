import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { Description } from '../_models/description';

@Injectable({
  providedIn: 'root',
})
export class DebitService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;

  public getCategories(): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl + 'categories');
  }

  public getAccounts(): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl + 'accounts');
  }

  public getDescriptionsByCategory(
    categoryId: number
  ): Observable<Description[]> {
    return this.http.get<Description[]>(
      `${this.baseUrl}descriptions/by-category/${categoryId}`
    );
  }

  public saveDebit(debit: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}debits/add`, debit, {
      headers: { 'Content-Type': 'application/json' },
    });
  }

  public addCategory(categoryName: string): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}categories/add`, {
      name: categoryName,
    });
  }

  public addDescription(
    descriptionName: string,
    categoryId: number
  ): Observable<Description> {
    return this.http
      .post<Description>(`${this.baseUrl}descriptions/add`, {
        name: descriptionName,
        categoryId: categoryId > 0 ? categoryId : null,
      })
      .pipe(
        tap((newDescription) =>
          console.log('New Description Added:', newDescription)
        )
      );
  }
}
