import { Component, HostListener, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CustomerModel, CustomerLedgerModel } from 'src/app/models/man-power-models';
import { MessagingService } from 'src/app/services/messaging.service';
import { MatDialog } from '@angular/material/dialog';
import { ManPowerService } from 'src/app/services/man-power.service';
import { Router } from '@angular/router';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';
import { PrintColumn, PrintService } from 'src/app/services/print.service';

@Component({
  selector: 'app-customer-ledger',
  templateUrl: './customer-ledger.component.html',
  styleUrls: ['./customer-ledger.component.scss']
})
export class CustomerLedgerComponent implements OnInit {
  customerLedgerForm: FormGroup;
  get formControls() { return this.customerLedgerForm.controls; }

  customers: CustomerModel[] = [];
  filteredCustomers: CustomerModel[] = [];

  customerLedgers: CustomerLedgerModel = new CustomerLedgerModel();
  public searchText: string;
  filterMetadata = { count: 0, sums: [] };

  navigateCustomerId: number;
  constructor(private _messaging: MessagingService,
    public dialog: MatDialog,
    private formBuilder: FormBuilder,
    private _service: ManPowerService,
    private router: Router,
    private gFuncs: GlobalFunctionsService,
    private _print: PrintService) {

    const navigation = this.router.getCurrentNavigation();
    const state = navigation.extras.state as { data: CustomerModel };
    if (state != null && state != undefined)
      this.navigateCustomerId = state.data.id;
  }

  ngOnInit(): void {
    this._messaging.changeHeaderRouteMessage('Reports/Customer Ledger');

    this.customerLedgerForm = this.formBuilder.group({
      fromDate: ['', Validators.required],
      toDate: ['', Validators.required],
      customerId: ['', Validators.required]
    });

    let date = new Date();
    let fromDate = new Date(date.getFullYear(), date.getMonth(), 1)
    this.formControls.fromDate.setValue(fromDate);
    this.formControls.toDate.setValue(date);

    this.fnLoadAllCustomers();
    if (this.navigateCustomerId != null && this.navigateCustomerId != undefined)
      return;
  }

  fnLoadAllCustomers(): void {
    this._service.getAllCustomers().subscribe((result: any[]) => {
      this.customers = result;
      this.filteredCustomers = result;
      this.gFuncs.validateAllFormFields(this.customerLedgerForm);
      if (this.navigateCustomerId != null && this.navigateCustomerId != undefined) {
        this.formControls.customerId.setValue(this.navigateCustomerId);
        this.fnLoadAllCustomerLedgers();
      }
    })
  }

  fnLoadAllCustomerLedgers(): void {
    if (this.customerLedgerForm.valid) {
      let FromDate: Date = new Date(this.formControls.fromDate.value);
      let ToDate: Date = new Date(this.formControls.toDate.value);
      let CustomerId: any = this.formControls.customerId.value;
      let params: string[] = [];

      params.push('FromDate=' + this.gFuncs.convertToYYYMMDD(FromDate));
      params.push('ToDate=' + this.gFuncs.convertToYYYMMDD(ToDate));
      if (CustomerId != null && CustomerId != undefined && CustomerId != "") {
        params.push('CustomerId=' + CustomerId);
      }

      this._service.getCustomerLedger(params.join("&")).subscribe((result: any) => {
        this.customerLedgers = result;
      })

    }
    else {
      this.gFuncs.validateAllFormFields(this.customerLedgerForm);
    }
  }

  public selectedRow: number;
  public highlightRow(id: number) {
    this.selectedRow = id;
  }

  print() {
    if (this.customerLedgers.particulars == undefined) {
      this.gFuncs.openSnackBar("Sorry you can't print empty list");
      return;
    }

    let printColumns: PrintColumn[] = [
      {
        columnName: 'date',
        displayname: 'Date',
        isDate: true
      },
      {
        columnName: 'particular',
        displayname: 'Particulars'
      },
      {
        columnName: 'customerPay',
        displayname: 'Payment'
      },
      {
        columnName: 'customerTA',
        displayname: 'TA'
      },
      {
        columnName: 'rent',
        displayname: 'Rent'
      },
      {
        columnName: 'received',
        displayname: 'Receipt'
      },
      {
        columnName: 'balance',
        displayname: 'Balance'
      }
    ]

    let FromDate: Date = new Date(this.formControls.fromDate.value);
      let ToDate: Date = new Date(this.formControls.toDate.value);
      let SelectedCutomer = this.customers.find(x=>x.id == this.formControls.customerId.value).name;

    let pageHeaders:string[] = [
      `Viswas Man Power Supply, Chowallor Branch - Customer Personal Ledger From ${this.gFuncs.convertToDDMMYYYY(FromDate)} to ${this.gFuncs.convertToDDMMYYYY(ToDate)}`,
      `Customer Name : ${SelectedCutomer}` 
    ]
    let data = this._print.ledgerTemplate(this.customerLedgers, printColumns,pageHeaders);
    let template = data;
    this._messaging.changePrintTemplate(template);
  }
}
