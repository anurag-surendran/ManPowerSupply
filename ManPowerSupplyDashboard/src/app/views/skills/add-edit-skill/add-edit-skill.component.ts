import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SkillModel, CreateSkillModel } from 'src/app/models/man-power-models';
import { MessagingService } from 'src/app/services/messaging.service';
import { ManPowerService } from 'src/app/services/man-power.service';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';

@Component({
  selector: 'app-add-edit-skill',
  templateUrl: './add-edit-skill.component.html',
  styleUrls: ['./add-edit-skill.component.scss']
})
export class AddEditSkillComponent implements OnInit {

  skillForm: FormGroup;

  get formControls() { return this.skillForm.controls; }


  constructor(public dialogRef: MatDialogRef<AddEditSkillComponent>,
    @Inject(MAT_DIALOG_DATA) public skillData: SkillModel,
    private _messagingService: MessagingService,
    private formBuilder: FormBuilder,
    private _service: ManPowerService,
    private _gFuncs: GlobalFunctionsService) {
    if (this.skillData != null) {
      this._messagingService.changeHeaderRouteMessage("Master/Skills/Edit");
    }
    else {
      this._messagingService.changeHeaderRouteMessage("Master/Skills/New");
    }
  }

  ngOnInit(): void {
    this.skillForm = this.formBuilder.group({
      name: ['', Validators.required],
      code: ['', Validators.required],
      description: [],
    });

    if (this.skillData != null)
      this.fnBindData();
  }

  fnBindData() {
    this.formControls['name'].setValue(this.skillData.name);
    this.formControls['code'].setValue(this.skillData.code);
    this.formControls['description'].setValue(this.skillData.description);
  }

  close() {
    this.dialogRef.close();
  }

  addOrUpdate() {
    if (this.skillForm.valid) {

      let requestModel: CreateSkillModel = {
        name: this.formControls.name.value,
        code: this.formControls.code.value,
        description: this.formControls.description.value
      }

      if (this.skillData == null) {
        this._service.addSkill(requestModel).subscribe((result: any) => {
          this.dialogRef.close(result);
        });
      }
      else {
        let skillId: number = this.skillData.id;
        this._service.updateSkill(skillId, requestModel).subscribe((result: any) => {
          this.dialogRef.close(result);
        });
      }
    }
    else {
      this._gFuncs.validateAllFormFields(this.skillForm);
    }
  }

}
