import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AccountHeadModel, ReceiptAndPaymentModel, ReceiptAndPaymentParticulars } from 'src/app/models/man-power-models';
import { MessagingService } from 'src/app/services/messaging.service';
import { MatDialog } from '@angular/material/dialog';
import { ManPowerService } from 'src/app/services/man-power.service';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';
import { AddEditPaymentComponent } from './add-edit-payment/add-edit-payment.component';
import { ConfirmDialogModel, ConfirmationDialogComponent } from 'src/app/shared/confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-payments',
  templateUrl: './payments.component.html',
  styleUrls: ['./payments.component.scss']
})
export class PaymentsComponent implements OnInit {

  paymentForm: FormGroup;
  get formControls() { return this.paymentForm.controls; }

  accountHeads: AccountHeadModel[] = [];
  filteredAccountHeads: AccountHeadModel[] = [];

  receipts: ReceiptAndPaymentModel = new ReceiptAndPaymentModel();
  public searchText: string;
  filterMetadata = { count: 0, sums: [] };
  
  constructor(private _messaging: MessagingService,
    public dialog: MatDialog,
    private formBuilder: FormBuilder,
    private _service: ManPowerService,
    private gFuncs: GlobalFunctionsService) {
  }

  ngOnInit(): void {
    this._messaging.changeHeaderRouteMessage('Receipts & Payments /Payments');

    this.paymentForm = this.formBuilder.group({
      fromDate: ['', Validators.required],
      toDate: ['', Validators.required],
      accountHeadId: ['']
    });

    let date = new Date();
    let fromDate = new Date(date.getFullYear(),date.getMonth(),1);
    this.formControls.fromDate.setValue(fromDate);
    this.formControls.toDate.setValue(date);

    this.fnLoadAllPaymentAccountHeads();    
    this.fnLoadAllPayments();
  }

  fnLoadAllPaymentAccountHeads(): void {
    let param = "AccountType=4&GroupId=2&Restricted=True"
    this._service.getAccountHeads(param).subscribe((result: any[]) => {
      this.accountHeads = result;
      this.filteredAccountHeads = result;
    })
  }

  fnLoadAllPayments(): void {
    if (this.paymentForm.valid) {
      let FromDate: Date = new Date(this.formControls.fromDate.value);
      let ToDate: Date = new Date(this.formControls.toDate.value);
      let AccountHeadId: any = this.formControls.accountHeadId.value;
      let params: string[] = [];

      params.push('FromDate=' + this.gFuncs.convertToYYYMMDD(FromDate));
      params.push('ToDate=' + this.gFuncs.convertToYYYMMDD(ToDate));
      params.push('TransactionType=2');
      if (AccountHeadId != null && AccountHeadId != undefined && AccountHeadId != "") {
        params.push('AccountHeadId=' + AccountHeadId);
      }

      this._service.getReceiptsAndPayments(params.join("&")).subscribe((result: any) => {
        this.receipts = result;
      })

    }
    else {
      this.gFuncs.validateAllFormFields(this.paymentForm);
    }

  }

  addOrUpdateReceipt(receiptData?: ReceiptAndPaymentModel): void {
    let type = null;
    let data = null;
    if(receiptData==null || receiptData == undefined){
      type = "EmptyNewPayment";
      data = new Date(this.formControls.fromDate.value);
    }
    else{
      type = "EditPayment";
      data =receiptData ;
    }
    
    const addEditDialog = this.dialog.open(AddEditPaymentComponent, {
      width: '700px',
      panelClass: 'myapp-no-padding-dialog',
      height: '350px',
      data: { type: type, data: data }
    });

    addEditDialog.afterClosed().subscribe((result: ReceiptAndPaymentParticulars) => {
      this._messaging.changeHeaderRouteMessage('Receipts & Payments/Payments');
      if (result != null) {
        let message = result.accountHeadName + ' has been ' + (receiptData == null ? 'added' : 'updated') + ' successfully'
        this.gFuncs.openSnackBar(message);
        if (receiptData == null) {
          this.searchText = '';
        }
        this.fnLoadAllPayments();
      }
    });
  }

  deleteReceipt(transactionId: number) {

    const message = `Are you sure you want to delete?`;

    const dialogData = new ConfirmDialogModel("Confirmation", message);

    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      maxWidth: "400px",
      data: dialogData,
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((dialogResult: boolean) => {
      if (dialogResult) {
        this._service.deleteReceiptAndPayment(transactionId).subscribe((result: ReceiptAndPaymentParticulars) => {
          let message = `Payment from ${result.accountHeadName} has been deleted!`
          this.gFuncs.openSnackBar(message);
          this.fnLoadAllPayments();
        })
      }
    });
  }

  public selectedRow: number;
  public highlightRow(id: number) {
    this.selectedRow = id;
  }

}
