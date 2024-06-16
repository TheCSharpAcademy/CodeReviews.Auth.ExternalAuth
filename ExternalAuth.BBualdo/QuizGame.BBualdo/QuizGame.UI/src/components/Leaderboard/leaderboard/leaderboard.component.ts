import { Component } from '@angular/core';
import { BackButtonComponent } from '../../shared/back-button/back-button.component';
import { RouterOutlet } from '@angular/router';
import { GamesService } from '../../../services/games.service';
import { ErrorsService } from '../../../services/errors.service';
import { Observable } from 'rxjs';
import { AsyncPipe, formatDate } from '@angular/common';
import { LoadingSpinnerComponent } from '../../shared/loading-spinner/loading-spinner.component';
import { ErrorComponent } from '../../shared/error/error.component';
import { DataService } from '../../../services/data.service';
import { MatIcon } from '@angular/material/icon';
import { Dialog } from '@angular/cdk/dialog';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog.component';
import { Game } from '../../../models/Game';

@Component({
  selector: 'app-leaderboard',
  standalone: true,
  imports: [
    BackButtonComponent,
    RouterOutlet,
    AsyncPipe,
    LoadingSpinnerComponent,
    ErrorComponent,
    MatIcon,
  ],
  templateUrl: './leaderboard.component.html',
})
export class LeaderboardComponent {
  currentPage = 1;

  games$ = this.dataService.games$;
  isLoading$: Observable<boolean> = this.gamesService.isLoading$;
  isError$: Observable<boolean> = this.errorsService.isError$;

  constructor(
    private gamesService: GamesService,
    private errorsService: ErrorsService,
    private dataService: DataService,
    private dialog: Dialog,
  ) {}

  openDeleteDialog(game: Game) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { title: 'Delete Game', itemName: 'Game' },
    });

    dialogRef.closed.subscribe((value) => {
      if (value === true) {
        this.gamesService.deleteGame(game.id).subscribe(() => {
          this.dataService.refreshGames();
        });
      }
    });
  }

  openDeleteAllDialog() {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { title: 'Clear History', itemName: 'All Games' },
    });

    dialogRef.closed.subscribe((value) => {
      if (value === true) {
        this.gamesService.deleteAllGames().subscribe(() => {
          this.dataService.refreshGames();
        });
      }
    });
  }

  nextPage(total: number) {
    if (this.currentPage === total) {
      return;
    }
    this.currentPage++;
    this.dataService.refreshGames(this.currentPage);
  }

  previousPage() {
    if (this.currentPage === 1) {
      return;
    }
    this.currentPage--;
    this.dataService.refreshGames(this.currentPage);
  }

  retry() {
    this.dataService.refreshGames();
  }

  protected readonly formatDate = formatDate;
}
