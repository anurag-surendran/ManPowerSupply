import { Injectable } from '@angular/core';
import { GlobalFunctionsService } from './global-functions.service';
import { MessagingService } from './messaging.service';

@Injectable({
  providedIn: 'root'
})
export class PrintService {

  constructor(
    private gFunc: GlobalFunctionsService,
    private _messaging: MessagingService
  ) { } 

  listTemplate(list:any,requiredColumns: PrintColumn[],pageHeaders:string[]):string{
    let pageHeaderDivs : string ='';
     pageHeaders.forEach(header=>{
      pageHeaderDivs+= `<div class="print-page-header">${header}</div>\n`
    })
    
    let headers = '\t<th class="print-table-th">#</th>\n';
    requiredColumns.forEach((col: PrintColumn) => {
      headers += `\t<th class="print-table-th">${col.displayname}</th>\n`;
    })

    let body = '';
    let slNo = 1;
    list.forEach((data: any) => {
      body += `<tr>\n\t<td class="print-table-td">${slNo}</td>\n`;
      requiredColumns.forEach((col: PrintColumn) => {
        var currentValue = data[col.columnName]==null? '-' : data[col.columnName];
        let val = col.isDate ? this.gFunc.convertToDDMMYYYY(currentValue) : currentValue;
        body += `\t<td class="print-table-td">${val}</td>\n`
      });
      body += '\n</tr>'
      slNo++;
    });


    var template = `<style>
                      .print-page-header{
                        font-family: TimesNewRoman, "Times New Roman", Times, Baskerville, Georgia, serif;
                        font-size: 16px;
                        font-weight: bold;
                      }
                      .print-table-th {
                        border-right: 1px dashed;
                        border-bottom: 1px dashed;                        
                        border-top: 1px dashed;
                        padding-left: 10px;
                        padding-right:5px;
                        color: black;
                        font-weight: 550;
                        font-family: TimesNewRoman, "Times New Roman", Times, Baskerville, Georgia, serif;
                      }
                      .print-table{
                        margin-top:5px;
                        width: 100%;
                        min-width: 500px;
                        border-spacing: 0;
                        border-left: 1px dashed;
                        font-family: TimesNewRoman, "Times New Roman", Times, Baskerville, Georgia, serif;
                      }
                      .print-table-td {
                        border-right: 1px dashed;
                        border-bottom: 1px dashed;
                        color: black;
                        padding-left: 10px;
                        padding-right:5px;
                        font-family: TimesNewRoman, "Times New Roman", Times, Baskerville, Georgia, serif;
                      }
                    </style>
                    ${pageHeaderDivs}
                    <table class="print-table">                     
                      <thead>
                        <tr>
                            ${headers}
                        </tr>
                      </thead>
                      <tbody>                        
                        </tr>
                          ${body}
                        <tr>
                      </tbody>
                    </table>`;
    return template;
  }
  
  ledgerTemplate(ledger:any,requiredColumns: PrintColumn[],pageHeaders:string[]):string{
    
    let pageHeaderDivs : string ='';
     pageHeaders.forEach(header=>{
      pageHeaderDivs+= `<div class="print-page-header">${header}</div>\n`
    })
    
    let headers = '';
    requiredColumns.forEach((col: PrintColumn) => {
      headers += `\t<th class="print-table-th">${col.displayname}</th>\n`;
    })

    let body = '';

    ledger.particulars.forEach((data: any) => {
      body += '<tr>\n'
      requiredColumns.forEach((col: PrintColumn) => {
        var currentValue = data[col.columnName];
        let val = col.isDate ? this.gFunc.convertToDDMMYYYY(currentValue) : currentValue;
        body += `\t<td class="print-table-td">${val}</td>\n`
      });
      body += '\n</tr>'
    });


    var template = `<style>
                      .print-page-header{
                        font-family: TimesNewRoman, "Times New Roman", Times, Baskerville, Georgia, serif;
                        font-size: 16px;
                        font-weight: bold;
                      }
                      .print-table-th {
                        border-right: 1px dashed;
                        border-bottom: 1px dashed;                        
                        border-top: 1px dashed;
                        padding-left: 10px;
                        color: black;
                        font-weight: 550;
                        font-family: TimesNewRoman, "Times New Roman", Times, Baskerville, Georgia, serif;
                      }
                      .print-table{
                        margin-top:5px;
                        width: 100%;
                        min-width: 500px;
                        border-spacing: 0;
                        border-left: 1px dashed;
                        font-family: TimesNewRoman, "Times New Roman", Times, Baskerville, Georgia, serif;
                      }
                      .print-table-td {
                        border-right: 1px dashed;
                        border-bottom: 1px dashed;
                        color: black;
                        padding-left: 10px;
                        font-family: TimesNewRoman, "Times New Roman", Times, Baskerville, Georgia, serif;
                      }
                    </style>
                    ${pageHeaderDivs}
                    <table class="print-table">                     
                      <thead>
                        <tr>
                            ${headers}
                        </tr>
                      </thead>
                      <tbody>
                        <tr>
                          <td class="print-table-td"></td>
                          <td class="print-table-td">Previuos Balance</td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td">${ledger.openingBalance}</td>
                        </tr>
                          ${body}
                        <tr>
                          <td class="print-table-td"></td>
                          <td class="print-table-td">Total</td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td">${ledger.closingBalance}</td>
                        </tr>
                      </tbody>
                    </table>`;
    return template;
  }

  employeeTemplate(ledger:any,requiredColumns: PrintColumn[],pageHeaders:string[]):string{
    
    let pageHeaderDivs : string ='';
     pageHeaders.forEach(header=>{
      pageHeaderDivs+= `<div class="print-page-header">${header}</div>\n`
    })
    
    let headers = '\t<th class="print-table-th">#</th>\n';
    requiredColumns.forEach((col: PrintColumn) => {
      headers += `\t<th class="print-table-th">${col.displayname}</th>\n`;
    })

    let body = '';
    let slNo = 1;
    ledger.particulars.forEach((data: any) => {
      body += `<tr>\n\t<td class="print-table-td">${slNo}</td>\n`;
      requiredColumns.forEach((col: PrintColumn) => {
        var currentValue = data[col.columnName]==null? '-' : data[col.columnName];
        let val = col.isDate ? this.gFunc.convertToDDMMYYYY(currentValue) : currentValue;
        body += `\t<td class="print-table-td">${val}</td>\n`
      });
      body += '\n</tr>'
      slNo++;
    });


    var template = `<style>
                      .print-page-header{
                        font-family: TimesNewRoman, "Times New Roman", Times, Baskerville, Georgia, serif;
                        font-size: 16px;
                        font-weight: bold;
                      }
                      .print-table-th {
                        border-right: 1px dashed;
                        border-bottom: 1px dashed;                        
                        border-top: 1px dashed;
                        padding-left: 10px;
                        color: black;
                        font-weight: 550;
                        font-family: TimesNewRoman, "Times New Roman", Times, Baskerville, Georgia, serif;
                      }
                      .print-table{
                        margin-top:5px;
                        width: 100%;
                        min-width: 500px;
                        border-spacing: 0;
                        border-left: 1px dashed;
                        font-family: TimesNewRoman, "Times New Roman", Times, Baskerville, Georgia, serif;
                      }
                      .print-table-td {
                        border-right: 1px dashed;
                        border-bottom: 1px dashed;
                        color: black;
                        padding-left: 10px;
                        font-family: TimesNewRoman, "Times New Roman", Times, Baskerville, Georgia, serif;
                      }
                    </style>
                    ${pageHeaderDivs}
                    <table class="print-table">                     
                      <thead>
                        <tr>
                            ${headers}
                        </tr>
                      </thead>
                      <tbody>
                        <tr>
                          <td class="print-table-td"></td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td">Previuos Balance</td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td">${ledger.openingBalance}</td>
                        </tr>
                          ${body}
                        <tr>
                          <td class="print-table-td"></td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td">Total</td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td">${ledger.closingBalance}</td>
                        </tr>
                      </tbody>
                    </table>`;
    return template;
  }

  incomeAndExpenditureTemplate(ledger:any,requiredColumns: PrintColumn[],pageHeaders:string[]):string{
    
    let pageHeaderDivs : string ='';
     pageHeaders.forEach(header=>{
      pageHeaderDivs+= `<div class="print-page-header">${header}</div>\n`
    })
    
    let headers = '\t<th class="print-table-th">#</th>\n';
    requiredColumns.forEach((col: PrintColumn) => {
      headers += `\t<th class="print-table-th">${col.displayname}</th>\n`;
    })

    let body = '';
    let slNo = 1;
    ledger.particulars.forEach((data: any) => {
      body += `<tr>\n\t<td class="print-table-td">${slNo}</td>\n`;
      requiredColumns.forEach((col: PrintColumn) => {
        var currentValue = data[col.columnName]==null? '-' : data[col.columnName];
        let val = col.isDate ? this.gFunc.convertToDDMMYYYY(currentValue) : currentValue;
        body += `\t<td class="print-table-td">${val}</td>\n`
      });
      body += '\n</tr>'
      slNo++;
    });


    var template = `<style>
                      .print-page-header{
                        font-family: TimesNewRoman, "Times New Roman", Times, Baskerville, Georgia, serif;
                        font-size: 16px;
                        font-weight: bold;
                      }
                      .print-table-th {
                        border-right: 1px dashed;
                        border-bottom: 1px dashed;                        
                        border-top: 1px dashed;
                        padding-left: 10px;
                        color: black;
                        font-weight: 550;
                        font-family: TimesNewRoman, "Times New Roman", Times, Baskerville, Georgia, serif;
                      }
                      .print-table{
                        margin-top:5px;
                        width: 100%;
                        min-width: 500px;
                        border-spacing: 0;
                        border-left: 1px dashed;
                        font-family: TimesNewRoman, "Times New Roman", Times, Baskerville, Georgia, serif;
                      }
                      .print-table-td {
                        border-right: 1px dashed;
                        border-bottom: 1px dashed;
                        color: black;
                        padding-left: 10px;
                        font-family: TimesNewRoman, "Times New Roman", Times, Baskerville, Georgia, serif;
                      }
                    </style>
                    ${pageHeaderDivs}
                    <table class="print-table">                     
                      <thead>
                        <tr>
                            ${headers}
                        </tr>
                      </thead>
                      <tbody>
                        <tr>
                          <td class="print-table-td"></td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td">Previuos Balance</td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td">${ledger.openingBalance}</td>
                        </tr>
                          ${body}
                        <tr>
                          <td class="print-table-td"></td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td">Total</td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td"></td>
                          <td class="print-table-td">${ledger.closingBalance}</td>
                        </tr>
                      </tbody>
                    </table>`;
    return template;
  }

  print(template:string){
    this._messaging.changePrintTemplate(template);
  }
}

export class PrintColumn {
  columnName: string;
  displayname: string;
  isDate?: boolean = false;
}
