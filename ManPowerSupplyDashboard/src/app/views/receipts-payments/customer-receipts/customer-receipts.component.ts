import { Component, OnInit } from '@angular/core';
import { CustomerReceiptModel, CustomerModel } from 'src/app/models/man-power-models';
import { MessagingService } from 'src/app/services/messaging.service';
import { MatDialog } from '@angular/material/dialog';
import { ManPowerService } from 'src/app/services/man-power.service';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';
import { AddEditCustomerReceiptComponent } from './add-edit-customer-receipt/add-edit-customer-receipt.component';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { ConfirmDialogModel, ConfirmationDialogComponent } from 'src/app/shared/confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-customer-receipts',
  templateUrl: './customer-receipts.component.html',
  styleUrls: ['./customer-receipts.component.scss']
})
export class CustomerReceiptsComponent implements OnInit {

  customerReceiptForm: FormGroup;
  get formControls() { return this.customerReceiptForm.controls; }

  customers: CustomerModel[] = [];
  filteredCustomers: CustomerModel[] = [];

  customerReceipts: CustomerReceiptModel[];
  public searchText: string;
  filterMetadata = { count: 0, sums: [] };

  navigateCustomerId: number;
  constructor(private _messaging: MessagingService,
    public dialog: MatDialog,
    private formBuilder: FormBuilder,
    private _service: ManPowerService,
    private router: Router,
    private gFuncs: GlobalFunctionsService) {

    const navigation = this.router.getCurrentNavigation();
    const state = navigation.extras.state as { data: CustomerModel };
    if (state != null && state != undefined)
      this.navigateCustomerId = state.data.id;
  }

  ngOnInit(): void {
    this._messaging.changeHeaderRouteMessage('Receipts & Payments/Customer Receipts');

    this.customerReceiptForm = this.formBuilder.group({
      fromDate: ['', Validators.required],
      toDate: ['', Validators.required],
      customerId: ['']
    });

    let date = new Date();
    let fromDate = new Date(date.getFullYear(),date.getMonth(),1)
    this.formControls.fromDate.setValue(fromDate);
    this.formControls.toDate.setValue(date);

    this.fnLoadAllCustomers();
    if (this.navigateCustomerId != null && this.navigateCustomerId != undefined)
      return;
    this.fnLoadAllCustomerReceipts();
  }

  fnLoadAllCustomers(): void {
    this._service.getAllCustomers().subscribe((result: any[]) => {
      this.customers = result;
      this.filteredCustomers = result;
      if (this.navigateCustomerId != null && this.navigateCustomerId != undefined) {
        this.formControls.customerId.setValue(this.navigateCustomerId);
        this.fnLoadAllCustomerReceipts();
      }
    })
  }

  fnLoadAllCustomerReceipts(): void {
    if (this.customerReceiptForm.valid) {
      let FromDate: Date = new Date(this.formControls.fromDate.value);
      let ToDate: Date = new Date(this.formControls.toDate.value);
      let CustomerId: any = this.formControls.customerId.value;
      let params: string[] = [];

      params.push('FromDate=' + this.gFuncs.convertToYYYMMDD(FromDate));
      params.push('ToDate=' + this.gFuncs.convertToYYYMMDD(ToDate));
      if (CustomerId != null && CustomerId != undefined && CustomerId != "") {
        params.push('CustomerId=' + CustomerId);
      }

      this._service.getCustomerReceipts(params.join("&")).subscribe((result: any[]) => {
        this.customerReceipts = result;
      })

    }
    else {
      this.gFuncs.validateAllFormFields(this.customerReceiptForm);
    }

  }

  addOrUpdateCustomerReceipt(customerReceiptData?: CustomerReceiptModel): void {
    let type = null;
    let data = null;
    if(customerReceiptData==null || customerReceiptData == undefined){
      type = "EmptyNewPayment";
      data = new Date(this.formControls.fromDate.value);
    }
    else{
      type = "EditPayment";
      data =customerReceiptData ;
    }
    const addEditDialog = this.dialog.open(AddEditCustomerReceiptComponent, {
      width: '700px',
      panelClass: 'myapp-no-padding-dialog',
      height: '350px',
      data: { type: type, data: data }
    });

    addEditDialog.afterClosed().subscribe((result: CustomerReceiptModel) => {
      this._messaging.changeHeaderRouteMessage('Receipts & Payments/Customer Receipts');
      if (result != null) {
        let message = result.customerName + ' has been ' + (customerReceiptData == null ? 'added' : 'updated') + ' successfully'
        this.gFuncs.openSnackBar(message);
        if (customerReceiptData == null) {
          this.searchText = '';
        }
        this.fnLoadAllCustomerReceipts();
      }
    });
  }

  deleteCustomerReceipt(customerId: number) {

    const message = `Are you sure you want to delete?`;

    const dialogData = new ConfirmDialogModel("Confirmation", message);

    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      maxWidth: "400px",
      data: dialogData,
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((dialogResult: boolean) => {
      if (dialogResult) {
        this._service.deleteCustomerReceipt(customerId).subscribe((result: CustomerReceiptModel) => {
          let message = `Payment from ${result.customerName} has been deleted!`
          this.gFuncs.openSnackBar(message);
          this.fnLoadAllCustomerReceipts();
        })
      }
    });
  }

  public selectedRow: number;
  public highlightRow(id: number) {
    this.selectedRow = id;
  }

}
