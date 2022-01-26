import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NzNotificationService } from 'ng-zorro-antd';
import { Observable, throwError, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { TemplateType } from '../../../models/template-type.model';

const url = '/template/';

@Injectable({
  providedIn: 'root'
})
export class TemplateTypesService {

  private needToRefreshTemplateType: boolean = true;
  private templateTypes: TemplateType[];

  constructor(private http: HttpClient, private message: NzNotificationService) { }

  getTemplateTypes(): Observable<TemplateType[]> {
    const endpoint = `${url}GetTemplateTypes`;

    if (!this.needToRefreshTemplateType) return of (this.templateTypes);

    return this.http.get<any>(endpoint).pipe(
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
      .post<any>(endpoint, { name: name })
      .pipe(catchError(this.handleError('createTemplateType', null, true)));
  }

  updateTemplateType(type: TemplateType): Observable<TemplateType> {
    const endpoint = `${url}UpdateTemplateType`;

    this.needToRefreshTemplateType = true;

    return this.http
      .put<any>(endpoint, type)
      .pipe(catchError(this.handleError('updateTemplateType', null, true)));
  }

  deleteTemplateType(typeId: number): Observable<boolean> {
    const endpoint = `${url}DeleteTemplateType?typeId=${typeId}`;

    this.needToRefreshTemplateType = true;

    return this.http
      .delete<boolean>(endpoint)
      .pipe(catchError(this.handleError('deleteTemplateType', null, true)));
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
