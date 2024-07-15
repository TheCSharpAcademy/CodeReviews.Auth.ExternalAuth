import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, catchError, finalize, Observable, of } from 'rxjs';
import { ErrorsService } from './errors.service';
import { GameReqDTO } from '../models/DTOs/GameReqDTO';
import { url } from '../config/config';
import { PaginatedGames } from '../models/DTOs/PaginatedGames';
import { Game } from '../models/Game';
import { UnauthorizedDialogComponent } from '../components/shared/unauthorized-dialog/unauthorized-dialog.component';
import { Dialog } from '@angular/cdk/dialog';

@Injectable({
  providedIn: 'root',
})
export class GamesService {
  private isLoadingSubject = new BehaviorSubject<boolean>(false);
  isLoading$: Observable<boolean> = this.isLoadingSubject.asObservable();

  constructor(
    private http: HttpClient,
    private errorsService: ErrorsService,
    private dialog: Dialog,
  ) {}

  addGame(game: GameReqDTO): Observable<GameReqDTO> {
    this.errorsService.clear();
    this.isLoadingSubject.next(true);
    return this.http.post(url + 'Games', game).pipe(
      catchError((error) => of(this.handleError(error, 'add'))),
      finalize(() => this.isLoadingSubject.next(false)),
    );
  }

  getGames(page: number = 1, pageSize: number = 5): Observable<PaginatedGames> {
    this.errorsService.clear();
    this.isLoadingSubject.next(true);
    return this.http
      .get<PaginatedGames>(url + `Games/?page=${page}&pageSize=${pageSize}`)
      .pipe(
        catchError((error) => of(this.handleError(error, 'get'))),
        finalize(() => this.isLoadingSubject.next(false)),
      );
  }

  deleteGame(id: number): Observable<Game> {
    this.errorsService.clear();
    this.isLoadingSubject.next(true);
    return this.http.delete(url + 'Games/' + id).pipe(
      catchError((error) => of(this.handleError(error, 'delete'))),
      finalize(() => this.isLoadingSubject.next(false)),
    );
  }

  deleteAllGames(): Observable<Game[]> {
    this.errorsService.clear();
    this.isLoadingSubject.next(true);
    return this.http.delete<Game[]>(url + 'Games/').pipe(
      catchError((error) => of(this.handleError(error, 'delete'))),
      finalize(() => this.isLoadingSubject.next(false)),
    );
  }

  private handleError(error: HttpErrorResponse, operation: string): any {
    if (error.status === 0) {
      this.errorsService.add("Couldn't connect to Games API.");
    }
    if (error.status === 500) {
      this.errorsService.add('Something went wrong. Try again later.');
    }
    if (error.status === 404) {
      this.errorsService.add('Games not found.');
    }
    if (error.status === 401) {
      this.dialog.open(UnauthorizedDialogComponent, {
        data: {
          message: `You must be logged in to ${operation} Games.`,
        },
      });
    }
    return;
  }
}
