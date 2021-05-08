import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CreateCustomerReceiptModel, CustomerReceiptModel, CustomerModel } from 'src/app/models/man-power-models';
import { MessagingService } from 'src/app/services/messaging.service';
import { ManPowerService } from 'src/app/services/man-power.service';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

@Component({
  selector: 'app-add-edit-customer-receipt',
  templateUrl: './add-edit-customer-receipt.component.html',
  styleUrls: ['./add-edit-customer-receipt.component.scss']
})

export class AddEditCustomerReceiptComponent implements OnInit {

  customerReceiptData: CustomerReceiptModel = null;
  receiptCustomer?: CustomerModel = null;
  defaultDate?: Date = null;

  customerReceiptForm: FormGroup;

  get formControls() { return this.customerReceiptForm.controls; }

  customers: CustomerModel[] = [];
  filteredCustomers: CustomerModel[] = [];
  balanceAmount?: number = null;

  cashCollectedPersons: string[] = [];
  filteredCashCollectedPersons: Observable<string[]>;


  constructor(public dialogRef: MatDialogRef<AddEditCustomerReceiptComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _messagingService: MessagingService,
    private formBuilder: FormBuilder,
    private _service: ManPowerService,
    private _gFuncs: GlobalFunctionsService) {

    if (data.type == 'NewPayment')
      this.receiptCustomer = data.customer
    else if (data.type == 'EmptyNewPayment')
      this.defaultDate = data.data;
    else
      this.customerReceiptData = data.data;

    if (this.customerReceiptData != null) {
      this._messagingService.changeHeaderRouteMessage("Master/Customer Receipts/Edit");
    }
    else {
      this._messagingService.changeHeaderRouteMessage("Master/Customer Receipts/New");
    }
  }

  ngOnInit(): void {
    this.customerReceiptForm = this.formBuilder.group({
      customerId: ['', Validators.required],
      date: ['', Validators.required],
      paidAmount: ['', Validators.required],
      cashCollectedBy: ['', Validators.required],
      remarks: [''],
      myControl: []
    });

    if (this.defaultDate != null)
      this.formControls.date.setValue(this.defaultDate);
    else
      this.formControls.date.setValue(new Date());
      
    this.fnLoadCashCollectedPersons();

    if (this.customerReceiptData != null) {
      this.fnBindData();
      return;
    }

    if (this.receiptCustomer != null) {
      this._service.getAllCustomers().subscribe((result: any[]) => {
        this.customers = result;
        this.filteredCustomers = result;
        this.formControls.customerId.setValue(this.receiptCustomer.id);
        this.balanceAmount = this.receiptCustomer.balanceAmount;
        this.formControls.customerId.disable();
      })
      return;
    }



    this.fnLoadAllCustomers();
  }

  fnLoadCashCollectedPersons() {
    this._service.getCustomerReceiptCollectionPersons().subscribe((result: any[]) => {
      this.cashCollectedPersons = result;
      this.filteredCashCollectedPersons = this.formControls.cashCollectedBy.valueChanges
        .pipe(
          startWith(''),
          map(value => this._filter(value))
        );
    })
  }


  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.cashCollectedPersons.filter(option => option.toLowerCase().includes(filterValue));
  }

  fnBindData() {
    this._service.getAllCustomers().subscribe((result: any[]) => {
      this.customers = result;
      this.filteredCustomers = result;
      this.formControls.customerId.setValue(this.customerReceiptData.customerId);
      this.balanceAmount = this.customers.find(x => x.id == this.customerReceiptData.customerId).balanceAmount;
    })
    this.formControls.date.setValue(this.customerReceiptData.date);
    this.formControls.paidAmount.setValue(this.customerReceiptData.paidAmount);
    this.formControls.cashCollectedBy.setValue(this.customerReceiptData.cashCollectedBy);
    this.formControls.remarks.setValue(this.customerReceiptData.remarks);

  }



  fnLoadAllCustomers(): void {
    this._service.getAllCustomers().subscribe((result: any[]) => {
      this.customers = result;
      this.filteredCustomers = result;
    })
  }

  fnChangeCustomerDDL() {
    let customerId = this.formControls.customerId.value;
    if (customerId != null && customerId != undefined && customerId != "") {
      let customer = this.customers.find(x => x.id == customerId);
      this.balanceAmount = customer.balanceAmount;
    }
  }

  close() {
    this.dialogRef.close();
  }

  addOrUpdate() {
    if (this.customerReceiptForm.valid) {
      let date = new Date(this.formControls.date.value);
      let requestModel: CreateCustomerReceiptModel = {
        customerId: this.formControls.customerId.value,
        date: this._gFuncs.convertToYYYMMDD(date),
        paidAmount: Number(this.formControls.paidAmount.value),
        cashCollectedBy: this.formControls.cashCollectedBy.value,
        remarks: this.formControls.remarks.value
      }

      if (this.customerReceiptData == null) {
        this._service.addCustomerReceipt(requestModel).subscribe((result: any) => {
          this.dialogRef.close(result);
        });
      }
      else {
        let customerReceiptId: number = this.customerReceiptData.id;
        this._service.updateCustomerReceipt(customerReceiptId, requestModel).subscribe((result: any) => {
          this.dialogRef.close(result);
        });
      }
    }
    else {
      this._gFuncs.validateAllFormFields(this.customerReceiptForm);
    }
  }

}
