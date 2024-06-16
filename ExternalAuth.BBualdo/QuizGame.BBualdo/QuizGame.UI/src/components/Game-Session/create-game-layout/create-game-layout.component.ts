import { Component, OnInit } from '@angular/core';
import { BackButtonComponent } from '../../shared/back-button/back-button.component';
import { NavigationStart, Router, RouterOutlet } from '@angular/router';
import { NewGameService } from '../../../services/new-game.service';

@Component({
  selector: 'app-create-game-layout',
  standalone: true,
  imports: [BackButtonComponent, RouterOutlet],
  templateUrl: './create-game-layout.component.html',
})
export class CreateGameLayout implements OnInit {
  constructor(
    private router: Router,
    private newGameService: NewGameService,
  ) {}

  ngOnInit() {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        if (!event.url.startsWith('/play')) {
          this.newGameService.clearGame();
        }
      }
    });
  }
}
