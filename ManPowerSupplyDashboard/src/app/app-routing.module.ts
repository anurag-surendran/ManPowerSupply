import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { CustomersComponent } from './views/customers/customers.component';
import { DefaultDashboardComponent } from './views/default-dashboard/default-dashboard.component';
import { DefaultLayoutComponent } from './layouts/default-layout/default-layout.component';
import { SkillsComponent } from './views/skills/skills.component';
import { EmployeesComponent } from './views/employees/employees.component';
import { AttendanceComponent } from './views/attendance/attendance.component';
import { CustomerReceiptsComponent } from './views/receipts-payments/customer-receipts/customer-receipts.component';
import { EmployeePaymentsComponent } from './views/receipts-payments/employee-payments/employee-payments.component';
import { ReceiptsComponent } from './views/receipts-payments/receipts/receipts.component';
import { PaymentsComponent } from './views/receipts-payments/payments/payments.component';
import { CustomerLedgerComponent } from './views/reports/customer-ledger/customer-ledger.component';
import { EmployeeLedgerComponent } from './views/reports/employee-ledger/employee-ledger.component';
import { IncomeAndExpenditureReportComponent } from './views/reports/income-and-expenditure-report/income-and-expenditure-report.component';
import { AuthGuard } from './authentication/helper/auth.guard';




const routes: Routes = [
  {
    path:'login',
    component:LoginComponent
  },
  {
    path:'',
    component:DefaultLayoutComponent,
    canActivate: [AuthGuard],
    children:[      
      {
        path: 'dashboard',
        component: DefaultDashboardComponent
      },
      {
         path:'master',
         children:[
           {
             path:'customer',
             children:[
               {
                  path:'list',
                  component:CustomersComponent
               },
               {
                path: '',
                redirectTo: 'list',
                pathMatch: 'full'
               }
             ]
           },
           {
             path:'skill',
             children:[
               {
                  path:'list',
                  component:SkillsComponent
               },
               {
                path: '',
                redirectTo: 'list',
                pathMatch: 'full'
               }
             ]
           },
           {
             path:'employee',
             children:[
               {
                  path:'list',
                  component:EmployeesComponent
               },
               {
                path: '',
                redirectTo: 'list',
                pathMatch: 'full'
               }
             ]
           },
           {
             path:'attendance',
             children:[
               {
                  path:'list',
                  component:AttendanceComponent
               },
               {
                path: '',
                redirectTo: 'list',
                pathMatch: 'full'
               }
             ]
           },           
           {
            path: '',
            redirectTo: 'customer',
            pathMatch: 'full'
           }
         ] 
      },
      {
        path:'receipt-payment',
        children:[
          {
             path:'customer',
             component:CustomerReceiptsComponent
          },
          {
             path:'employee',
             component:EmployeePaymentsComponent
          },
          {
             path:'receipt',
             component:ReceiptsComponent
          },
          {
             path:'payment',
             component:PaymentsComponent
          },
          {
           path: '',
           redirectTo: 'customer',
           pathMatch: 'full'
          }
        ]
      },
      {
        path:'reports',
        children:[
          {
            path:'customer',
            component:CustomerLedgerComponent
          },
          {
            path:'employee',
            component:EmployeeLedgerComponent
          },
          {
            path:'income-expenditure',
            component:IncomeAndExpenditureReportComponent
          },
          {
            path:'',
            redirectTo:'customer',
            pathMatch:'full'
          }
        ]
      },
      {
        path: '',
        redirectTo: "dashboard",
        pathMatch: 'full'
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
