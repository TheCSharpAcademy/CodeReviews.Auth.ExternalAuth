import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { GameReqDTO } from '../models/DTOs/GameReqDTO';

@Injectable({
  providedIn: 'root',
})
export class NewGameService {
  private readonly GAME_DATA_KEY = 'newGame';

  newGame: GameReqDTO = this.loadFromLocalStorage() || this.createEmptyGame();

  private newGameSubject = new BehaviorSubject<GameReqDTO>(this.newGame);
  newGame$ = this.newGameSubject.asObservable();

  updateUsername(username: string) {
    this.newGame = { ...this.newGame, username };
    this.newGameSubject.next(this.newGame);
    this.saveToLocalStorage(this.newGame);
  }

  updateQuiz(quizId: number) {
    this.newGame = { ...this.newGame, quizId };
    this.newGameSubject.next(this.newGame);
    this.saveToLocalStorage(this.newGame);
  }

  updateDifficulty(difficulty: 'Easy' | 'Medium' | 'Hard') {
    this.newGame = { ...this.newGame, difficulty };
    this.newGameSubject.next(this.newGame);
    this.saveToLocalStorage(this.newGame);
  }

  setDate() {
    this.newGame = { ...this.newGame, date: new Date().toISOString() };
    this.newGameSubject.next(this.newGame);
    this.saveToLocalStorage(this.newGame);
  }

  setScore(score: number) {
    this.newGame = { ...this.newGame, score };
    this.newGameSubject.next(this.newGame);
    this.saveToLocalStorage(this.newGame);
  }

  clearGame() {
    this.newGame = this.createEmptyGame();
    this.newGameSubject.next(this.newGame);
    localStorage.removeItem(this.GAME_DATA_KEY);
  }

  private createEmptyGame(): GameReqDTO {
    return {
      username: null,
      date: null,
      difficulty: 'Medium',
      score: null,
      quizId: null,
    };
  }

  private saveToLocalStorage(game: GameReqDTO) {
    localStorage.setItem(this.GAME_DATA_KEY, JSON.stringify(game));
  }

  private loadFromLocalStorage(): GameReqDTO | null {
    const data = localStorage.getItem(this.GAME_DATA_KEY);
    return data ? JSON.parse(data) : null;
  }
}
