import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SideMenuService {
  constructor() {
    this.collapseOccured = new BehaviorSubject<boolean>(false);
  }

  private _isCollapsed: boolean = false;
  private collapseOccured: BehaviorSubject<boolean>;

  get isCollapsed() {
    return this._isCollapsed;
  }

  set isCollapsed(value: boolean) {
    this._isCollapsed = value;
    this.setCollapseOccurred(value);
  }

  getIsCollapseOccurred(): Observable<boolean> {
    return this.collapseOccured.asObservable();
  }

  private setCollapseOccurred(newValue: boolean): void {
    this.collapseOccured.next(newValue);
  }
}
