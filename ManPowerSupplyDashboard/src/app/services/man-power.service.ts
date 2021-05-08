import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { CreateCustomerModel, CreateSkillModel, CreateEmployeeModel, CreateAttendanceModel, CreateCustomerReceiptModel, CreateEmployeePaymentModel, CreateAccountHeadModel, CreateReceiptAndPaymentModel } from '../models/man-power-models';
import { DatePipe } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class ManPowerService {
  private baseUrl: string = `${environment.apiUrl}ManPower/`;

  constructor(private _http: HttpClient, public datepipe: DatePipe) {

  }

  //#region Customers
  getAllCustomers() {
    let url = `${this.baseUrl}Customers`;
    return this._http.get(url).pipe();
  }

  addCustomer(requestModel: CreateCustomerModel) {
    let url = `${this.baseUrl}Customers`;
    return this._http.post(url, requestModel).pipe();

  }

  updateCustomer(customerId: number, requestModel: CreateCustomerModel) {
    let url = `${this.baseUrl}Customers/CustomerId/${customerId}`;
    return this._http.put(url, requestModel).pipe();
  }

  deleteCustomer(customerId: number) {
    let url = `${this.baseUrl}Customers/CustomerId/${customerId}`;
    return this._http.delete(url).pipe();
  }

  activateCustomer(customerId: number) {
    let url = `${this.baseUrl}Customers/CustomerId/${customerId}/Activate`;
    return this._http.put(url, null).pipe();
  }
  //#endregion

  //#region Skills
  getAllSkills() {
    let url = `${this.baseUrl}EmployeeSkills`;
    return this._http.get(url).pipe();
  }

  addSkill(requestModel: CreateSkillModel) {
    let url = `${this.baseUrl}EmployeeSkills`;
    return this._http.post(url, requestModel).pipe();
  }

  updateSkill(skillId: number, requestModel: CreateSkillModel) {
    let url = `${this.baseUrl}EmployeeSkills/${skillId}`;
    return this._http.put(url, requestModel).pipe();
  }
  //#endregion

  //#region Employee
  getAllEmployees() {
    let url = `${this.baseUrl}Employees`;
    return this._http.get(url).pipe();
  }

  addEmployee(requestModel: CreateEmployeeModel) {
    let url = `${this.baseUrl}Employees`;
    return this._http.post(url, requestModel).pipe();
  }

  updateEmployee(employeeId: number, requestModel: CreateEmployeeModel) {
    let url = `${this.baseUrl}Employees/${employeeId}`;
    return this._http.put(url, requestModel).pipe();
  }
  //#endregion


  //#region Attendance
  getAttendance(date: Date) {
    let strDate = this.datepipe.transform(date, 'yyyy-MM-dd');
    let url = `${this.baseUrl}Attendance/Date/${strDate}`;
    return this._http.get(url).pipe();
  }

  addAttendance(requestModel: CreateAttendanceModel) {
    let url = `${this.baseUrl}Attendance`;
    return this._http.post(url, requestModel).pipe();
  }

  updateAttendance(attendanceId: number, requestModel: CreateAttendanceModel) {
    let url = `${this.baseUrl}Attendance/${attendanceId}`;
    return this._http.put(url, requestModel).pipe();
  }

  deleteAttendance(attendanceId: number) {
    let url = `${this.baseUrl}Attendance/${attendanceId}`;
    return this._http.delete(url).pipe();
  }

  transerAttendace(date: Date) {
    let strDate = this.datepipe.transform(date, '"yyyy-MM-dd"');
    const headers = { 'Accept': 'application/json, text/plain, */*', 'Content-Type': 'application/json' };
    let url = `${this.baseUrl}Attendance/Transfer`;
    return this._http.post(url, strDate, { headers }).pipe();
  }
  //#endregion

  //#region Customer Receipts
  getCustomerReceipts(params: string) {
    let url = `${this.baseUrl}Receipt/Customer?${params}`;
    return this._http.get(url).pipe();
  }

  getCustomerReceiptCollectionPersons() {
    let url = `${this.baseUrl}Receipt/Customer/CollectionPersons`;
    return this._http.get(url).pipe();
  }

  addCustomerReceipt(requestModel: CreateCustomerReceiptModel) {
    let url = `${this.baseUrl}Receipt/Customer`;
    return this._http.post(url, requestModel).pipe();
  }

  updateCustomerReceipt(receiptId: number, requestModel: CreateCustomerReceiptModel) {
    let url = `${this.baseUrl}Receipt/Customer/ReceiptId/${receiptId}`;
    return this._http.put(url, requestModel).pipe();
  }

  deleteCustomerReceipt(receiptId: number) {
    let url = `${this.baseUrl}Receipt/Customer/ReceiptId/${receiptId}`;
    return this._http.delete(url).pipe();
  }
  //#endregion

  //#region Employee Payment
  getEmployeePayments(params: string) {
    let url = `${this.baseUrl}Payment/Employee?${params}`;
    return this._http.get(url).pipe();
  }

  addEmployeePayment(requestModel: CreateEmployeePaymentModel) {
    let url = `${this.baseUrl}Payment/Employee`;
    return this._http.post(url, requestModel).pipe();
  }

  updateEmployeePayment(paymentId: number, requestModel: CreateEmployeePaymentModel) {
    let url = `${this.baseUrl}Payment/Employee/PaymentId/${paymentId}`;
    return this._http.put(url, requestModel).pipe();
  }

  deleteEmployeePayment(paymentId: number) {
    let url = `${this.baseUrl}Payment/Employee/PaymentId/${paymentId}`;
    return this._http.delete(url).pipe();
  }

  getAllEmployeePaymentTypes() {
    let url = `${this.baseUrl}EmployeePaymentTypes`;
    return this._http.get(url).pipe();
  }
  //#endregion

  //#region Reports
  getCustomerLedger(params: string) {
    let url = `${this.baseUrl}Reports/CustomerLedger?${params}`;
    return this._http.get(url).pipe();
  }

  getEmployeeLedger(params: string) {
    let url = `${this.baseUrl}Reports/EmployeeLedger?${params}`;
    return this._http.get(url).pipe();
  }
  //#endregiona


  //#region Account Heads
  getAccountHeads(params:string ){
    let url = `${this.baseUrl}AccountHeads?${params}`;
    return this._http.get(url).pipe();
  }

  addAccountHead(requestModel: CreateAccountHeadModel) {
    let url = `${this.baseUrl}AccountHeads`;
    return this._http.post(url, requestModel).pipe();
  }

  //#endregion

  //#region ReceiptAndPayments
  getReceiptsAndPayments(params:string ){
    let url = `${this.baseUrl}ReceiptAndPayments?${params}`;
    return this._http.get(url).pipe();
  }

  addReceiptAndPayment(requestModel: CreateReceiptAndPaymentModel) {
    let url = `${this.baseUrl}ReceiptAndPayments`;
    return this._http.post(url, requestModel).pipe();
  }

  updateReceiptAndPayment(transactionId:number, requestModel: CreateReceiptAndPaymentModel) {
    let url = `${this.baseUrl}ReceiptAndPayments/TransactionId/${transactionId}`;
    return this._http.put(url, requestModel).pipe();
  }

  deleteReceiptAndPayment(transactionId:number, ) {
    let url = `${this.baseUrl}ReceiptAndPayments/TransactionId/${transactionId}`;
    return this._http.delete(url).pipe();
  }

  getIncomeAndExpenditureReport(params:string ){
    let url = `${this.baseUrl}Reports/IncomeAndExpenditure?${params}`;
    return this._http.get(url).pipe();
  }

  //#endregion
}