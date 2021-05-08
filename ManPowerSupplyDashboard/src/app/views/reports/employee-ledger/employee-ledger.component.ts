import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { EmployeeModel, EmployeeLedgerModel } from 'src/app/models/man-power-models';
import { MessagingService } from 'src/app/services/messaging.service';
import { MatDialog } from '@angular/material/dialog';
import { ManPowerService } from 'src/app/services/man-power.service';
import { Router } from '@angular/router';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';
import { PrintColumn, PrintService } from 'src/app/services/print.service';

@Component({
  selector: 'app-employee-ledger',
  templateUrl: './employee-ledger.component.html',
  styleUrls: ['./employee-ledger.component.scss']
})
export class EmployeeLedgerComponent implements OnInit {

  employeeLedgerForm: FormGroup;
  get formControls() { return this.employeeLedgerForm.controls; }

  employees: EmployeeModel[] = [];
  filteredEmployees: EmployeeModel[] = [];

  employeeLedgers: EmployeeLedgerModel = new EmployeeLedgerModel();
  public searchText: string;
  filterMetadata = { count: 0, sums: [] };

  navigateEmployeeId: number;
  constructor(private _messaging: MessagingService,
    public dialog: MatDialog,
    private formBuilder: FormBuilder,
    private _service: ManPowerService,
    private router: Router,
    private gFuncs: GlobalFunctionsService,
    private _print: PrintService) {

    const navigation = this.router.getCurrentNavigation();
    const state = navigation.extras.state as { data: EmployeeModel };
    if (state != null && state != undefined)
      this.navigateEmployeeId = state.data.id;
  }

  ngOnInit(): void {
    this._messaging.changeHeaderRouteMessage('Reports/Employee Ledger');

    this.employeeLedgerForm = this.formBuilder.group({
      fromDate: ['', Validators.required],
      toDate: ['', Validators.required],
      employeeId: ['', Validators.required]
    });

    let date = new Date();
    let fromDate = new Date(date.getFullYear(),date.getMonth(),1)
    this.formControls.fromDate.setValue(fromDate);
    this.formControls.toDate.setValue(date);

    this.fnLoadAllEmployees();
    if (this.navigateEmployeeId != null && this.navigateEmployeeId != undefined)
      return;
  }

  fnLoadAllEmployees(): void {
    this._service.getAllEmployees().subscribe((result: any[]) => {
      this.employees = result;
      this.filteredEmployees = result;
      this.gFuncs.validateAllFormFields(this.employeeLedgerForm);
      if (this.navigateEmployeeId != null && this.navigateEmployeeId != undefined) {
        this.formControls.employeeId.setValue(this.navigateEmployeeId);
        this.fnLoadAllEmployeeLedgers();
      }
    })
  }

  fnLoadAllEmployeeLedgers(): void {
    if (this.employeeLedgerForm.valid) {
      let FromDate: Date = new Date(this.formControls.fromDate.value);
      let ToDate: Date = new Date(this.formControls.toDate.value);
      let EmployeeId: any = this.formControls.employeeId.value;
      let params: string[] = [];

      params.push('FromDate=' + this.gFuncs.convertToYYYMMDD(FromDate));
      params.push('ToDate=' + this.gFuncs.convertToYYYMMDD(ToDate));
      if (EmployeeId != null && EmployeeId != undefined && EmployeeId != "") {
        params.push('EmployeeId=' + EmployeeId);
      }

      this._service.getEmployeeLedger(params.join("&")).subscribe((result: any) => {
        this.employeeLedgers = result;
      })

    }
    else {
      this.gFuncs.validateAllFormFields(this.employeeLedgerForm);
    }
  }

  print(){
    if (this.employeeLedgers == null  || this.employeeLedgers == undefined ) {
      this.gFuncs.openSnackBar("Sorry you can't print empty list");
      return;
    }

    let printColumns: PrintColumn[] = [
      {
        columnName: 'date',
        displayname: 'Date',
        isDate: true
      },
      {
        columnName: 'particular',
        displayname: 'Particulars'
      },
      {
        columnName: 'employeePay',
        displayname: 'Employee Pay'
      },
      {
        columnName: 'payment',
        displayname: 'payment'
      },
      {
        columnName: 'balance',
        displayname: 'Balance'
      }
    ]

    let FromDate: Date = new Date(this.formControls.fromDate.value);
      let ToDate: Date = new Date(this.formControls.toDate.value);
      let selectedEmployee = this.employees.find(x=>x.id == this.formControls.employeeId.value).name;

    let pageHeaders:string[] = [
      `Viswas Man Power Supply, Chowallor Branch - Employee Personal Ledger From ${this.gFuncs.convertToDDMMYYYY(FromDate)} to ${this.gFuncs.convertToDDMMYYYY(ToDate)}`,
      `Employee Name : ${selectedEmployee}` 
    ]
    let template = this._print.employeeTemplate(this.employeeLedgers, printColumns,pageHeaders);
    this._print.print(template);
  }

  public selectedRow: number;
  public highlightRow(id: number) {
    this.selectedRow = id;
  }

}
