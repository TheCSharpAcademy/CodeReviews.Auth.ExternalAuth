import { Component, OnInit } from '@angular/core';
import { NewGameService } from '../../../services/new-game.service';
import { GameReqDTO } from '../../../models/DTOs/GameReqDTO';
import { Router } from '@angular/router';

@Component({
  selector: 'app-game-results',
  standalone: true,
  imports: [],
  templateUrl: './game-results.component.html',
})
export class GameResultsComponent implements OnInit {
  game: GameReqDTO | null = null;
  summaryText = '';

  constructor(
    private newGameService: NewGameService,
    private router: Router,
  ) {}

  ngOnInit() {
    this.newGameService.newGame$.subscribe((game) => (this.game = game));
    this.validateResults();
    this.generateSummaryText();
  }

  playAgain() {
    this.newGameService.clearGame();
    this.router.navigate(['play']);
  }

  mainMenu() {
    this.router.navigate(['']);
  }

  private generateSummaryText() {
    const score = this.game!.score!;
    if (score <= 20) {
      this.summaryText = 'Try again';
    } else if (score <= 40) {
      this.summaryText = 'Not bad';
    } else if (score <= 60) {
      this.summaryText = 'Good job';
    } else if (score <= 80) {
      this.summaryText = 'Nicely done';
    } else {
      this.summaryText = 'Amazing work';
    }
  }

  private validateResults() {
    if (!this.game?.date) {
      this.router.navigate(['play']);
    }
  }
}
