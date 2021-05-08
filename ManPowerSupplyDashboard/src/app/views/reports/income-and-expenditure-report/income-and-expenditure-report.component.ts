import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { IncomeAndExpenditureModel, AccountHeadModel } from 'src/app/models/man-power-models';
import { MessagingService } from 'src/app/services/messaging.service';
import { MatDialog } from '@angular/material/dialog';
import { ManPowerService } from 'src/app/services/man-power.service';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';
import { PrintColumn, PrintService } from 'src/app/services/print.service';

@Component({
  selector: 'app-income-and-expenditure-report',
  templateUrl: './income-and-expenditure-report.component.html',
  styleUrls: ['./income-and-expenditure-report.component.scss']
})
export class IncomeAndExpenditureReportComponent implements OnInit {

  incomeAndExpenditureForm: FormGroup;
  get formControls() { return this.incomeAndExpenditureForm.controls; }

  accountHeads: AccountHeadModel[] = [];
  filteredAccountHeads: AccountHeadModel[] = [];
  selectedAccountHeadId?: number = null;

  incomeAndExpenditures: IncomeAndExpenditureModel = new IncomeAndExpenditureModel();
  public searchText: string;
  filterMetadata = { count: 0, sums: [] };

  transactionTypes: any[] = [
    { id: 1, name: 'Receipt' },
    { id: 2, name: 'Payment' }
  ]

  constructor(private _messaging: MessagingService,
    public dialog: MatDialog,
    private formBuilder: FormBuilder,
    private _service: ManPowerService,
    private gFuncs: GlobalFunctionsService,
    private _print: PrintService) {

  }

  ngOnInit(): void {
    this._messaging.changeHeaderRouteMessage('Reports/Income And Expenditure');

    this.incomeAndExpenditureForm = this.formBuilder.group({
      fromDate: ['', Validators.required],
      toDate: ['', Validators.required],
      accountHeadId: [''],
      transactionTypeId: ['']
    });

    let date = new Date();
    let fromDate = new Date(date.getFullYear(), date.getMonth(), 1);
    this.formControls.fromDate.setValue(fromDate);
    this.formControls.toDate.setValue(date);

    this.fnLoadAllAccountHeads();
    this.fnLoadAllIncomeAndExpenditures();
  }

  fnLoadAllAccountHeads(): void {
    let param = "";
    this._service.getAccountHeads(param).subscribe((result: any[]) => {
      this.accountHeads = result;
      this.filteredAccountHeads = result;
      if (this.selectedAccountHeadId != null && this.selectedAccountHeadId != undefined)
        this.formControls.accountHeadId.setValue(this.selectedAccountHeadId);
    })
  }

  fnLoadAllIncomeAndExpenditures(): void {
    if (this.incomeAndExpenditureForm.valid) {
      let FromDate: Date = new Date(this.formControls.fromDate.value);
      let ToDate: Date = new Date(this.formControls.toDate.value);
      let AccountHeadId: any = this.formControls.accountHeadId.value;
      let TransactionType: any = this.formControls.transactionTypeId.value;
      let params: string[] = [];

      params.push('FromDate=' + this.gFuncs.convertToYYYMMDD(FromDate));
      params.push('ToDate=' + this.gFuncs.convertToYYYMMDD(ToDate));
      if (AccountHeadId != null && AccountHeadId != undefined && AccountHeadId != "") {
        params.push('AccountHeadId=' + AccountHeadId);
      }
      if (TransactionType != null && TransactionType != undefined && TransactionType != "") {
        params.push('TransactionType=' + TransactionType);
      }

      this._service.getIncomeAndExpenditureReport(params.join("&")).subscribe((result: any) => {
        this.incomeAndExpenditures = result;
      })

    }
    else {
      this.gFuncs.validateAllFormFields(this.incomeAndExpenditureForm);
    }
  }

  print() {
    if (this.incomeAndExpenditures == null || this.incomeAndExpenditures == undefined) {
      this.gFuncs.openSnackBar("Sorry you can't print empty list");
      return;
    }

    this.formControls.accountHeadId.value;

    let printColumns: PrintColumn[] = [
      {
        columnName: 'date',
        displayname: 'Date',
        isDate: true
      },
      {
        columnName: 'accountHeadName',
        displayname: 'Account Head Name'
      },
      {
        columnName: 'particular',
        displayname: 'Particulars'
      },
      {
        columnName: 'receivedAmount',
        displayname: 'Received Amount'
      },
      {
        columnName: 'paidAmount',
        displayname: 'Paid Amount'
      },
      {
        columnName: 'balance',
        displayname: 'Balance'
      }
    ]

    let FromDate: Date = new Date(this.formControls.fromDate.value);
    let ToDate: Date = new Date(this.formControls.toDate.value);
    let accountHeadName = 'ALL';
    let AccountHeadId = this.formControls.accountHeadId.value
    if (AccountHeadId != null && AccountHeadId != undefined && AccountHeadId != "")
      accountHeadName = this.accountHeads.find(x => x.id == AccountHeadId).name;

    let pageHeaders: string[] = [
      `Viswas Man Power Supply, Chowallor Branch - Income & Expenditure Ledger From ${this.gFuncs.convertToDDMMYYYY(FromDate)} to ${this.gFuncs.convertToDDMMYYYY(ToDate)}`,
      `Account Name : ${accountHeadName}`
    ]
    let template = this._print.incomeAndExpenditureTemplate(this.incomeAndExpenditures, printColumns, pageHeaders);
    this._print.print(template);
  }

  public selectedRow: number;
  public highlightRow(id: number) {
    this.selectedRow = id;
  }

}
