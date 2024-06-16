import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { QuizReqDTO } from '../models/DTOs/QuizReqDTO';

@Injectable({
  providedIn: 'root',
})
export class QuizCreatorService {
  private readonly QUIZ_STORAGE_KEY = 'quizCreatorData';

  private quizSubject: BehaviorSubject<QuizReqDTO | null> =
    new BehaviorSubject<QuizReqDTO | null>(this.loadQuizFromStorage());
  quiz$ = this.quizSubject.asObservable();

  updateQuiz(quiz: QuizReqDTO) {
    this.quizSubject.next(quiz);
    this.saveQuizToStorage(quiz);
  }

  clearQuiz() {
    this.quizSubject.next(null);
    localStorage.removeItem(this.QUIZ_STORAGE_KEY);
  }

  private saveQuizToStorage(quiz: QuizReqDTO) {
    localStorage.setItem(this.QUIZ_STORAGE_KEY, JSON.stringify(quiz));
  }

  private loadQuizFromStorage(): QuizReqDTO | null {
    const data = localStorage.getItem(this.QUIZ_STORAGE_KEY);
    return data ? JSON.parse(data) : null;
  }
}
