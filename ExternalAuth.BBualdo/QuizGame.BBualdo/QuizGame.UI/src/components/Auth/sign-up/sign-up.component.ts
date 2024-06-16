import { Component } from '@angular/core';
import { BackButtonComponent } from '../../shared/back-button/back-button.component';
import { Router, RouterLink } from '@angular/router';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { matchPasswordValidator } from '../../../validators/match-password.validator';
import { NgClass } from '@angular/common';
import { emailValidator } from '../../../validators/email.validator';
import { passwordValidator } from '../../../validators/password.validator';
import { UserRegister } from '../../../models/UserRegister';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [BackButtonComponent, RouterLink, ReactiveFormsModule, NgClass],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.css',
})
export class SignUpComponent {
  registerForm: FormGroup = new FormGroup(
    {
      username: new FormControl('', [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(40),
      ]),
      email: new FormControl('', [Validators.required, emailValidator]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(6),
        passwordValidator,
      ]),
      confirmPassword: new FormControl('', [Validators.required]),
    },
    {
      validators: matchPasswordValidator,
    },
  );

  constructor(
    private userService: UserService,
    private router: Router,
  ) {
    this.userService.isLoggedIn$.subscribe((isLoggedIn) => {
      if (isLoggedIn) {
        this.router.navigate(['']);
      }
    });
  }

  submit() {
    this.registerForm.markAllAsTouched();
    const formValues = this.registerForm.value;

    if (this.registerForm.valid) {
      const user: UserRegister = {
        username: formValues.username,
        email: formValues.email,
        password: formValues.password,
      };

      this.userService
        .registerUser(user)
        .subscribe(() => this.backAfterLogin());
    }
  }

  private backAfterLogin() {
    this.userService.isLoggedIn$.subscribe((isLoggedIn) => {
      if (isLoggedIn) this.router.navigate(['']);
    });
  }
}
