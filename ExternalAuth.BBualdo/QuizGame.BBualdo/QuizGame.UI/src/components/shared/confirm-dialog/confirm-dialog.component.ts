import { Component, Inject } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { DIALOG_DATA, DialogRef } from '@angular/cdk/dialog';

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [MatIcon],
  templateUrl: './confirm-dialog.component.html',
})
export class ConfirmDialogComponent {
  title: string = this.data.title;
  itemName: string = this.data.itemName;

  constructor(
    private dialogRef: DialogRef,
    @Inject(DIALOG_DATA) public data: any,
  ) {}

  close() {
    this.dialogRef.close();
  }

  confirm() {
    this.dialogRef.close(true);
  }
}
