import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { User } from 'src/app/authentication/models/user-model';
import { AuthenticationService } from 'src/app/authentication/services/authentication.service';
import { UpdatePasswordRequestModel } from 'src/app/models/update-password-request-model';
import { ValidatePasswordRequestModel } from 'src/app/models/validate-password-request-model';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';
import { MessagingService } from 'src/app/services/messaging.service';
import { UserManagerService } from 'src/app/services/user-manager.service';

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const invalidCtrl = !!(control && control.invalid && control.parent.dirty);
    const invalidParent = !!(control && control.parent && control.parent.invalid && control.parent.dirty);

    return (invalidCtrl || invalidParent);
  }
}

@Component({
  selector: 'app-update-password',
  templateUrl: './update-password.component.html',
  styleUrls: ['./update-password.component.scss']
})
export class UpdatePasswordComponent implements OnInit {

  updatePasswordForm: FormGroup;
  matcher = new MyErrorStateMatcher();

  user : User;
  hideOld = true;
  hideNew = true;
  hideConfirm = true;

  get formControls() { return this.updatePasswordForm.controls; }

  constructor(public dialogRef: MatDialogRef<UpdatePasswordComponent>,
    @Inject(MAT_DIALOG_DATA) public inputData: User,
    private _messagingService: MessagingService,
    private formBuilder: FormBuilder,
    private _service: UserManagerService,
    private _gFuncs: GlobalFunctionsService,
    private _authService:AuthenticationService) { 
      this.user=inputData;
      this._messagingService.changeHeaderRouteMessage("Update Password");
    }

  ngOnInit(): void {
    this.updatePasswordForm = this.formBuilder.group({
      oldPassword: ['', Validators.required],
      newPassword: ['', Validators.required],
      confirmPassword: ['']
    }, { validator: this.checkPasswords })
  }

  checkPasswords(group: FormGroup) { // here we have the 'passwords' group
    let pass = group.controls.newPassword.value;
    let confirmPass = group.controls.confirmPassword.value;

    return pass === confirmPass ? null : { notSame: true }
  }

  validateExistingPassword(){
    let password = this.formControls.oldPassword.value;
    if(password!=null && password!=undefined && password!='')
    {
      let requestModel : ValidatePasswordRequestModel={
        userId : this.user.id,
        password : password
      }

      this._service.validatePassword(requestModel).subscribe({
       error: error => {
        this._gFuncs.openSnackBar("Invalid Password")
        this.formControls.oldPassword.setValue('');
        (<any>this.updatePasswordForm.get('oldPassword')).nativeElement.focus();
        }
      })
    }
  }

  updatePassword(){
    if (this.updatePasswordForm.valid) {

      let requestModel : UpdatePasswordRequestModel ={
        userId : this.user.id,
        oldPassword : this.formControls.oldPassword.value,
        newPassword : this.formControls.newPassword.value
      }

      this._service.updatePassword(requestModel).subscribe(()=>{
        this._gFuncs.openSnackBar("Password has been updated, Redirecting to login page.");
        this._authService.logout();
        this.dialogRef.close();
      })
    }
    else {
      this._gFuncs.validateAllFormFields(this.updatePasswordForm);
    }
  }
  
  close() {
    this.dialogRef.close();
  }

}
