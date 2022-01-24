import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable()
export class ConfigService {
  _apiURI: string;

  constructor() {
    if (environment.production) {
      this._apiURI = 'api'; // change to api url
    } else {
      this._apiURI = 'api'; // change to api url
    }
  }

  getApiURI() {
    return this._apiURI;
  }
}
