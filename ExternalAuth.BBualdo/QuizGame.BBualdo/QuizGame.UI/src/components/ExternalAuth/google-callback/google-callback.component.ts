import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { AsyncPipe } from '@angular/common';
import { LoadingSpinnerComponent } from '../../shared/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-google-callback',
  standalone: true,
  imports: [AsyncPipe, LoadingSpinnerComponent],
  templateUrl: './google-callback.component.html',
})
export class GoogleCallbackComponent implements OnInit {
  isLoading$ = this.authService.isLoading$;

  constructor(
    private route: ActivatedRoute,
    private authService: AuthService,
    private userService: UserService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      const code = params['code'];
      if (code) {
        this.authService.handleGoogleCallback(code).subscribe((token) => {
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
