import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { UserRegister } from '../models/UserRegister';
import {
  BehaviorSubject,
  catchError,
  finalize,
  Observable,
  of,
  tap,
} from 'rxjs';
import { url } from '../config/config';
import { ErrorsService } from './errors.service';
import { ErrorDetails } from '../models/ErrorDetails';
import { Dialog } from '@angular/cdk/dialog';
import { ErrorDialogComponent } from '../components/shared/error-dialog/error-dialog.component';
import { UserLogin } from '../models/DTOs/UserLogin';
import { UserDTO } from '../models/DTOs/UserDTO';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  // TODO: Make it don't disappear after reload
  private userSubject = new BehaviorSubject<UserDTO | null>(null);
  user$ = this.userSubject.asObservable();
  private isLoadingSubject = new BehaviorSubject<boolean>(false);
  isLoading$ = this.isLoadingSubject.asObservable();
  private isLoggedInSubject = new BehaviorSubject<boolean>(this.hasToken());
  isLoggedIn$ = this.isLoggedInSubject.asObservable();

  constructor(
    private http: HttpClient,
    private errorsService: ErrorsService,
    private dialog: Dialog,
  ) {}

  getCurrentUser(): Observable<UserDTO | null> {
    this.errorsService.clear();
    this.isLoadingSubject.next(true);
    return this.http.get<UserDTO | null>(url + 'Account/currentUser').pipe(
      tap((user) => this.userSubject.next(user)),
      catchError((error) => of(this.handleErrors(error))),
      finalize(() => {
        this.isLoadingSubject.next(true);
      }),
    );
  }

  registerUser(user: UserRegister): Observable<UserRegister> {
    this.errorsService.clear();
    this.isLoadingSubject.next(true);
    return this.http.post(url + 'Account/register', user).pipe(
      catchError((error) => of(this.handleErrors(error))),
      finalize(() => {
        this.isLoadingSubject.next(false);
        this.checkLoginStatus();
        this.getCurrentUser().subscribe();
      }),
    );
  }

  loginUser(user: UserLogin): Observable<UserLogin> {
    this.errorsService.clear();
    this.isLoadingSubject.next(true);
    return this.http.post(url + 'Account/login', user).pipe(
      catchError((error) => of(this.handleErrors(error))),
      finalize(() => {
        this.isLoadingSubject.next(false);
        this.checkLoginStatus();
        this.getCurrentUser().subscribe();
      }),
    );
  }

  logout() {
    this.errorsService.clear();
    this.isLoadingSubject.next(true);
    return this.http.post(url + 'Account/logout', {}).pipe(
      catchError((error) => of(this.handleErrors(error))),
      finalize(() => {
        this.isLoadingSubject.next(false);
        this.userSubject.next(null);
        this.checkLoginStatus();
      }),
    );
  }

  checkLoginStatus() {
    const isLoggedIn = this.hasToken();
    this.isLoggedInSubject.next(isLoggedIn);
  }

  private handleErrors(error: HttpErrorResponse): any {
    if (error.status === 200) {
      return;
    }

    if (error.status === 500) {
      this.errorsService.add('Something bad happened. Try again later.');
    }

    if (error.status === 401) {
      this.errorsService.add(error.error);
    }

    if (error.status === 400) {
      error.error.forEach((err: ErrorDetails) =>
        this.errorsService.add(err.description),
      );
    }
    this.dialog.open(ErrorDialogComponent);
    return;
  }

  private hasToken(): boolean {
    return document.cookie.includes('QuizGameToken');
  }
}
