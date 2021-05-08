import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { MessagingService } from './messaging.service';
import { ManPowerService } from './man-power.service';
import { HttpClientModule } from '@angular/common/http';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { OnlyNumbersDirective } from './only-numbers.directive';
import { TableFilterPipe } from './table-filter.pipe';



@NgModule({
  declarations: [OnlyNumbersDirective, TableFilterPipe],
  imports: [
    CommonModule,
    HttpClientModule,

    MatSnackBarModule
  ],
  providers:[
    MessagingService,
    ManPowerService,
    DatePipe
  ],
  exports:[    
    OnlyNumbersDirective,
    TableFilterPipe
  ]
})
export class ServicesModule { }
