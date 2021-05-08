import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-side-bar',
  templateUrl: './side-bar.component.html',
  styleUrls: ['./side-bar.component.scss']
})
export class SideBarComponent implements OnInit {
  currentExpanded:string = 'Master'
  customCollapsedHeight:string = '50px';
  customExpandedHeight:string = '50px';
  
  constructor(private _router: Router) {
    if (_router.url.includes('master')) {
      this.currentExpanded = 'Master';
    }
    else if (this._router.url.includes('receipt-payment')){
      this.currentExpanded = 'ReceiptsPayments'
    }
    else if (this._router.url.includes('reports')){
      this.currentExpanded = 'Reports'
    }
  }

  ngOnInit(): void {
  }

  setExpanded(name: string) {
    this.currentExpanded = name;
  }

}
