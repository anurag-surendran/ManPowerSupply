import { Component, OnInit, Inject, HostListener } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MessagingService } from 'src/app/services/messaging.service';
import { ManPowerService } from 'src/app/services/man-power.service';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';
import { AttendanceModel, EmployeeModel, CustomerModel, CreateAttendanceModel } from 'src/app/models/man-power-models';

@Component({
  selector: 'app-add-edit-attendance',
  templateUrl: './add-edit-attendance.component.html',
  styleUrls: ['./add-edit-attendance.component.scss']
})
export class AddEditAttendanceComponent implements OnInit {

  attendanceForm: FormGroup|any;

  attendanceData: AttendanceModel;

  employees: EmployeeModel[] = [];
  filteredEmployees: EmployeeModel[] = [];

  customers: CustomerModel[] = [];
  filteredCustomers: CustomerModel[] = [];

  nxnDayCustomers: CustomerModel[] = [];
  filteredNxnDayCustomers: CustomerModel[] = [];

  get formControls() { return this.attendanceForm.controls; }


  constructor(public dialogRef: MatDialogRef<AddEditAttendanceComponent>,
    @Inject(MAT_DIALOG_DATA) public inputData: any[],
    private _messagingService: MessagingService,
    private formBuilder: FormBuilder,
    private _service: ManPowerService,
    private _gFuncs: GlobalFunctionsService) {

    this.attendanceData = inputData[0];
    if (this.attendanceData != null) {
      this._messagingService.changeHeaderRouteMessage("Master/Attendances/Edit");
    }
    else {
      this._messagingService.changeHeaderRouteMessage("Master/Attendances/New");
    }
  }

  ngOnInit(): void {
    this.attendanceForm = this.formBuilder.group({
      date: [{ value: '', disabled: true }, Validators.required],
      employeeId: ['', Validators.required],
      customerId: ['', Validators.required],
      attendanceStatus: ['', Validators.required],
      nextDayCustomerId: ['', Validators.required],
      customerPay: ['', Validators.required],
      rent: [''],
      customerTA: [''],
      totalCustomerPay: [{ value: '', disabled: true }],
      companyTA: [''],
      employeePay: ['', Validators.required],
      remarks: [''],
    });

    this.formControls.totalCustomerPay.disabled;

    if (this.attendanceData != null) {
      this.fnBindData();
      return
    }
    this.fnLoadAllEmployees();
    this.fnLoadAllCustomers();
    this.fnSetTotalCustomerPayment();
    this.formControls.date.setValue(this.inputData[1]);
  }

  fnBindData() {

    this.formControls.date.setValue(new Date(this.attendanceData.date));

    this.formControls.date.disabled;

    this._service.getAllCustomers().subscribe((result: any[]|any) => {
      this.customers = result;
      this.filteredCustomers = result;

      this.nxnDayCustomers = result;
      this.filteredNxnDayCustomers = result;

      this.formControls.customerId.setValue(this.attendanceData.customerId);
      this.formControls.nextDayCustomerId.setValue(this.attendanceData.nextDayCustomerId);
    });

    this.formControls.attendanceStatus.setValue(this.attendanceData.attendanceStatus);

    this._service.getAllEmployees().subscribe((result: any[]|any) => {
      this.employees = result;
      this.filteredEmployees = result;

      this.formControls.employeeId.setValue(this.attendanceData.employeeId);
    })



    this.formControls.customerPay.setValue(this.attendanceData.customerPay);
    this.formControls.customerTA.setValue(this.attendanceData.customerTA);
    this.formControls.rent.setValue(this.attendanceData.rent);

    this.formControls.companyTA.setValue(this.attendanceData.companyTA);
    this.formControls.employeePay.setValue(this.attendanceData.employeePay);

    this.formControls.remarks.setValue(this.attendanceData.remarks);

    this.fnSetTotalCustomerPayment();
  }

  close() {
    this.dialogRef.close();
  }

  fnLoadAllEmployees(): void {
    this._service.getAllEmployees().subscribe((result: any[]|any) => {
      this.employees = result;
      this.filteredEmployees = result;
    })
  }

  fnLoadAllCustomers(): void {
    this._service.getAllCustomers().subscribe((result: any[]|any) => {
      this.customers = result;
      this.filteredCustomers = result;

      this.nxnDayCustomers = result;
      this.filteredNxnDayCustomers = result;
    })
  }

  fnSetDefaultNextDayCustomer() {
    let customerId = this.formControls.customerId.value;
    this.formControls['nextDayCustomerId'].setValue(customerId);
  }

  fnSetTotalCustomerPayment() {
    let customerPay = this.formControls.customerPay.value;
    let rent = this.formControls.rent.value;
    let customerTA = this.formControls.customerTA.value;

    if (customerPay == '' || customerPay == null || customerPay == undefined) {
      this.formControls.customerPay.setValue(customerPay);
    }

    if (rent == '' || rent == null || rent == undefined) {
      rent = 0;
    }

    if (customerTA == '' || customerTA == null || customerTA == undefined) {
      this.formControls.customerTA.setValue(customerTA);
    }

    let totalCustomerPay: number = Number(customerPay) + Number(rent) + Number(customerTA);

    this.formControls.totalCustomerPay.setValue(totalCustomerPay);
  }

  addOrUpdate() {
    if (this.attendanceForm.valid) {

      let requestModel: CreateAttendanceModel = {
        date: this._gFuncs.convertToYYYMMDD(new Date(this.formControls.date.value)),

        employeeId: this.formControls.employeeId.value,
        customerId: this.formControls.customerId.value,
        attendanceStatus: this.formControls.attendanceStatus.value,
        nextDayCustomerId: this.formControls.nextDayCustomerId.value,

        customerPay: Number(this.formControls.customerPay.value),
        customerTA: Number(this.formControls.customerTA.value),
        rent: Number(this.formControls.rent.value),

        companyTA: Number(this.formControls.companyTA.value),
        employeePay: Number(this.formControls.employeePay.value),

        remarks: this.formControls.remarks.value
      };

      if (this.attendanceData == null) {
        this._service.addAttendance(requestModel).subscribe((result: any) => {
          this.dialogRef.close(result);
        });
      }
      else {
        let attendanceId: number = this.attendanceData.id;
        this._service.updateAttendance(attendanceId, requestModel).subscribe((result: any) => {
          this.dialogRef.close(result);
        });
      }
    }
    else {
      this._gFuncs.validateAllFormFields(this.attendanceForm);
    }
  }

  @HostListener('window:keyup.esc') onKeyUp() {
    this.dialogRef.close();
  }
}
