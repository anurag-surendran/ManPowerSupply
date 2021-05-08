import { Component, OnInit } from '@angular/core';
import { MessagingService } from 'src/app/services/messaging.service';
import { MatDialog } from '@angular/material/dialog';
import { ManPowerService } from 'src/app/services/man-power.service';
import { GlobalFunctionsService } from 'src/app/services/global-functions.service';
import { CustomerModel, CustomerReceiptModel } from 'src/app/models/man-power-models';
import { AddEditCustomerComponent } from './add-edit-customer/add-edit-customer.component';
import { AddEditCustomerReceiptComponent } from '../receipts-payments/customer-receipts/add-edit-customer-receipt/add-edit-customer-receipt.component';
import { Router, NavigationExtras } from '@angular/router';
import { PrintColumn, PrintService } from 'src/app/services/print.service';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.scss']
})
export class CustomersComponent implements OnInit {

  customers: CustomerModel[];
  public searchText: string;
  filterMetadata = { count: 0, sums: [] };

  constructor(private _messaging: MessagingService,
    public dialog: MatDialog,
    private router: Router,
    private _service: ManPowerService,
    private gFuncs: GlobalFunctionsService,
    private _print: PrintService) { }

  ngOnInit(): void {
    this._messaging.changeHeaderRouteMessage('Master/Customers');
    this.fnLoadAllCustomers();
  }

  fnLoadAllCustomers(): void {
    this._service.getAllCustomers().subscribe((result: any[] | any) => {
      this.customers = result;
    })
  }

  addOrUpdateCustomer(customerData?: CustomerModel): void {
    const addEditDialog = this.dialog.open(AddEditCustomerComponent, {
      width: '600px',
      panelClass: 'myapp-no-padding-dialog',
      height: '400px',
      data: customerData
    });

    addEditDialog.afterClosed().subscribe((result: CustomerModel) => {
      this._messaging.changeHeaderRouteMessage('Master/Customers');
      if (result != null) {
        let message = result.name + ' has been ' + (customerData == null ? 'added' : 'updated') + ' successfully'
        this.gFuncs.openSnackBar(message);
        if (customerData == null) {
          this.searchText = '';
        }
        this.fnLoadAllCustomers();
      }
    });
  }

  fnDeleteCustomer(customerId: number) {
    this._service.deleteCustomer(customerId).subscribe((result: CustomerModel | any) => {
      let message = 'Customer has ' + result.name + 'been Deactivated successfully'
      this.gFuncs.openSnackBar(message);
      this.customers.find(x => x.id === result.id).isDeleted = result.isDeleted;
    })
  }

  fnActivateCustomer(customerId: number) {
    this._service.activateCustomer(customerId).subscribe((result: CustomerModel) => {
      let message = 'Customer has ' + result.name + 'been Activated successfully'
      this.gFuncs.openSnackBar(message);
      this.customers.find(x => x.id === result.id).isDeleted = result.isDeleted;
    })
  }

  fnCustomerReceipt(customer) {
    const addEditDialog = this.dialog.open(AddEditCustomerReceiptComponent, {
      width: '600px',
      panelClass: 'myapp-no-padding-dialog',
      height: '400px',
      data: { type: 'NewPayment', customer: customer }
    });

    addEditDialog.afterClosed().subscribe((result: CustomerReceiptModel) => {
      this._messaging.changeHeaderRouteMessage('Master/Customers');
      if (result != null) {
        let message = 'Payment from ' + result.customerName + ' has been added successfully'
        this.gFuncs.openSnackBar(message);
        this.fnLoadAllCustomers();
      }
    });
  }

  fnViewReceiptTransactions(customer: CustomerModel) {
    const navigationExtras: NavigationExtras = { state: { data: customer } };
    this.router.navigate(['/receipt-payment/customer'], navigationExtras);
  }

  fnViewCustomerLedger(customer: CustomerModel) {
    const navigationExtras: NavigationExtras = { state: { data: customer } };
    this.router.navigate(['/reports/customer'], navigationExtras);
  }

  print() {
    if (this.customers == null || this.customers == undefined || this.customers.length == 0) {
      this.gFuncs.openSnackBar("Sorry you can't print empty list");
      return;
    }

    let printColumns: PrintColumn[] = [
      {
        columnName: 'name',
        displayname: 'Customer Name'
      },
      {
        columnName: 'mobile',
        displayname: 'Mobile'
      },
      {
        columnName: 'alternateMobile',
        displayname: 'Alternate Mobile'
      },
      {
        columnName: 'balanceAmount',
        displayname: 'Balance Amount'
      },
      {
        columnName: 'address',
        displayname: 'Address'
      }
    ]    

    let receivableCustomers = this.customers.filter(x => x.balanceAmount > 0);
    let sum: number = receivableCustomers.map(a => a.balanceAmount).reduce(function (a, b) {
      return a + b;
    });

    let customerTotal: CustomerModel = {
      isDeleted: false,
      lastUpdatedBy: '',
      lastUpdatedDate: new Date(),
      id: 0,
      name: '',
      mobile: 'Total',
      alternateMobile: '',
      address: '',
      balanceAmount: sum
    }

    receivableCustomers.push(customerTotal);

    let pageHeaders: string[] = [
      `Viswas Man Power Supply, Chowallor Branch - Customer Details`
    ]
    let template = this._print.listTemplate(receivableCustomers, printColumns, pageHeaders);
    this._print.print(template);
  }

  public selectedRow: number;
  public highlightRow(id: number) {
    this.selectedRow = id;
  }

}
