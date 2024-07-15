import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Quiz } from '../models/Quiz';
import { QuizzesService } from './quizzes.service';
import { QuizDetailsDTO } from '../models/DTOs/QuizDetailsDTO';
import { PaginatedGames } from '../models/DTOs/PaginatedGames';
import { GamesService } from './games.service';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  private quizzesSubject = new BehaviorSubject<Quiz[] | null>(null);
  quizzes$ = this.quizzesSubject.asObservable();

  private quizSubject = new BehaviorSubject<QuizDetailsDTO | null>(null);
  quiz$ = this.quizSubject.asObservable();

  private gamesSubject = new BehaviorSubject<PaginatedGames | null>(null);
  games$ = this.gamesSubject.asObservable();

  constructor(
    private quizzesService: QuizzesService,
    private gamesService: GamesService,
  ) {
    this.refreshQuizzes();
    this.refreshGames();
  }

  refreshQuizzes() {
    this.quizzesService
      .getQuizzes()
      .subscribe((quizzes) => this.quizzesSubject.next(quizzes));
  }

  refreshQuiz(id: number) {
    this.quizzesService
      .getQuiz(id)
      .subscribe((quiz) => this.quizSubject.next(quiz));
  }

  refreshGames(page: number = 1, pageSize: number = 5) {
    this.gamesService
      .getGames(page, pageSize)
      .subscribe((games) => this.gamesSubject.next(games));
  }
}
