import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, catchError, finalize, Observable, of } from 'rxjs';
import { Quiz } from '../models/Quiz';
import { url } from '../config/config';
import { ErrorsService } from './errors.service';
import { QuizDetailsDTO } from '../models/DTOs/QuizDetailsDTO';
import { QuizReqDTO } from '../models/DTOs/QuizReqDTO';
import { Dialog } from "@angular/cdk/dialog";
import { UnauthorizedDialogComponent } from "../components/shared/unauthorized-dialog/unauthorized-dialog.component";

@Injectable({
  providedIn: 'root',
})
export class QuizzesService {
  private isLoadingSubject = new BehaviorSubject<boolean>(false);
  isLoading$: Observable<boolean> = this.isLoadingSubject.asObservable();

  constructor(
    private http: HttpClient,
    private errorsService: ErrorsService,
    private dialog:Dialog
  ) {}

  getQuizzes(): Observable<Quiz[]> {
    this.errorsService.clear();
    this.isLoadingSubject.next(true);
    return this.http.get<Quiz[]>(url + 'Quizzes').pipe(
      catchError((error) => of(this.handleError(error, "get"))),
      finalize(() => this.isLoadingSubject.next(false)),
    );
  }

  getQuiz(id: number): Observable<QuizDetailsDTO> {
    this.errorsService.clear();
    this.isLoadingSubject.next(true);
    return this.http.get<QuizDetailsDTO>(url + 'Quizzes/' + id).pipe(
      catchError((error) => of(this.handleError(error, "get"))),
      finalize(() => this.isLoadingSubject.next(false)),
    );
  }

  addQuiz(quiz: QuizReqDTO): Observable<QuizReqDTO> {
    this.errorsService.clear();
    this.isLoadingSubject.next(true);
    return this.http.post<QuizReqDTO>(url + 'Quizzes', quiz).pipe(
      catchError((error) => of(this.handleError(error, "add"))),
      finalize(() => this.isLoadingSubject.next(false)),
    );
  }

  deleteQuiz(id: number): Observable<Quiz> {
    this.errorsService.clear();
    this.isLoadingSubject.next(true);
    return this.http.delete<Quiz>(url + 'Quizzes/' + id).pipe(
      catchError((error) => of(this.handleError(error, "delete"))),
      finalize(() => this.isLoadingSubject.next(false)),
    );
  }

  private handleError(error: HttpErrorResponse, operation:string): any {
    if (error.status === 0) {
      this.errorsService.add("Couldn't connect to Quizzes API.");
    }
    if (error.status === 500) {
      this.errorsService.add('Something went wrong. Try again later.');
    }
    if (error.status === 404) {
      this.errorsService.add('Quizzes not found.');
    }
    if (error.status === 401) {
      this.dialog.open(UnauthorizedDialogComponent, {data: {
        message: `You must be logged in to ${operation} Quizzes.`
        }})
    }
    return;
  }
}
