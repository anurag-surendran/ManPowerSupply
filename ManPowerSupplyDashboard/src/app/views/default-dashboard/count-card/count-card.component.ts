import { Component, HostBinding, Input, OnInit } from '@angular/core';
import { CountCardDataModel } from '../models/count-card-data-model';

@Component({
  selector: 'app-count-card',
  templateUrl: './count-card.component.html',
  styleUrls: ['./count-card.component.scss']
})
export class CountCardComponent implements OnInit {

  @Input() cardData : CountCardDataModel;
  constructor() { }

  ngOnInit(): void {
    this.bgColorKey = this.cardData.cardBackgroundColor;
  }
  @HostBinding('style.backgroundColor') bgColorKey=''
  
}
