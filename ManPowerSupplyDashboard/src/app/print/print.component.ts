import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { DomSanitizer,SafeHtml } from '@angular/platform-browser';
import { MessagingService } from '../services/messaging.service';

@Component({
  selector: 'app-print',
  templateUrl: './print.component.html',
  styleUrls: ['./print.component.scss'],
  encapsulation:ViewEncapsulation.None
})
export class PrintComponent implements OnInit {

  constructor(private _messaging: MessagingService,
    private _sanitizer: DomSanitizer) { }

  content : SafeHtml = '';

  ngOnInit(): void {
    this._messaging.printTemplate.subscribe(template => {
      // this.content = template;
      this.content =  this._sanitizer.bypassSecurityTrustHtml(template);
      if (template.trim() != '')
        setTimeout(function(){window.print();},1000);
    });
  }

}
