import { Component, OnInit } from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgClass } from '@angular/common';
import { NewGameService } from '../../../services/new-game.service';
import { Router } from '@angular/router';
import { GameReqDTO } from '../../../models/DTOs/GameReqDTO';

@Component({
  selector: 'app-enter-username (deprecated)',
  standalone: true,
  imports: [ReactiveFormsModule, NgClass],
  templateUrl: './enter-username.component.html',
})
export class EnterUsernameComponent implements OnInit {
  game: GameReqDTO | null = null;

  username = new FormControl<string>('', [
    Validators.required,
    Validators.maxLength(50),
  ]);

  constructor(
    private newGameService: NewGameService,
    private router: Router,
  ) {}

  ngOnInit() {
    this.newGameService.newGame$.subscribe((game) => (this.game = game));
    if (this.game) {
      this.loadUsername();
    }
  }

  proceed() {
    this.username.markAsTouched();
    if (this.username.valid) {
      this.newGameService.updateUsername(this.username.value!);
      this.router.navigate(['play/quiz'], { skipLocationChange: true });
    }
  }

  loadUsername() {
    if (this.game) {
      this.username.setValue(this.game.username);
    }
  }
}
