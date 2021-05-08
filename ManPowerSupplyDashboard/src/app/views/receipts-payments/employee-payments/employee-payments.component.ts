import { Component, OnInit } from '@angular/core';
import { EmployeeModel, EmployeePaymentModel } from 'src/app/models/man-power-models';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MessagingService } from 'src/app/services/messaging.service';
import { MatDialog } from '@angular/material/dialog';
import { ManPowerService } from 'src/app/services/man-power.service';
import { Router } from '@angular/router';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';
import { AddEditEmployeePaymentComponent } from './add-edit-employee-payment/add-edit-employee-payment.component';
import { ConfirmDialogModel, ConfirmationDialogComponent } from 'src/app/shared/confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-employee-payments',
  templateUrl: './employee-payments.component.html',
  styleUrls: ['./employee-payments.component.scss']
})
export class EmployeePaymentsComponent implements OnInit {

  employeePaymentForm: FormGroup;
  get formControls() { return this.employeePaymentForm.controls; }

  employees: EmployeeModel[] = [];
  filteredEmployees: EmployeeModel[] = [];

  employeePayments: EmployeePaymentModel[];
  public searchText: string;
  filterMetadata = { count: 0, sums: [] };

  navigateEmployeeId: number;
  constructor(private _messaging: MessagingService,
    public dialog: MatDialog,
    private formBuilder: FormBuilder,
    private _service: ManPowerService,
    private router: Router,
    private gFuncs: GlobalFunctionsService) {

    const navigation = this.router.getCurrentNavigation();
    const state = navigation.extras.state as { data: EmployeeModel };
    if (state != null && state != undefined)
      this.navigateEmployeeId = state.data.id;
  }

  ngOnInit(): void {
    this._messaging.changeHeaderRouteMessage('Receipts & Payments/Employee Payments');

    this.employeePaymentForm = this.formBuilder.group({
      fromDate: ['', Validators.required],
      toDate: ['', Validators.required],
      employeeId: ['']
    });

    let date = new Date();
    let fromDate = new Date(date.getFullYear(),date.getMonth(),1)
    this.formControls.fromDate.setValue(fromDate);
    this.formControls.toDate.setValue(date);

    this.fnLoadAllEmployees();
    if (this.navigateEmployeeId != null && this.navigateEmployeeId != undefined)
      return;
    this.fnLoadAllEmployeePayments();
  }

  fnLoadAllEmployees(): void {
    this._service.getAllEmployees().subscribe((result: any[]) => {
      this.employees = result;
      this.filteredEmployees = result;
      if (this.navigateEmployeeId != null && this.navigateEmployeeId != undefined) {
        this.formControls.employeeId.setValue(this.navigateEmployeeId);
        this.fnLoadAllEmployeePayments();
      }
    })
  }

  fnLoadAllEmployeePayments(): void {
    if (this.employeePaymentForm.valid) {
      let FromDate: Date = new Date(this.formControls.fromDate.value);
      let ToDate: Date = new Date(this.formControls.toDate.value);
      let EmployeeId: any = this.formControls.employeeId.value;
      let params: string[] = [];

      params.push('FromDate=' + this.gFuncs.convertToYYYMMDD(FromDate));
      params.push('ToDate=' + this.gFuncs.convertToYYYMMDD(ToDate));
      if (EmployeeId != null && EmployeeId != undefined && EmployeeId != "") {
        params.push('EmployeeId=' + EmployeeId);
      }

      this._service.getEmployeePayments(params.join("&")).subscribe((result: any[]) => {
        this.employeePayments = result;
      })

    }
    else {
      this.gFuncs.validateAllFormFields(this.employeePaymentForm);
    }

  }

  addOrUpdateEmployeePayment(employeePaymentData?: EmployeePaymentModel): void {
    let type = null;
    let data = null;
    if(employeePaymentData==null || employeePaymentData == undefined){
      type = "EmptyNewPayment";
      data = new Date(this.formControls.fromDate.value);
    }
    else{
      type = "EditPayment";
      data =employeePaymentData ;
    }
    const addEditDialog = this.dialog.open(AddEditEmployeePaymentComponent, {
      width: '700px',
      panelClass: 'myapp-no-padding-dialog',
      height: '350px',
      data: { type: type, data: data }
    });

    addEditDialog.afterClosed().subscribe((result: EmployeePaymentModel) => {
      this._messaging.changeHeaderRouteMessage('Receipts & Payments/Employee Payments');
      if (result != null) {
        let message = result.employeeName + ' has been ' + (employeePaymentData == null ? 'added' : 'updated') + ' successfully'
        this.gFuncs.openSnackBar(message);
        if (employeePaymentData == null) {
          this.searchText = '';
        }
        this.fnLoadAllEmployeePayments();
      }
    });
  }

  deleteEmployeePayment(employeeId: number) {

    const message = `Are you sure you want to delete?`;

    const dialogData = new ConfirmDialogModel("Confirmation", message);

    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      maxWidth: "400px",
      data: dialogData,
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((dialogResult: boolean) => {
      if (dialogResult) {
        this._service.deleteEmployeePayment(employeeId).subscribe((result: EmployeePaymentModel) => {
          let message = `Payment from ${result.employeeName} has been deleted!`
          this.gFuncs.openSnackBar(message);
          this.fnLoadAllEmployeePayments();
        })
      }
    });
  }

  public selectedRow: number;
  public highlightRow(id: number) {
    this.selectedRow = id;
  }

}
