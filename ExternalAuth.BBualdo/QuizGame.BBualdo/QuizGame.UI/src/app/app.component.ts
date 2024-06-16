import { Component, OnChanges, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit, OnChanges {
  constructor(private userService: UserService) {}

  ngOnInit() {
    this.userService.checkLoginStatus();
  }

  ngOnChanges() {
    this.userService.checkLoginStatus();
  }
}
