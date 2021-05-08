import { Component, OnInit, Inject } from '@angular/core';
import { EmployeePaymentModel, EmployeeModel, CreateEmployeePaymentModel } from 'src/app/models/man-power-models';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MessagingService } from 'src/app/services/messaging.service';
import { ManPowerService } from 'src/app/services/man-power.service';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';

@Component({
  selector: 'app-add-edit-employee-payment',
  templateUrl: './add-edit-employee-payment.component.html',
  styleUrls: ['./add-edit-employee-payment.component.scss']
})
export class AddEditEmployeePaymentComponent implements OnInit {

  employeePaymentData: EmployeePaymentModel = null;
  paymentEmployee?: EmployeeModel = null;
  defaultDate?: Date = null;

  employeePaymentForm: FormGroup;

  get formControls() { return this.employeePaymentForm.controls; }

  employees: EmployeeModel[] = [];
  filteredEmployees: EmployeeModel[] = [];
  balanceAmount?: number = null;

  paymentTypes: any[];

  constructor(public dialogRef: MatDialogRef<AddEditEmployeePaymentComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _messagingService: MessagingService,
    private formBuilder: FormBuilder,
    private _service: ManPowerService,
    private _gFuncs: GlobalFunctionsService) {

    if (data.type == 'NewPayment')
      this.paymentEmployee = data.employee
    else if (data.type == 'EmptyNewPayment')
      this.defaultDate = data.data;
    else
      this.employeePaymentData = data.data;

    if (this.employeePaymentData != null) {
      this._messagingService.changeHeaderRouteMessage("Master/Employee Payments/Edit");
    }
    else {
      this._messagingService.changeHeaderRouteMessage("Master/Employee Payments/New");
    }
  }

  ngOnInit(): void {
    this.employeePaymentForm = this.formBuilder.group({
      employeeId: ['', Validators.required],
      date: ['', Validators.required],
      amount: ['', Validators.required],
      paymentTypeId: ['', Validators.required],
      remarks: [''],
    });

    if (this.defaultDate != null)
      this.formControls.date.setValue(this.defaultDate);
    else
      this.formControls.date.setValue(new Date());

    if (this.employeePaymentData != null) {
      this.fnBindData();
      return;
    }

    if (this.paymentEmployee != null) {
      this._service.getAllEmployees().subscribe((result: any[]) => {
        this.employees = result;
        this.filteredEmployees = result;
        this.formControls.employeeId.setValue(this.paymentEmployee.id);
        this.balanceAmount = this.paymentEmployee.balanceAmount;
        this.formControls.employeeId.disable();
      })
      this.fnLoadAllPaymentTypes();
      return;
    }

    this.fnLoadAllEmployees();
    this.fnLoadAllPaymentTypes();
  }

  fnBindData() {
    this._service.getAllEmployees().subscribe((result: any[]) => {
      this.employees = result;
      this.filteredEmployees = result;
      this.formControls.employeeId.setValue(this.employeePaymentData.employeeId);
      this.balanceAmount = this.employees.find(x => x.id == this.employeePaymentData.employeeId).balanceAmount;
    })
    this._service.getAllEmployeePaymentTypes().subscribe((result: any[]) => {
      this.paymentTypes = result;
      this.formControls.paymentTypeId.setValue(this.employeePaymentData.paymentTypeId);
    })
    this.formControls.date.setValue(this.employeePaymentData.date);
    this.formControls.amount.setValue(this.employeePaymentData.amount);
    this.formControls.remarks.setValue(this.employeePaymentData.remarks);
  }



  fnLoadAllEmployees(): void {
    this._service.getAllEmployees().subscribe((result: any[]) => {
      this.employees = result;
      this.filteredEmployees = result;
    })
  }

  fnLoadAllPaymentTypes() {
    this._service.getAllEmployeePaymentTypes().subscribe((result: any[]) => {
      this.paymentTypes = result;
    })
  }

  fnChangeEmployeeDDL() {
    let employeeId = this.formControls.employeeId.value;
    if (employeeId != null && employeeId != undefined && employeeId != "") {
      let employee = this.employees.find(x => x.id == employeeId);
      this.balanceAmount = employee.balanceAmount;
    }
  }

  close() {
    this.dialogRef.close();
  }

  addOrUpdate() {
    if (this.employeePaymentForm.valid) {
      let date = new Date(this.formControls.date.value);
      let requestModel: CreateEmployeePaymentModel = {
        employeeId: this.formControls.employeeId.value,
        date: this._gFuncs.convertToYYYMMDD(date),
        amount: Number(this.formControls.amount.value),
        remarks: this.formControls.remarks.value,
        paymentTypeId: this.formControls.paymentTypeId.value
      }

      if (this.employeePaymentData == null) {
        this._service.addEmployeePayment(requestModel).subscribe((result: any) => {
          this.dialogRef.close(result);
        });
      }
      else {
        let employeePaymentId: number = this.employeePaymentData.id;
        this._service.updateEmployeePayment(employeePaymentId, requestModel).subscribe((result: any) => {
          this.dialogRef.close(result);
        });
      }
    }
    else {
      this._gFuncs.validateAllFormFields(this.employeePaymentForm);
    }
  }

}
