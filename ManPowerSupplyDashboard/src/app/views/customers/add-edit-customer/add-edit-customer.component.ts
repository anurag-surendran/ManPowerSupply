import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomerModel, CreateCustomerModel } from 'src/app/models/man-power-models';
import { MessagingService } from 'src/app/services/messaging.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ManPowerService } from 'src/app/services/man-power.service';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';

@Component({
  selector: 'app-add-edit-customer',
  templateUrl: './add-edit-customer.component.html',
  styleUrls: ['./add-edit-customer.component.scss']
})
export class AddEditCustomerComponent implements OnInit {

  customerForm: FormGroup | any;

  get formControls() { return this.customerForm.controls; }


  constructor(public dialogRef: MatDialogRef<AddEditCustomerComponent>,
    @Inject(MAT_DIALOG_DATA) public customerData: CustomerModel,
    private _messagingService: MessagingService,
    private formBuilder: FormBuilder,
    private _service: ManPowerService,
    private _gFuncs: GlobalFunctionsService) {
    if (this.customerData != null) {
      this._messagingService.changeHeaderRouteMessage("Master/Customers/Edit");
    }
    else {
      this._messagingService.changeHeaderRouteMessage("Master/Customers/New");
    }
  }

  ngOnInit(): void {
    this.customerForm = this.formBuilder.group({
      name: ['', Validators.required],
      mobile: ['', Validators.required],
      alternateNumber: [],
      address: ['', Validators.required],
    });

    if (this.customerData != null)
      this.fnBindData();
  }

  fnBindData() {
    this.formControls['name'].setValue(this.customerData.name);
    this.formControls['mobile'].setValue(this.customerData.mobile);
    this.formControls['alternateNumber'].setValue(this.customerData.alternateMobile);
    this.formControls['address'].setValue(this.customerData.address);
  }

  close() {
    this.dialogRef.close();
  }

  addOrUpdate() {
    if (this.customerForm.valid) {

      let requestModel: CreateCustomerModel = {
        name: this.formControls.name.value,
        mobile: this.formControls.mobile.value,
        alternateMobile: this.formControls.alternateNumber.value,
        address: this.formControls.address.value
      }

      if (this.customerData == null) {
        this._service.addCustomer(requestModel).subscribe((result: any) => {
          this.dialogRef.close(result);
        });
      }
      else {
        let customerId: number = this.customerData.id;
        this._service.updateCustomer(customerId, requestModel).subscribe((result: any) => {
          this.dialogRef.close(result);
        });
      }
    }
    else {
      this._gFuncs.validateAllFormFields(this.customerForm);
    }
  }

}


