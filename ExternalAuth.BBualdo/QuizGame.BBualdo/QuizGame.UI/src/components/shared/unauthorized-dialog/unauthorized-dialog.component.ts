import { Component, Inject } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { DIALOG_DATA, DialogRef } from '@angular/cdk/dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-unauthorized-dialog',
  standalone: true,
  imports: [MatIcon],
  templateUrl: './unauthorized-dialog.component.html',
})
export class UnauthorizedDialogComponent {
  constructor(
    private dialogRef: DialogRef,
    @Inject(DIALOG_DATA) public data: any,
    private router: Router,
  ) {}

  redirectToLoginPage() {
    this.router.navigate(['login']);
    this.dialogRef.close();
  }

  close() {
    this.dialogRef.close(false);
  }
}
