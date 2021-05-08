import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { OrganizationModel } from 'src/app/authentication/models/user-model';
import { UserRequestModel } from 'src/app/models/user-request-model';
import { RolesModel } from 'src/app/models/user-response-model';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';
import { MessagingService } from 'src/app/services/messaging.service';
import { UserManagerService } from 'src/app/services/user-manager.service';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.scss']
})
export class AddUserComponent implements OnInit {


  addUserForm: FormGroup;
  organizations: OrganizationModel[] = [];
  organizationsFiltered: OrganizationModel[] = [];
  selectedOrganizations: OrganizationModel[] = [];

  roles: RolesModel[] = [];
  rolesFiltered: RolesModel[] = [];
  selectedRoles: RolesModel[] = [];

  get formControls() { return this.addUserForm.controls; }

  constructor(public dialogRef: MatDialogRef<AddUserComponent>,
    @Inject(MAT_DIALOG_DATA) public inputData: any,
    private _messagingService: MessagingService,
    private formBuilder: FormBuilder,
    private _service: UserManagerService,
    private _gFuncs: GlobalFunctionsService) {

    this._messagingService.changeHeaderRouteMessage("Add User");
  }
  ngOnInit(): void {
    this.addUserForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      mobileNumber: ['', Validators.required],
      eMail: ['', [Validators.required, Validators.email]],
      userName: ['', Validators.required],
      organizations: ['', Validators.required],
      roles: ['', Validators.required]
    })

    this.fnLoadOrganizations();
    this.fnLoadRoles();
  }

  comparer(o1: any, o2: any): boolean {
    if (o2 == undefined)
      return;
    // if possible compare by object's name, and not by reference.
    return o1 && o2 ? o1.name === o2.name : o2 === o2;
  }

  fnLoadOrganizations() {
    this._service.getAllOrganization().subscribe((result: any[]) => {
      this.organizations = result;
      this.organizationsFiltered = result;
    })
  }

  fnLoadRoles() {
    this._service.getAllRoles().subscribe((result: any[]) => {
      this.roles = result;
      this.rolesFiltered = result;
    })
  }

  addUser(){
    if (this.addUserForm.valid) {
      let password : string = `${this.formControls.userName.value}@123`
      let requestModel : UserRequestModel ={
        firstName: this.formControls.firstName.value,
        lastName: this.formControls.lastName.value,
        userName: this.formControls.userName.value,
        mobileNumber: this.formControls.mobileNumber.value,
        eMail: this.formControls.eMail.value,
        password: password,
        organizations : this.selectedOrganizations,
        roles :this.selectedRoles
      }

      this._service.addUser(requestModel).subscribe(()=>{
        this._gFuncs.openSnackBar("New user has been added");
        this.dialogRef.close();
      })
    }
    else {
      this._gFuncs.validateAllFormFields(this.addUserForm);
    }
  }
  
  close() {
    this.dialogRef.close();
  }

}
