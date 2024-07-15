import { Component } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { DialogRef } from '@angular/cdk/dialog';
import { ErrorsService } from '../../../services/errors.service';

@Component({
  selector: 'app-error-dialog',
  standalone: true,
  imports: [MatIcon],
  templateUrl: './error-dialog.component.html',
})
export class ErrorDialogComponent {
  constructor(
    private dialogRef: DialogRef,
    public errorsService: ErrorsService,
  ) {}

  close() {
    this.dialogRef.close();
  }
}
