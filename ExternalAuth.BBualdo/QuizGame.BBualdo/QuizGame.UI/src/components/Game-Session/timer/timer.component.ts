import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { NgStyle } from '@angular/common';
import { TimerService } from '../../../services/timer.service';

@Component({
  selector: 'countdown-timer',
  standalone: true,
  imports: [NgStyle],
  templateUrl: './timer.component.html',
})
export class TimerComponent implements OnInit, OnChanges, OnDestroy {
  @Input() time: number | null = null;
  timeLeft: number | null = null;

  @Input() answersChecked: boolean = false;

  @Output() timeUpEmitter = new EventEmitter();

  constructor(private timerService: TimerService) {}

  ngOnInit() {
    this.start();

    this.timerService.timeLeft$.subscribe((timeLeft) => {
      this.timeLeft = timeLeft;
      if (timeLeft === 0) {
        setTimeout(() => this.timeUpEmitter.emit('timesUp'), 1000);
      }
    });
  }

  ngOnChanges() {
    if (this.answersChecked) {
      this.stop();
    } else {
      this.start();
    }
  }

  ngOnDestroy() {
    this.stop();
  }

  start() {
    if (this.time !== null) {
      this.timerService.start(this.time);
    }
  }

  stop() {
    this.timerService.stop();
  }
}
