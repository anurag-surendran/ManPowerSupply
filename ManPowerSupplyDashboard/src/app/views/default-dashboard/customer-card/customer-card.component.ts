import { Component, OnInit } from '@angular/core';
import { CustomerModel } from 'src/app/models/man-power-models';
import { ManPowerService } from 'src/app/services/man-power.service';
import { PrintColumn, PrintService } from 'src/app/services/print.service';

@Component({
  selector: 'app-customer-card',
  templateUrl: './customer-card.component.html',
  styleUrls: ['./customer-card.component.scss']
})
export class CustomerCardComponent implements OnInit {

  customers: CustomerModel[];
  public searchText: string;
  filterMetadata = { count: 0, sums: [] };

  constructor(private _service: ManPowerService,
    private _print: PrintService) { }

  ngOnInit(): void {
    this.fnLoadAllCustomers();
  }

  fnLoadAllCustomers(): void {
    this._service.getAllCustomers().subscribe((result: CustomerModel[]) => {
      this.customers = result.filter(x=>x.balanceAmount != 0);
      SortUtil.sortByProperty<CustomerModel>(this.customers,"balanceAmount","DESC")
    })
  }

  print(){
    if (this.customers == null  || this.customers == undefined || this.customers.length == 0) {
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

export class SortUtil {

  static sortByProperty<T>(array: T[], propName: keyof T, order: 'ASC' | 'DESC'): void {
      array.sort((a, b) => {
          if (a[propName] < b[propName]) {
              return -1;
          }

          if (a[propName] > b[propName]) {
              return 1;
          }
          return 0;
      });

      if (order === 'DESC') {
          array.reverse();
      }
  }
}
