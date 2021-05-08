import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MessagingService } from 'src/app/services/messaging.service';
import { ManPowerService } from 'src/app/services/man-power.service';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';
import { CreateAccountHeadModel } from 'src/app/models/man-power-models';

@Component({
  selector: 'app-add-receipt-payment-account-head',
  templateUrl: './add-receipt-payment-account-head.component.html',
  styleUrls: ['./add-receipt-payment-account-head.component.scss']
})
export class AddReceiptPaymentAccountHeadComponent implements OnInit {

  accountHeadForm: FormGroup;

  get formControls() { return this.accountHeadForm.controls; }

  constructor(public dialogRef: MatDialogRef<AddReceiptPaymentAccountHeadComponent>,
    @Inject(MAT_DIALOG_DATA) public groupHeadId: number,
    private _messagingService: MessagingService,
    private formBuilder: FormBuilder,
    private _service: ManPowerService,
    private _gFuncs: GlobalFunctionsService) {

    this._messagingService.changeHeaderRouteMessage("'Receipts & Payments/Receipts/Account Head/New");
  }

  ngOnInit(): void {
    this.accountHeadForm = this.formBuilder.group({
      name: ['', Validators.required]
    });
  }

  close() {
    this.dialogRef.close();
  }

  add() {
    if (this.accountHeadForm.valid) {
      let requestModel: CreateAccountHeadModel = {
        accountGroupId: this.groupHeadId,
        name: this.formControls.name.value,
        description: ''
      }

      this._service.addAccountHead(requestModel).subscribe((result: any) => {
        this.dialogRef.close(result);
        this._gFuncs.openSnackBar('Account head '+result.name+' has been added successfully')
      });
    }
    else {
      this._gFuncs.validateAllFormFields(this.accountHeadForm);
    }
  }

}
