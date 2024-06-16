import { Component, OnInit } from '@angular/core';
import { AsyncPipe, NgClass } from '@angular/common';
import { ErrorComponent } from '../../shared/error/error.component';
import { LoadingSpinnerComponent } from '../../shared/loading-spinner/loading-spinner.component';
import { Router, RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { NewGameService } from '../../../services/new-game.service';

@Component({
  selector: 'app-select-difficulty',
  standalone: true,
  imports: [
    AsyncPipe,
    ErrorComponent,
    LoadingSpinnerComponent,
    RouterLink,
    ReactiveFormsModule,
    NgClass,
  ],
  templateUrl: './select-difficulty.component.html',
})
export class SelectDifficultyComponent implements OnInit {
  selectedDifficulty: 'Easy' | 'Medium' | 'Hard' | null = null;

  constructor(
    private router: Router,
    private newGameService: NewGameService,
  ) {}

  ngOnInit() {
    this.newGameService.newGame$.subscribe(
      (game) => (this.selectedDifficulty = game.difficulty),
    );
  }

  selectDifficulty(difficulty: 'Easy' | 'Medium' | 'Hard') {
    this.selectedDifficulty = difficulty;
    this.newGameService.updateDifficulty(difficulty);
  }

  back() {
    this.router.navigate(['play'], { skipLocationChange: true });
  }

  play() {
    this.router.navigate(['play/session']);
  }
}
