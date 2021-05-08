import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/authentication/models/user-model';
import { AuthenticationService } from 'src/app/authentication/services/authentication.service';
import { MessagingService } from 'src/app/services/messaging.service';
import { DashboardService } from './dashboard.service';
import { AttendanceDashboardModel } from './models/attendance-dashboard-model';
import { CountCardDataModel } from './models/count-card-data-model';

@Component({
  selector: 'app-default-dashboard',
  templateUrl: './default-dashboard.component.html',
  styleUrls: ['./default-dashboard.component.scss']
})

export class DefaultDashboardComponent implements OnInit {

  user: User;
  isCurrentMonthAttendance : Boolean = true;

  customerBalanceCardModel: CountCardDataModel = {
    headerName: 'Customer Receipt',
    headIconColor: 'red',
    cardBackgroundColor: '#82eeda73'
  }
  employeeBalanceCardModel: CountCardDataModel = {
    headerName: 'Employee Payment',
    headIconColor: '#2852c9',
    cardBackgroundColor: '#e482ee73'
  }
  currentBalanceCardModel: CountCardDataModel = {
    headerName: 'Current Balance',
    headIconColor: '#8428c9',
    cardBackgroundColor: '#82eeb273'
  }

  constructor(private _service: DashboardService,
    private _messaging: MessagingService,
    private authenticationService: AuthenticationService) {

    this._messaging.changeHeaderRouteMessage('Dashboard');
    this.authenticationService.user.subscribe(x => this.user = x);
  }



  ngOnInit(): void {
    this.getCustomerBalance();
    this.getEmployeeBalance();
    this.getCurrentAmount();
  }

  getCustomerBalance() {
    this._service.getCustomerBalance().subscribe((x: number) => {
      this.customerBalanceCardModel.count = x;
    })
  }

  getEmployeeBalance() {
    this._service.getEmployeeBalance().subscribe((x: number) => {
      this.employeeBalanceCardModel.count = x;
    })
  }

  getCurrentAmount() {
    this._service.getCurrentAmount().subscribe((x: number) => {
      this.currentBalanceCardModel.count = x;
    })
  }
}



export interface Tile {
  color: string;
  cols: number;
  rows: number;
  text: string;
}
