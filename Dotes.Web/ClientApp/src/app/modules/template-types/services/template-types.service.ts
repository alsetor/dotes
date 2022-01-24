import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NzNotificationService } from 'ng-zorro-antd';
import { Observable, throwError, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { TemplateType } from '../../../models/template-type.model';

const url = '/api/template/';

@Injectable({
  providedIn: 'root'
})
export class TemplateTypesService {
  headers: HttpHeaders;

  private needToRefreshTemplateType: boolean = true;
  private templateTypes: TemplateType[];

  constructor(private http: HttpClient, private message: NzNotificationService) {
    this.headers = new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('Authorization', `bearer ${localStorage.getItem('auth_template_token')}`);
  }

  getTemplateTypes(): Observable<TemplateType[]> {
    const endpoint = `${url}GetTemplateTypes`;

    if (!this.needToRefreshTemplateType) return of (this.templateTypes);

    return this.http
      .get<any>(endpoint, { headers: this.headers })
      .pipe(
        map((types) => {
          this.templateTypes = types;
          this.needToRefreshTemplateType = false;
          return types;
        }),
        catchError(this.handleError('getTemplateTypes', []))
      );
  }

  createTemplateType(name: string): Observable<TemplateType> {
    const endpoint = `${url}CreateTemplateType`;

    this.needToRefreshTemplateType = true;

    return this.http
      .post<any>(endpoint, { name: name }, { headers: this.headers })
      .pipe(catchError(this.handleError('createTemplateType', null, true)));
  }

  updateTemplateType(type: TemplateType): Observable<TemplateType> {
    const endpoint = `${url}UpdateTemplateType`;

    this.needToRefreshTemplateType = true;

    return this.http
      .put<any>(endpoint, type, { headers: this.headers })
      .pipe(catchError(this.handleError('updateTemplateType', null, true)));
  }

  private handleError<T>(operation = 'operation', result?: T, withContinueThrow = false) {
    return (error: any): Observable<T> => {
      console.error(error);
      console.log(`${operation} failed: ${error.message}`);
      this.message.error('Error', error.message);
      return withContinueThrow ? throwError(error) : of(result as T);
    };
  }
}
