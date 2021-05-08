import { Component, OnInit } from '@angular/core';
import { EmployeeModel, EmployeePaymentModel } from 'src/app/models/man-power-models';
import { MessagingService } from 'src/app/services/messaging.service';
import { MatDialog } from '@angular/material/dialog';
import { ManPowerService } from 'src/app/services/man-power.service';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';
import { AddEditEmployeeComponent } from './add-edit-employee/add-edit-employee.component';
import { AddEditEmployeePaymentComponent } from '../receipts-payments/employee-payments/add-edit-employee-payment/add-edit-employee-payment.component';
import { NavigationExtras, Router } from '@angular/router';
import { PrintColumn, PrintService } from 'src/app/services/print.service';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.scss']
})
export class EmployeesComponent implements OnInit {

  employees: EmployeeModel[];
  public searchText: string;
  filterMetadata = { count: 0, sums: [] };

  constructor(private _messaging: MessagingService,
    public dialog: MatDialog,
    private router: Router,
    private _service: ManPowerService,
    private gFuncs: GlobalFunctionsService,
    private _print: PrintService) { }

  ngOnInit(): void {
    this._messaging.changeHeaderRouteMessage('Master/Employees');
    this.fnLoadAllEmployees();
  }

  fnLoadAllEmployees(): void {
    this._service.getAllEmployees().subscribe((result: any[]) => {
      this.employees = result;
    })
  }

  addOrUpdateEmployee(employeeData?: EmployeeModel): void {
    const addEditDialog = this.dialog.open(AddEditEmployeeComponent, {
      width: '700px',
      panelClass: 'myapp-no-padding-dialog',
      height: '520px',
      data: employeeData
    });

    addEditDialog.afterClosed().subscribe((result: EmployeeModel) => {
      this._messaging.changeHeaderRouteMessage('Master/Employees');
      if (result != null) {
        let message = result.name + ' has been ' + (employeeData == null ? 'added' : 'updated') + ' successfully'
        this.gFuncs.openSnackBar(message);
        if (employeeData == null) {
          this.searchText = '';
        }
        this.fnLoadAllEmployees();
      }
    });
  }

  fnEmployeePayment(employee) {
    const addEditDialog = this.dialog.open(AddEditEmployeePaymentComponent, {
      width: '600px',
      panelClass: 'myapp-no-padding-dialog',
      height: '400px',
      data: { type: 'NewPayment', employee: employee }
    });

    addEditDialog.afterClosed().subscribe((result: EmployeePaymentModel) => {
      this._messaging.changeHeaderRouteMessage('Master/Employees');
      if (result != null) {
        let message = 'Payment from ' + result.employeeName + ' has been added successfully'
        this.gFuncs.openSnackBar(message);
        this.fnLoadAllEmployees();
      }
    });
  }

  fnViewPaymentTransactions(employee: EmployeeModel) {
    const navigationExtras: NavigationExtras = { state: { data: employee } };
    this.router.navigate(['/receipt-payment/employee'], navigationExtras);
  }

  fnViewEmployeeLedger(employee: EmployeeModel) {
    const navigationExtras: NavigationExtras = { state: { data: employee } };
    this.router.navigate(['/reports/employee'], navigationExtras);
  }

  print(){
    if (this.employees == null  || this.employees == undefined || this.employees.length == 0) {
      this.gFuncs.openSnackBar("Sorry you can't print empty list");
      return;
    }

    let printColumns: PrintColumn[] = [
      {
        columnName: 'name',
        displayname: 'Employee Name'
      },
      {
        columnName: 'mobile',
        displayname: 'Mobile'
      },
      {
        columnName: 'alternateMobile',
        displayname: 'Alternate Mobile'
      },
      {
        columnName: 'balanceAmount',
        displayname: 'Balance Amount'
      },
      {
        columnName: 'location',
        displayname: 'Location'
      }
    ]

    let payableEmployees = this.employees.filter(x => x.balanceAmount > 0);
    let sum: number = payableEmployees.map(a => a.balanceAmount).reduce(function (a, b) {
      return a + b;
    });

    let customerTotal: EmployeeModel = {
      id: 0,
    name: '',
    mobile: 'Total',
    alternateMobile: '',
    location: '',
    identityDetails: '',
    address: '',
    isVerified: false,
    description: '',
    skillsAsPlainText: '',
    balanceAmount: sum,
    skills: [
        {
            id: 0,
            name: '',
            code: ''
        }
    ],
    LastUpdatedDate: new Date()
    }

    payableEmployees.push(customerTotal);

    let pageHeaders:string[] = [
      `Viswas Man Power Supply, Chowallor Branch - Employee Details` 
    ]
    let template = this._print.listTemplate(payableEmployees, printColumns,pageHeaders);
    this._print.print(template);
  }

  public selectedRow: number;
  public highlightRow(id: number) {
    this.selectedRow = id;
  }

}
