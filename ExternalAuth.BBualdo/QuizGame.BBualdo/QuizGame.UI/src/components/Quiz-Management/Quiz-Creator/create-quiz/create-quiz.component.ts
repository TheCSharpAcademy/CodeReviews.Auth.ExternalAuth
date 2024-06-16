import { Component, OnInit } from '@angular/core';
import { BackButtonComponent } from '../../../shared/back-button/back-button.component';
import { NgClass } from '@angular/common';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { StepperComponent } from '../stepper/stepper.component';
import { QuizCreatorService } from '../../../../services/quiz-creator.service';
import { QuestionReqDTO } from '../../../../models/DTOs/QuestionReqDTO';
import { AnswerReqDTO } from '../../../../models/DTOs/AnswerReqDTO';
import { UnauthorizedDialogComponent } from '../../../shared/unauthorized-dialog/unauthorized-dialog.component';
import { UserService } from '../../../../services/user.service';
import { Dialog } from '@angular/cdk/dialog';

@Component({
  selector: 'app-create-quiz',
  standalone: true,
  imports: [
    BackButtonComponent,
    ReactiveFormsModule,
    FormsModule,
    NgClass,
    RouterLink,
    StepperComponent,
  ],
  templateUrl: './create-quiz.component.html',
})
export class CreateQuizComponent implements OnInit {
  quizInfoFormGroup = new FormGroup({
    name: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(50),
    ]),
    questionsAmount: new FormControl(5, [
      Validators.required,
      Validators.min(5),
      Validators.max(10),
    ]),
  });

  constructor(
    private quizCreatorService: QuizCreatorService,
    private router: Router,
    private userService: UserService,
    private dialog: Dialog,
  ) {}

  ngOnInit() {
    this.userService.isLoggedIn$.subscribe((isLoggedIn) => {
      if (!isLoggedIn) {
        this.dialog
          .open(UnauthorizedDialogComponent, {
            data: {
              message: `You must be logged in to create Quizzes.`,
            },
          })
          .closed.subscribe((result) => {
            if (result === false) this.router.navigate(['']);
          });
      }
    });
  }

  proceed() {
    this.quizInfoFormGroup.markAllAsTouched();
    const formValues = this.quizInfoFormGroup.value;
    if (this.quizInfoFormGroup.valid) {
      const answersArray: AnswerReqDTO[] = Array.from(
        { length: 4 },
        (_, index) => ({
          name: '',
          isCorrect: index === 0,
        }),
      );

      const questionsArray: QuestionReqDTO[] = Array.from(
        { length: formValues.questionsAmount! },
        () => ({
          name: '',
          answers: answersArray,
        }),
      );
      this.quizCreatorService.updateQuiz({
        name: formValues.name!,
        questions: questionsArray,
      });
      this.router.navigate(['quiz-management/create/steps']);
    }
  }
}
