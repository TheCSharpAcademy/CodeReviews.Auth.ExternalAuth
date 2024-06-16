import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { UserService } from '../../../services/user.service';
import { ErrorsService } from '../../../services/errors.service';
import { AsyncPipe } from '@angular/common';
import { LoadingSpinnerComponent } from '../../shared/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-microsoft-callback',
  standalone: true,
  imports: [AsyncPipe, LoadingSpinnerComponent],
  templateUrl: './microsoft-callback.component.html',
})
export class MicrosoftCallbackComponent implements OnInit {
  isLoading$ = this.authService.isLoading$;

  constructor(
    private route: ActivatedRoute,
    private authService: AuthService,
    private userService: UserService,
    private router: Router,
    private errorsService: ErrorsService,
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      const code = params['code'];
      const error = params['error'];
      if (error) {
        this.errorsService.add('Microsoft login failed. Try again later.');
        this.errorsService.openDialog();
      }
      if (code) {
        this.authService.handleMicrosoftCallback(code).subscribe((token) => {
          if (token) {
            this.userService.checkLoginStatus();
          }
          this.router.navigate(['/']);
        });
      } else {
        this.router.navigate(['auth/login']);
      }
    });
  }
}
