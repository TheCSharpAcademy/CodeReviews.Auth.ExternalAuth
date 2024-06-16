import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'back-button',
  standalone: true,
  imports: [RouterLink, MatIcon],
  templateUrl: './back-button.component.html',
})
export class BackButtonComponent {
  @Input() previousPageName: string = 'Back';
  @Input() class: string = 'white';
}
