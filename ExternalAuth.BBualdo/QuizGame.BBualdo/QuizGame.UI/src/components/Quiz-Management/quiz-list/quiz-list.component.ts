import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { Quiz } from '../../../models/Quiz';
import { QuizzesService } from '../../../services/quizzes.service';
import { ErrorsService } from '../../../services/errors.service';
import { AsyncPipe } from '@angular/common';
import { ErrorComponent } from '../../shared/error/error.component';
import { LoadingSpinnerComponent } from '../../shared/loading-spinner/loading-spinner.component';
import { RouterLink } from '@angular/router';
import { DataService } from '../../../services/data.service';

@Component({
  selector: 'app-quiz-list',
  standalone: true,
  imports: [AsyncPipe, ErrorComponent, LoadingSpinnerComponent, RouterLink],
  templateUrl: './quiz-list.component.html',
})
export class QuizListComponent {
  quizzes$: Observable<Quiz[] | null> = this.dataService.quizzes$;
  isLoading$: Observable<boolean> = this.quizzesService.isLoading$;
  isError$: Observable<boolean> = this.errorsService.isError$;

  constructor(
    private dataService: DataService,
    private quizzesService: QuizzesService,
    private errorsService: ErrorsService,
  ) {}

  retry(): void {
    this.dataService.refreshQuizzes();
  }
}
