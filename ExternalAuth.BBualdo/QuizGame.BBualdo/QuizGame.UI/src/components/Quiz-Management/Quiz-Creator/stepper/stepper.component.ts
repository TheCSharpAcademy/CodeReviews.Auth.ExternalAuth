import { Component, OnInit } from '@angular/core';
import { QuizCreatorService } from '../../../../services/quiz-creator.service';
import { QuizReqDTO } from '../../../../models/DTOs/QuizReqDTO';
import { Router } from '@angular/router';
import { NgClass } from '@angular/common';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { QuizzesService } from '../../../../services/quizzes.service';
import { DataService } from '../../../../services/data.service';

@Component({
  selector: 'stepper',
  standalone: true,
  imports: [NgClass, ReactiveFormsModule],
  templateUrl: './stepper.component.html',
})
export class StepperComponent implements OnInit {
  currentStep: number = 1;
  quiz: QuizReqDTO | null = null;

  qAndAFormGroup = new FormGroup({
    question: new FormControl<string>('', [
      Validators.required,
      Validators.minLength(6),
      Validators.maxLength(200),
    ]),
    answer1: new FormControl<string>('', [
      Validators.required,
      Validators.maxLength(50),
    ]),
    answer2: new FormControl<string>('', [
      Validators.required,
      Validators.maxLength(50),
    ]),
    answer3: new FormControl<string>('', [
      Validators.required,
      Validators.maxLength(50),
    ]),
    answer4: new FormControl<string>('', [
      Validators.required,
      Validators.maxLength(50),
    ]),
  });

  constructor(
    private quizCreatorService: QuizCreatorService,
    private router: Router,
    private quizzesService: QuizzesService,
    private dataService: DataService,
  ) {}

  ngOnInit(): void {
    this.quizCreatorService.quiz$.subscribe((quiz) => (this.quiz = quiz));
    if (this.quiz) {
      this.loadFormData();
    }
  }

  setCorrect(answerIndex: number) {
    const currentQuestion = this.quiz?.questions[this.currentStep - 1];
    if (currentQuestion) {
      currentQuestion.answers.forEach((answer, index) => {
        answer.isCorrect = answerIndex === index;
      });
    }
  }

  next() {
    this.qAndAFormGroup.markAllAsTouched();
    if (this.currentStep === this.quiz?.questions.length) {
      this.saveQuestion();
      this.submit();
      return;
    }
    if (this.qAndAFormGroup.valid) {
      this.saveQuestion();
      this.qAndAFormGroup.reset();
      this.currentStep++;
      this.loadFormData();
    }
  }

  previous() {
    if (this.currentStep === 1) {
      this.quizCreatorService.clearQuiz();
      this.router.navigate(['quiz-management/create']);
      return;
    }
    this.saveQuestion();
    this.qAndAFormGroup.reset();
    this.currentStep--;
    this.loadFormData();
  }

  submit() {
    if (this.quiz) {
      this.quizzesService.addQuiz(this.quiz).subscribe(() => {
        this.dataService.refreshQuizzes();
      });
      this.router.navigate(['quiz-management']);
    }
  }

  private loadFormData() {
    const question = this.quiz?.questions[this.currentStep - 1];
    if (question) {
      this.qAndAFormGroup.setValue({
        question: question.name || '',
        answer1: question.answers[0].name || '',
        answer2: question.answers[1].name || '',
        answer3: question.answers[2].name || '',
        answer4: question.answers[3].name || '',
      });
    }
  }

  private saveQuestion() {
    const currentQuestion = this.quiz?.questions[this.currentStep - 1];
    const formValues = this.qAndAFormGroup.value;
    if (currentQuestion) {
      currentQuestion.name = formValues.question!;
      currentQuestion.answers = [
        {
          name: formValues.answer1!,
          isCorrect: currentQuestion.answers[0].isCorrect,
        },
        {
          name: formValues.answer2!,
          isCorrect: currentQuestion.answers[1].isCorrect,
        },
        {
          name: formValues.answer3!,
          isCorrect: currentQuestion.answers[2].isCorrect,
        },
        {
          name: formValues.answer4!,
          isCorrect: currentQuestion.answers[3].isCorrect,
        },
      ];

      this.quizCreatorService.updateQuiz(this.quiz!);
    }
  }
}
