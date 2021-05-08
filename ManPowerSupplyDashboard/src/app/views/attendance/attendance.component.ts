import { Component, OnInit } from '@angular/core';
import { AttendanceModel } from 'src/app/models/man-power-models';
import { MessagingService } from 'src/app/services/messaging.service';
import { MatDialog } from '@angular/material/dialog';
import { ManPowerService } from 'src/app/services/man-power.service';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';
import { AddEditAttendanceComponent } from './add-edit-attendance/add-edit-attendance.component';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { ConfirmDialogModel, ConfirmationDialogComponent } from 'src/app/shared/confirmation-dialog/confirmation-dialog.component';
import { PrintColumn, PrintService } from 'src/app/services/print.service';

@Component({
  selector: 'app-attendance',
  templateUrl: './attendance.component.html',
  styleUrls: ['./attendance.component.scss']
})
export class AttendanceComponent implements OnInit {

  attendances: AttendanceModel[];
  public searchText: string;
  filterMetadata = { count: 0, sums: [] };

  isTrnsferData: boolean = false;
  date: Date = new Date();

  constructor(private _messaging: MessagingService,
    public dialog: MatDialog,
    private _service: ManPowerService,
    private gFuncs: GlobalFunctionsService,
    private _print: PrintService) { }

  ngOnInit(): void {
    this._messaging.changeHeaderRouteMessage('Master/Attendances');
    this.fnLoadAttendances();
    this.fnCheckTransferStatus();
  }

  fnLoadAttendances(): void {
    this.attendances = null;
    this._service.getAttendance(this.date).subscribe((result: any[] | any) => {
      this.attendances = result;
    })
  }

  addOrUpdateAttendance(attendanceData?: AttendanceModel): void {

    if (attendanceData != null && attendanceData.isDeleted) {
      this.gFuncs.openSnackBar("Sorry! You can't modify or view deleted attendance data.");
      return;
    }

    const addEditDialog = this.dialog.open(AddEditAttendanceComponent, {
      width: '900px',
      panelClass: 'myapp-no-padding-dialog',
      height: '450px',
      data: [attendanceData, this.date],
      disableClose: true
    });

    addEditDialog.afterClosed().subscribe((result: AttendanceModel) => {
      this._messaging.changeHeaderRouteMessage('Master/Attendances');
      if (result != null) {
        let message = 'Attendance details been' + (attendanceData == null ? 'added' : 'updated') + ' successfully'
        this.gFuncs.openSnackBar(message);
        if (attendanceData == null) {
          this.searchText = '';
        }
        this.fnLoadAttendances();
        this.fnCheckTransferStatus();
      }
    });
  }

  onDateChange(event: MatDatepickerInputEvent<Date>) {
    this.date = event.value;
    if (this.date != null) {
      this.fnLoadAttendances();
      this.fnCheckTransferStatus();
    }
  }

  fnCheckTransferStatus() {
    this.isTrnsferData = false;
    let date = new Date(this.date);

    date.setDate(date.getDate() + 1);
    this.attendances = null;
    this._service.getAttendance(date).subscribe((result: any[] | any) => {
      if (result.length == 0) {
        this.isTrnsferData = true;
      }
    })
  }

  fnTransferAttendance() {

    //Validate Attendance
    let unUpdatedAttendacne = this.attendances.filter(x => x.attendanceStatus == null).length;

    if(unUpdatedAttendacne > 0)
    {
      this.gFuncs.openSnackBar(unUpdatedAttendacne+' item(s) attendance status not given. Please update all attendance status and try again');
      return;
    }
    

    const message = `Do you want to transfer?`;

    const dialogData = new ConfirmDialogModel("Confirmation", message);

    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      maxWidth: "400px",
      data: dialogData,
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((dialogResult: boolean) => {
      if (dialogResult) {
        this._service.transerAttendace(this.date).subscribe((result: boolean | any) => {
          if (result)
            this.gFuncs.openSnackBar("Successfully Transfered");
        })
      }
    });
  }

  fnDeleteAttendance(attendanceId: number) {
    const message = `Do you want to delete?`;

    const dialogData = new ConfirmDialogModel("Confirmation", message);

    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      maxWidth: "400px",
      data: dialogData,
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((dialogResult: boolean) => {
      if (dialogResult) {
        this._service.deleteAttendance(attendanceId).subscribe((result: AttendanceModel | any) => {
          if (result.isDeleted)
            this.gFuncs.openSnackBar("Successfully Deleted");
          this.fnLoadAttendances();
        });
      }
    });

  }

  print(){
    if (this.attendances == null  || this.attendances == undefined || this.attendances.length == 0) {
      this.gFuncs.openSnackBar("Sorry you can't print empty list");
      return;
    }

    let printColumns: PrintColumn[] = [
      {
        columnName: 'employeeName',
        displayname: 'Employee'
      },
      {
        columnName: 'customerName',
        displayname: 'Customer'
      },
      {
        columnName: 'attendanceStatus',
        displayname: 'Attendance'
      },
      {
        columnName: 'customerPay',
        displayname: 'Customer Pay'
      },
      {
        columnName: 'customerTA',
        displayname: 'Customer TA'
      },
      {
        columnName: 'rent',
        displayname: 'Rent'
      },
      {
        columnName: 'companyTA',
        displayname: 'Company TA'
      },
      {
        columnName: 'employeePay',
        displayname: 'Employee Pay'
      },
    ]

    let attendanceList:any[] = this.attendances;
    let totalAttendanceModel = {
      id: 0,
      date: '',
      employeeId: 0,
      employeeName: 'Total',
      customerId: '',
      customerName: '',
      attendanceStatus: '',
      nextDayCustomerId: '',
      nextDayCustomerName: '',
      customerPay: this.filterMetadata.sums[0],
      rent: this.filterMetadata.sums[2],
      customerTA: this.filterMetadata.sums[1],
      companyTA: this.filterMetadata.sums[3],
      employeePay: this.filterMetadata.sums[4],
      remarks: '',
      skill: '',
      skillCode: '',
      isDeleted: '',
      lastUpdatedBy: ''
    }

    attendanceList.push(totalAttendanceModel);

    let pageHeaders:string[] = [
      `Viswas Man Power Supply, Chowallor Branch - Employee Details` 
    ]
    let template = this._print.listTemplate(attendanceList, printColumns,pageHeaders);
    this._print.print(template);
  }

  public selectedRow: number | any;
  public highlightRow(id: number) {
    this.selectedRow = id;
  }

}
