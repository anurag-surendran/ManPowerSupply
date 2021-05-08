import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AccountHeadModel, ReceiptAndPaymentParticulars, CreateReceiptAndPaymentModel } from 'src/app/models/man-power-models';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { MessagingService } from 'src/app/services/messaging.service';
import { ManPowerService } from 'src/app/services/man-power.service';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';
import { AddReceiptPaymentAccountHeadComponent } from '../../add-receipt-payment-account-head/add-receipt-payment-account-head.component';

@Component({
  selector: 'app-add-edit-payment',
  templateUrl: './add-edit-payment.component.html',
  styleUrls: ['./add-edit-payment.component.scss']
})
export class AddEditPaymentComponent implements OnInit {


  receiptForm: FormGroup;

  receiptData: ReceiptAndPaymentParticulars = null;
  defaultDate?: Date = null;

  get formControls() { return this.receiptForm.controls; }

  accountHeads: AccountHeadModel[] = [];
  filteredAccountHeads: AccountHeadModel[] = [];

  selectedAccountHead?: number = null;


  constructor(public dialogRef: MatDialogRef<AddEditPaymentComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialog: MatDialog,
    private _messagingService: MessagingService,
    private formBuilder: FormBuilder,
    private _service: ManPowerService,
    private _gFuncs: GlobalFunctionsService) {

    if (data.type == 'EmptyNewPayment')
      this.defaultDate = data.data;
    else
      this.receiptData = data.data;

    if (this.receiptData != null) {
      this._messagingService.changeHeaderRouteMessage("Receipts & Payments/Payments/Edit");
    }
    else {
      this._messagingService.changeHeaderRouteMessage("Receipts & Payments/Payments/New");
    }
  }

  ngOnInit(): void {
    this.receiptForm = this.formBuilder.group({
      date: ['', Validators.required],
      accountHeadId: ['', Validators.required],
      amount: ['', Validators.required],
      particular: ['', Validators.required],
      description: [''],
    });

    if (this.defaultDate != null)
      this.formControls.date.setValue(this.defaultDate);
    else
      this.formControls.date.setValue(new Date());

    if (this.receiptData != null) {
      this.fnBindData();
      return;
    }

    this.fnLoadAllReceiptAccountHeads();

  }


  fnBindData() {
    let param = "AccountType=4&GroupId=2"
    this._service.getAccountHeads(param).subscribe((result: any[]) => {
      this.accountHeads = result;
      this.filteredAccountHeads = result;
      this.formControls.accountHeadId.setValue(this.receiptData.accountHeadId);
    })

    this.formControls.date.setValue(this.receiptData.date);
    this.formControls.amount.setValue(this.receiptData.amount);
    this.formControls.particular.setValue(this.receiptData.particular);
    this.formControls.description.setValue(this.receiptData.description);

  }

  fnLoadAllReceiptAccountHeads(): void {
    let param = "AccountType=4&GroupId=2&Restricted=True"
    this._service.getAccountHeads(param).subscribe((result: any[]) => {
      this.accountHeads = result;
      this.filteredAccountHeads = result;
      if (this.selectedAccountHead != null)
        this.formControls.accountHeadId.setValue(this.selectedAccountHead);
    })
  }

  addAccountHead() {

    const addEditDialog = this.dialog.open(AddReceiptPaymentAccountHeadComponent, {
      width: '300px',
      panelClass: 'myapp-no-padding-dialog',
      height: '180px',
      data: 2
    });

    addEditDialog.afterClosed().subscribe((result: AccountHeadModel) => {

      if (this.receiptData != null) {
        this._messagingService.changeHeaderRouteMessage("'Receipts & Payments/Payments/Edit");
      }
      else {
        this._messagingService.changeHeaderRouteMessage("'Receipts & Payments/Payments/New");
      }

      if (result != null) {
        this.selectedAccountHead = result.id;
        this.fnLoadAllReceiptAccountHeads();
      }
    });
  }


  close() {
    this.dialogRef.close();
  }

  addOrUpdate() {
    if (this.receiptForm.valid) {
      let date = new Date(this.formControls.date.value);
      let requestModel: CreateReceiptAndPaymentModel = {
        date: this._gFuncs.convertToYYYMMDDHHMMSS(date),
        accountHeadId: this.formControls.accountHeadId.value,
        amount: Number(this.formControls.amount.value),
        particular: this.formControls.particular.value,
        description: this.formControls.description.value,
        transactionType: 2
      }

      if (this.receiptData == null) {
        this._service.addReceiptAndPayment(requestModel).subscribe((result: any) => {
          this.dialogRef.close(result);
        });
      }
      else {
        let receiptId: number = this.receiptData.id;
        this._service.updateReceiptAndPayment(receiptId, requestModel).subscribe((result: any) => {
          this.dialogRef.close(result);
        });
      }
    }
    else {
      this._gFuncs.validateAllFormFields(this.receiptForm);
    }
  }

}
