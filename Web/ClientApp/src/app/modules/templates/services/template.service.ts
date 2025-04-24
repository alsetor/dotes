import { Injectable } from '@angular/core';
import { Template } from 'src/app/models/template.model';
import { HttpClient } from '@angular/common/http';
import { NzNotificationService } from 'ng-zorro-antd';
import { Params, ActivatedRoute } from '@angular/router';
import { Observable, throwError, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { saveAs } from 'file-saver';

const url = 'api/template';

@Injectable({
  providedIn: 'root'
})
export class TemplateService {

  constructor(private http: HttpClient, private message: NzNotificationService, private route: ActivatedRoute) { }

  getTemplates(params: Params = this.route.snapshot.queryParams): Observable<Template[]> {
    const endpoint = `${url}`;

    return this.http.get<any>(endpoint, { params })
      .pipe(catchError(this.handleError('getTemplates', [])));
  }

  deleteTemplate(id: string): Observable<any> {
    const endpoint = `${url}/${id}`;

    return this.http.delete<any>(endpoint)
      .pipe(catchError(this.handleError('deleteTemplate', null, true)));
  }

  getTagsFromTemplate(id: string): Observable<any> {
    const endpoint = `${url}/${id}/tags`;

    return this.http.get<any>(endpoint)
      .pipe(catchError(this.handleError('tags', null, true)));
  }

  getTemplate(id: string): Observable<Template> {
    const endpoint = `${url}/${id}`;

    return this.http.get<any>(endpoint)
      .pipe(catchError(this.handleError('getTemplate', null)));
  }

  downloadTemplate(templateId: string, fileName: string) {
    const endpoint = `${url}/${templateId}/download`;

    this.http.get(endpoint, { responseType: 'blob' }).subscribe(
      (blob) => {
        saveAs(blob, fileName);
      },
      (error) => this.message.create('error', `Can't download file`, '')
    );
  }

  createTemplate(documentTemplate: Template): Observable<any> {
    const endpoint = `${url}/CreateTemplate`;

    return this.http
      .post<any>(endpoint, documentTemplate)
      .pipe(catchError(this.handleError('createTemplate', null, true)));
  }

  updateTemplate(documentTemplate: Template): Observable<any> {
    const endpoint = `${url}/UpdateTemplate`;

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
