import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.scss']
})
export class SpinnerComponent implements OnInit {

  @Input() diameter?: number;
  constructor() {
    this.diameter = this.diameter == null ? 30 : this.diameter;
  }

  ngOnInit(): void {
  }

}
