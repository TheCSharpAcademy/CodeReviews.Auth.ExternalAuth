import { Component, Inject } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { DIALOG_DATA, DialogRef } from '@angular/cdk/dialog';
import { Question } from '../../../models/Question';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-answers-list',
  standalone: true,
  imports: [MatIcon, NgClass],
  templateUrl: './answers-list.component.html',
})
export class AnswersListComponent {
  question: Question = this.data.question;

  constructor(
    @Inject(DIALOG_DATA) public data: any,
    private dialogRef: DialogRef,
  ) {}

  close() {
    this.dialogRef.close();
  }
}
