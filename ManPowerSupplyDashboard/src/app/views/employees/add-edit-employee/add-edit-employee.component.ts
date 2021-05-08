import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EmployeeModel, CreateEmployeeModel, SkillModel } from 'src/app/models/man-power-models';
import { MessagingService } from 'src/app/services/messaging.service';
import { ManPowerService } from 'src/app/services/man-power.service';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';

@Component({
  selector: 'app-add-edit-employee',
  templateUrl: './add-edit-employee.component.html',
  styleUrls: ['./add-edit-employee.component.scss']
})
export class AddEditEmployeeComponent implements OnInit {

  employeeForm: FormGroup;
  skills: SkillModel[] = [];
  skillsFiltered: SkillModel[] = [];
  selectedSkills: SkillModel[] = [];

  get formControls() { return this.employeeForm.controls; }


  constructor(public dialogRef: MatDialogRef<AddEditEmployeeComponent>,
    @Inject(MAT_DIALOG_DATA) public employeeData: EmployeeModel,
    private _messagingService: MessagingService,
    private formBuilder: FormBuilder,
    private _service: ManPowerService,
    private _gFuncs: GlobalFunctionsService) {
    if (this.employeeData != null) {
      this._messagingService.changeHeaderRouteMessage("Master/Employees/Edit");
    }
    else {
      this._messagingService.changeHeaderRouteMessage("Master/Employees/New");
    }
  }

  ngOnInit(): void {
    this.employeeForm = this.formBuilder.group({
      name: ['', Validators.required],
      mobile: ['', Validators.required],
      alternateMobile: [],
      location: ['', Validators.required],
      identityDetails: ['', Validators.required],
      address: ['', Validators.required],
      skills: ['', Validators.required],
    });

    this.fnLoadSkills();

    if (this.employeeData != null)
      this.fnBindData();
  }

  fnBindData() {
    this.formControls['name'].setValue(this.employeeData.name);
    this.formControls['mobile'].setValue(this.employeeData.mobile);
    this.formControls['alternateMobile'].setValue(this.employeeData.alternateMobile);
    this.formControls['location'].setValue(this.employeeData.location);
    this.formControls['identityDetails'].setValue(this.employeeData.identityDetails);
    this._service.getAllSkills().subscribe((result: any[]) => {
      this.skills = result;
      this.skillsFiltered = result;
      let selectedSkills:any = this.employeeData.skills;
      console.log(selectedSkills);
      this.formControls['skills'].setValue(selectedSkills);
      this.selectedSkills = selectedSkills;
    })
    this.formControls['address'].setValue(this.employeeData.address);
  }

  comparer(o1: any, o2: any): boolean {
    if (o2 == undefined)
      return;
    // if possible compare by object's name, and not by reference.
    return o1 && o2 ? o1.name === o2.name : o2 === o2;
  }

  fnLoadSkills() {
    this._service.getAllSkills().subscribe((result: any[]) => {
      this.skills = result;
      this.skillsFiltered = result;
    })
  }

  close() {
    this.dialogRef.close();
  }

  addOrUpdate() {
    if (this.employeeForm.valid) {
      let requestModel: CreateEmployeeModel = {
        name: this.formControls.name.value,
        mobile: this.formControls.mobile.value,
        alternateMobile: this.formControls.alternateMobile.value,
        identityDetails: this.formControls.identityDetails.value,
        location: this.formControls.location.value,
        skills: this.formControls.skills.value,
        address: this.formControls.address.value
      }

      if (this.employeeData == null) {
        this._service.addEmployee(requestModel).subscribe((result: any) => {
          this.dialogRef.close(result);
        });
      }
      else {
        let employeeId: number = this.employeeData.id;
        this._service.updateEmployee(employeeId, requestModel).subscribe((result: any) => {
          this.dialogRef.close(result);
        });
      }
    }
    else {
      this._gFuncs.validateAllFormFields(this.employeeForm);
    }
  }

}
