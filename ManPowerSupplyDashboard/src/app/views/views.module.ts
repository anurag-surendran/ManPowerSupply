import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DragDropModule } from '@angular/cdk/drag-drop'

//Angular Materials
import { MatDividerModule } from '@angular/material/divider'
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { DateAdapter, MatNativeDateModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatBadgeModule } from '@angular/material/badge';
import { MatBottomSheetModule } from '@angular/material/bottom-sheet';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatStepperModule } from '@angular/material/stepper';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSliderModule } from '@angular/material/slider';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatTreeModule } from '@angular/material/tree';
import { MatSelectFilterModule } from 'mat-select-filter';


import { DefaultDashboardComponent } from './default-dashboard/default-dashboard.component';
import { CustomersComponent } from './customers/customers.component';
import { SkillsComponent } from './skills/skills.component';
import { AddEditSkillComponent } from './skills/add-edit-skill/add-edit-skill.component';
import { EmployeesComponent } from './employees/employees.component';
import { AddEditEmployeeComponent } from './employees/add-edit-employee/add-edit-employee.component';
import { AddEditCustomerComponent } from './customers/add-edit-customer/add-edit-customer.component';

import { ServicesModule } from '../services/services.module';
import { SharedModule } from '../shared/shared.module';
import { AttendanceComponent } from './attendance/attendance.component';
import { AddEditAttendanceComponent } from './attendance/add-edit-attendance/add-edit-attendance.component';
import { CustomerReceiptsComponent } from './receipts-payments/customer-receipts/customer-receipts.component';
import { AddEditCustomerReceiptComponent } from './receipts-payments/customer-receipts/add-edit-customer-receipt/add-edit-customer-receipt.component';
import { EmployeePaymentsComponent } from './receipts-payments/employee-payments/employee-payments.component';
import { AddEditEmployeePaymentComponent } from './receipts-payments/employee-payments/add-edit-employee-payment/add-edit-employee-payment.component';
import { ReceiptsComponent } from './receipts-payments/receipts/receipts.component';
import { AddEditReceiptComponent } from './receipts-payments/receipts/add-edit-receipt/add-edit-receipt.component';
import { PaymentsComponent } from './receipts-payments/payments/payments.component';
import { AddEditPaymentComponent } from './receipts-payments/payments/add-edit-payment/add-edit-payment.component';
import { CustomerLedgerComponent } from './reports/customer-ledger/customer-ledger.component';
import { EmployeeLedgerComponent } from './reports/employee-ledger/employee-ledger.component';
import { IncomeAndExpenditureReportComponent } from './reports/income-and-expenditure-report/income-and-expenditure-report.component';
import { DateFormat } from './date-format';
import { AddReceiptPaymentAccountHeadComponent } from './receipts-payments/add-receipt-payment-account-head/add-receipt-payment-account-head.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { AddUserComponent } from './add-user/add-user.component';
import { UpdatePasswordComponent } from './update-password/update-password.component';
import { CountCardComponent } from './default-dashboard/count-card/count-card.component';
import { AttendanceCardComponent } from './default-dashboard/attendance-card/attendance-card.component';
import { ChartsModule } from 'ng2-charts';
import { CustomerCardComponent } from './default-dashboard/customer-card/customer-card.component';



@NgModule({
  declarations: [
    DefaultDashboardComponent,
    CustomersComponent,
    AddEditCustomerComponent,
    SkillsComponent,
    AddEditSkillComponent,
    EmployeesComponent,
    AddEditEmployeeComponent,
    AttendanceComponent,
    AddEditAttendanceComponent,
    CustomerReceiptsComponent,
    AddEditCustomerReceiptComponent,
    EmployeePaymentsComponent,
    AddEditEmployeePaymentComponent,
    ReceiptsComponent,
    AddEditReceiptComponent,
    PaymentsComponent,
    AddEditPaymentComponent,
    CustomerLedgerComponent,
    EmployeeLedgerComponent,
    IncomeAndExpenditureReportComponent,
    AddReceiptPaymentAccountHeadComponent,
    UserProfileComponent,
    AddUserComponent,
    UpdatePasswordComponent,
    CountCardComponent,
    AttendanceCardComponent,
    CustomerCardComponent
  ],
  imports: [
    ChartsModule,
    RouterModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    DragDropModule,

    //Angular Material Modules
    MatDividerModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatMenuModule,
    MatListModule,
    MatSidenavModule,
    MatDialogModule,
    MatTableModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatNativeDateModule,
    MatInputModule,
    MatAutocompleteModule,
    MatBadgeModule,
    MatBottomSheetModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatStepperModule,
    MatExpansionModule,
    MatGridListModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatSelectModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatTabsModule,
    MatTooltipModule,
    MatTreeModule,
    MatSelectFilterModule,

    //Local Modules
    SharedModule,
    ServicesModule,
  ],
  providers: [{ provide: DateAdapter, useClass: DateFormat }]
})
export class ViewsModule { 
  constructor(private dateAdapter: DateAdapter<Date>) {
    dateAdapter.setLocale("en-in"); // DD/MM/YYYY
  }
}
