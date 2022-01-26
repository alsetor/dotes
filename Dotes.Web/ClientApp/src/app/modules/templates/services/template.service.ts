import { Injectable } from '@angular/core';
import { Template } from 'src/app/models/template.model';
import { HttpClient } from '@angular/common/http';
import { NzNotificationService } from 'ng-zorro-antd';
import { Params, ActivatedRoute } from '@angular/router';
import { Observable, throwError, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { saveAs } from 'file-saver';

const url = '/template/';

@Injectable({
  providedIn: 'root'
})
export class TemplateService {

  constructor(private http: HttpClient, private message: NzNotificationService, private route: ActivatedRoute) { }

  getTemplates(params: Params = this.route.snapshot.queryParams): Observable<Template[]> {
    const endpoint = `${url}GetTemplates`;

    return this.http.get<any>(endpoint, { params })
      .pipe(catchError(this.handleError('getTemplates', [])));
  }

  deleteTemplate(id: number): Observable<any> {
    const endpoint = `${url}DeleteTemplate`;

    return this.http.post<any>(endpoint, { id: id })
      .pipe(catchError(this.handleError('deleteTemplate', null, true)));
  }

  getTemplate(id: number): Observable<Template> {
    const endpoint = `${url}GetTemplate?id=${id}`;

    return this.http.get<any>(endpoint)
      .pipe(catchError(this.handleError('getTemplate', null)));
  }

  getFileByTemplateId(templateId: number, fileName: string) {
    const endpoint = `${url}getFileByTemplateId?templateId=${templateId}`;

    this.http.get(endpoint, { responseType: 'blob' }).subscribe(
      (blob) => {
        saveAs(blob, fileName);
      },
      (error) => this.message.create('error', `Can't download file`, '')
    );
  }

  createTemplate(documentTemplate: Template): Observable<any> {
    const endpoint = `${url}CreateTemplate`;

    return this.http
      .post<any>(endpoint, documentTemplate)
      .pipe(catchError(this.handleError('createTemplate', null, true)));
  }

  updateTemplate(documentTemplate: Template): Observable<any> {
    const endpoint = `${url}UpdateTemplate`;

    return this.http
      .post<any>(endpoint, documentTemplate)
      .pipe(catchError(this.handleError('updateTemplate', null, true)));
  }

  private handleError<T>(operation = 'operation', result?: T, withContinueThrow = false) {
    return (error: any): Observable<T> => {
      console.error(error);
      this.message.error('Error', error.message);
      console.log(`${operation} failed: ${error.message}`);
      return withContinueThrow ? throwError(error) : of(result as T);
    };
  }  
}
