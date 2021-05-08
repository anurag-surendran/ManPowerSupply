import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { OrganizationModel, User } from 'src/app/authentication/models/user-model';
import { UserRequestModel } from 'src/app/models/user-request-model';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';
import { MessagingService } from 'src/app/services/messaging.service';
import { UserManagerService } from 'src/app/services/user-manager.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {

  userModel : User;

  profileForm: FormGroup;
  get formControls() { return this.profileForm.controls; }

  constructor(public dialogRef: MatDialogRef<UserProfileComponent>,
    @Inject(MAT_DIALOG_DATA) public inputData: User,
    private _messagingService: MessagingService,
    private formBuilder: FormBuilder,
    private _service: UserManagerService,
    private _gFuncs: GlobalFunctionsService) {

    this.userModel = inputData;
    this._messagingService.changeHeaderRouteMessage("Profile");
  }
  ngOnInit(): void {
    this.profileForm = this.formBuilder.group({
      firstName : ['', Validators.required],
      lastName : ['', Validators.required],
      mobileNumber : ['', Validators.required],
      eMail : ['', [Validators.required,Validators.email]],
      userName : ['', Validators.required],
    })

    this.fnLoadData();
  }

  fnLoadData(){
    this.formControls.firstName.setValue(this.userModel.firstName);
    this.formControls.lastName.setValue(this.userModel.lastName);
    this.formControls.mobileNumber.setValue(this.userModel.mobileNumber);
    this.formControls.eMail.setValue(this.userModel.eMail);
    this.formControls.userName.setValue(this.userModel.userName);
    this.formControls.userName.disable();
  }

  fnUpdateProfile(){
    if (this.profileForm.valid) {

      let requestModel : UserRequestModel ={
        firstName: this.formControls.firstName.value,
        lastName: this.formControls.lastName.value,
        userName: this.formControls.userName.value,
        mobileNumber: this.formControls.mobileNumber.value,
        eMail: this.formControls.eMail.value,
        password: null,
        organizations : [],
        roles : []
      }

      this._service.updateUser(requestModel,this.userModel.id).subscribe(()=>{
        this._gFuncs.openSnackBar("Profile has been updated");
        this.dialogRef.close();
      })
    }
    else {
      this._gFuncs.validateAllFormFields(this.profileForm);
    }
  }


  close() {
    this.dialogRef.close();
  }

}
