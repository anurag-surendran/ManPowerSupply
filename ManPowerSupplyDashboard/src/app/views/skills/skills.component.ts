import { Component, OnInit } from '@angular/core';
import { SkillModel } from 'src/app/models/man-power-models';
import { AddEditSkillComponent } from './add-edit-skill/add-edit-skill.component';
import { MessagingService } from 'src/app/services/messaging.service';
import { MatDialog } from '@angular/material/dialog';
import { ManPowerService } from 'src/app/services/man-power.service';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';

@Component({
  selector: 'app-skills',
  templateUrl: './skills.component.html',
  styleUrls: ['./skills.component.scss']
})
export class SkillsComponent implements OnInit {

  skills: SkillModel[];
  public searchText: string;
  filterMetadata = { count: 0 };

  constructor(private _messaging: MessagingService,
    public dialog: MatDialog,
    private _service: ManPowerService,
    private gFuncs: GlobalFunctionsService) { }

  ngOnInit(): void {
    this._messaging.changeHeaderRouteMessage('Master/Skills');
    this.fnLoadAllSkills();
  }

  fnLoadAllSkills(): void {
    this._service.getAllSkills().subscribe((result: any[]) => {
      this.skills = result;
    })
  }

  addOrUpdateSkill(skillData?: SkillModel): void {
    const addEditDialog = this.dialog.open(AddEditSkillComponent, {
      width: '600px',
      panelClass: 'myapp-no-padding-dialog',
      height: '400px',
      data: skillData
    });

    addEditDialog.afterClosed().subscribe((result: SkillModel) => {
      this._messaging.changeHeaderRouteMessage('Master/Skills');
      if (result != null) {
        let message = result.name + ' has been ' + (skillData == null ? 'added' : 'updated') + ' successfully'
        this.gFuncs.openSnackBar(message);
        if(skillData==null){
          this.searchText='';
        }
        this.fnLoadAllSkills();
      }
    });
  }

  public selectedRow: number;
  public highlightRow(id: number) {
    this.selectedRow = id;
  }

}
