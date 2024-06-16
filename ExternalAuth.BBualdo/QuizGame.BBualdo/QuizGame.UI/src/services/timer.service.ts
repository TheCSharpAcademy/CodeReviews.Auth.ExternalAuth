import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Subscription, takeWhile, timer } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TimerService {
  private timeLeftSubject = new BehaviorSubject<number>(0);
  timeLeft$ = this.timeLeftSubject.asObservable();

  private timerSubscription: Subscription | null = null;

  start(time: number) {
    this.stop();
    this.timeLeftSubject.next(time);
    this.timerSubscription = timer(0, 1000)
      .pipe(
        map((elapsed) => time - elapsed),
        takeWhile((timeLeft) => timeLeft >= 0),
      )
      .subscribe((timeLeft) => this.timeLeftSubject.next(timeLeft));
  }

  stop() {
    if (this.timerSubscription) {
      this.timerSubscription.unsubscribe();
      this.timerSubscription = null;
    }
  }
}
