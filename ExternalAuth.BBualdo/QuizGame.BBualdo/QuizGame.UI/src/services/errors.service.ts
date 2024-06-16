import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Dialog } from '@angular/cdk/dialog';
import { ErrorDialogComponent } from '../components/shared/error-dialog/error-dialog.component';

@Injectable({
  providedIn: 'root',
})
export class ErrorsService {
  errors: string[] = [];
  private isErrorSubject = new BehaviorSubject<boolean>(false);
  isError$: Observable<boolean> = this.isErrorSubject.asObservable();

  constructor(private dialog: Dialog) {}

  add(error: string) {
    this.isErrorSubject.next(true);
    this.errors.push(error);
  }

  openDialog() {
    this.dialog.open(ErrorDialogComponent);
  }

  clear() {
    this.errors = [];
    this.isErrorSubject.next(false);
  }
}
