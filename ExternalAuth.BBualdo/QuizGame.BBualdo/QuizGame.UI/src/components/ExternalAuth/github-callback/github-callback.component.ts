import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { UserService } from '../../../services/user.service';
import { AsyncPipe } from '@angular/common';
import { LoadingSpinnerComponent } from '../../shared/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-github-callback',
  standalone: true,
  imports: [AsyncPipe, LoadingSpinnerComponent],
  templateUrl: './github-callback.component.html',
})
export class GithubCallbackComponent implements OnInit {
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
        this.authService.handleGithubCallback(code).subscribe((token) => {
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
