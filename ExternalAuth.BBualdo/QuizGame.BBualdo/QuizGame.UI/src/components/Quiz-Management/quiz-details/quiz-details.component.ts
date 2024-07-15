import { Component, Input, OnInit } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { ErrorComponent } from '../../shared/error/error.component';
import { LoadingSpinnerComponent } from '../../shared/loading-spinner/loading-spinner.component';
import { BackButtonComponent } from '../../shared/back-button/back-button.component';
import { QuizDetailsDTO } from '../../../models/DTOs/QuizDetailsDTO';
import { Observable } from 'rxjs';
import { QuizzesService } from '../../../services/quizzes.service';
import { ErrorsService } from '../../../services/errors.service';
import { DataService } from '../../../services/data.service';
import { Dialog } from '@angular/cdk/dialog';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog.component';
import { Router } from '@angular/router';
import { Question } from '../../../models/Question';
import { AnswersListComponent } from '../answers-list/answers-list.component';

@Component({
  selector: 'app-quiz-details',
  standalone: true,
  imports: [
    AsyncPipe,
    ErrorComponent,
    LoadingSpinnerComponent,
    BackButtonComponent,
  ],
  templateUrl: './quiz-details.component.html',
})
export class QuizDetailsComponent implements OnInit {
  @Input() id: string = '';

  quiz$: Observable<QuizDetailsDTO | null> = this.dataService.quiz$;
  isLoading$: Observable<boolean> = this.quizzesService.isLoading$;
  isError$: Observable<boolean> = this.errorService.isError$;

  constructor(
    private quizzesService: QuizzesService,
    private errorService: ErrorsService,
    private dataService: DataService,
    private dialog: Dialog,
    private router: Router,
  ) {}

  ngOnInit(): void {
    if (this.id) {
      this.dataService.refreshQuiz(Number(this.id));
    }
  }

  retry() {
    this.quizzesService.getQuiz(Number(this.id)).subscribe();
  }

  openDeleteDialog(quizName: string) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete Quiz',
        itemName: quizName,
      },
    });

    dialogRef.closed.subscribe((value) => {
      if (value === true) {
        this.quizzesService.deleteQuiz(Number(this.id)).subscribe(() => {
          this.dataService.refreshQuizzes();
        });
        this.router.navigate(['quiz-management']);
      }
    });
  }

  showAnswers(question: Question) {
    this.dialog.open(AnswersListComponent, { data: { question: question } });
  }
}
