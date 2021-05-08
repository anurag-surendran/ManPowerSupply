import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

//Angular Material Modules 
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

//Custom Components
import { DefaultLayoutComponent } from './default-layout/default-layout.component';


//Custom Modules
import { SharedModule } from '../shared/shared.module';
import { ViewsModule } from '../views/views.module';



@NgModule({
  declarations: [
    DefaultLayoutComponent
  ],
  imports: [
    CommonModule,
    RouterModule,

    //Angular Materials
    MatSidenavModule,
    MatDividerModule,
    MatProgressSpinnerModule,

    //Local Custom Modules
    SharedModule,
    ViewsModule
  ]
})
export class LayoutsModule { }
