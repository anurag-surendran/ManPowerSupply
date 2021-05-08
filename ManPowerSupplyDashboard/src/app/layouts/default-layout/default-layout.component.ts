import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-default-layout',
  templateUrl: './default-layout.component.html',
  styleUrls: ['./default-layout.component.scss']
})
export class DefaultLayoutComponent implements OnInit {
  sideBarOpen = true;
  IsShowSettings = false;
  constructor() { }

  ngOnInit(): void {
  }

  sidebarToggle(event){
    this.sideBarOpen = !this.sideBarOpen;
  }


}
