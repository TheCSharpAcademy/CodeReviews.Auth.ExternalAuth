import { Component, inject } from '@angular/core';
import { BackButtonComponent } from '../../shared/back-button/back-button.component';
import { RouterOutlet } from '@angular/router';
import { NgOptimizedImage } from '@angular/common';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-auth-layout',
  standalone: true,
  imports: [BackButtonComponent, RouterOutlet, NgOptimizedImage],
  templateUrl: './auth-layout.component.html',
})
export class AuthLayoutComponent {
  private authService = inject(AuthService);

  loginWithGoogle() {
    this.authService.loginWithGoogle();
  }

  loginWithFacebook() {
    this.authService.loginWithFacebook();
  }

  loginWithMicrosoft() {
    this.authService.loginWithMicrosoft();
  }

  loginWithGithub() {
    this.authService.loginWithGithub();
  }

  loginWithTwitter() {
    this.authService.loginWithTwitter();
  }
}
