import { Component, OnInit } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { LoadingSpinnerComponent } from '../../shared/loading-spinner/loading-spinner.component';
import { AuthService } from '../../../services/auth.service';
import { UserService } from '../../../services/user.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-facebook-callback',
  standalone: true,
  imports: [AsyncPipe, LoadingSpinnerComponent],
  templateUrl: './facebook-callback.component.html',
})
export class FacebookCallbackComponent implements OnInit {
  isLoading$ = this.authService.isLoading$;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router,
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe((params) => {
      const code = params['code'];
      if (code) {
        this.authService.handleFacebookCallback(code).subscribe((token) => {
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
