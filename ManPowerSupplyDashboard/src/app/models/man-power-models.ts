export interface CustomerModel {
    isDeleted: Boolean,
    lastUpdatedBy: string,
    lastUpdatedDate: Date,
    id: number,
    name: string,
    mobile: string,
    alternateMobile: string,
    address: string,
    balanceAmount: number
}

export interface CreateCustomerModel {
    name: string,
    mobile: string,
    alternateMobile: string,
    address: string
}

export interface SkillModel {
    isDeleted: Boolean,
    lastUpdatedBy: string,
    lastUpdatedDate: Date,
    id: number,
    name: string,
    code: string,
    description: string,
}

export interface CreateSkillModel {
    name: string,
    code: string,
    description?: string,
}

export interface EmployeeModel {
    id: number,
    name: string,
    mobile: string,
    alternateMobile: string,
    location: string,
    identityDetails: string,
    address: string,
    isVerified: Boolean,
    description: string,
    skillsAsPlainText: string,
    balanceAmount: number,
    skills: [
        {
            id: number,
            name: string,
            code: string
        }
    ],
    LastUpdatedDate: Date
}

export interface CreateEmployeeModel {
    name: string,
    mobile: string,
    alternateMobile: string,
    location: string,
    identityDetails: string,
    address: string,
    skills: [
        {
            id: number,
            name: string,
            code: string
        }
    ]
}

export interface AttendanceModel {
    id: number,
    date: Date,
    employeeId: number,
    employeeName: string,
    customerId: number,
    customerName: string,
    attendanceStatus: Boolean,
    nextDayCustomerId: number,
    nextDayCustomerName: string,
    customerPay: number,
    rent: number,
    customerTA: number,
    companyTA: number,
    employeePay: number,
    remarks: string,
    skill: string,
    skillCode: string,
    isDeleted: boolean,
    lastUpdatedBy: string
}

export interface CreateAttendanceModel {
    date: string | any ,
    employeeId: number,
    customerId: number,
    attendanceStatus: Boolean,
    nextDayCustomerId: number,
    customerPay: number,
    rent: number,
    customerTA: number,
    companyTA: number,
    employeePay: number,
    remarks: string
}

export interface CustomerReceiptModel {
    isDeleted: boolean,
    lastUpdatedBy: string,
    lastUpdatedDate: Date,
    id: number,
    customerId: number,
    customerName: string,
    date: Date,
    paidAmount: number,
    cashCollectedBy: string,
    remarks: string
}

export interface CreateCustomerReceiptModel {
    customerId: number,
    date: string,
    paidAmount: number,
    cashCollectedBy: string,
    remarks: string
}

export interface EmployeePaymentModel {
    isDeleted: Boolean,
    lastUpdatedBy: string,
    lastUpdatedDate: Date,
    id: number,
    date: Date,
    employeeId: number,
    employeeName: string,
    paymentTypeId: number,
    paymentTypeName: string,
    amount: number,
    remarks: string
}

export interface CreateEmployeePaymentModel {
    date: string,
    employeeId: number,
    paymentTypeId: number,
    amount: number,
    remarks: string
}

export class CustomerLedgerModel {
    openingBalance: number | undefined;
    particulars: [
        {
            date: Date;
            particular: string;
            customerPay: number;
            customerTA: number;
            rent: number;
            totalPay: number;
            received: number;
            amount: number; 
            balance: number;
            type: string;
        }
    ] | undefined;
    closingBalance: number | undefined
}

export class EmployeeLedgerModel {
    openingBalance: number | undefined;
    closingBalance: number | undefined;
    particulars: [
        {
            date: Date;
            particular: string;
            employeePay: number;
            payment: number;
            amount: number;
            balance: number;
            type: string;
        }
    ] | undefined
}

export interface AccountHeadModel {
    id: number,
    name: string,
    groupId: number,
    groupName: string,
    accountType: string,
    description: string
}

export interface CreateAccountHeadModel {
    name: string,
    accountGroupId: number,
    description: string
}

export class ReceiptAndPaymentModel {
    openingBalance: number;
    closingBalance: number;
    particulars: ReceiptAndPaymentParticulars[];
}

export interface ReceiptAndPaymentParticulars {
    id: number,
    date: Date,
    accountHeadId: number,
    accountHeadName: string,
    transactionType: string,
    amount: number,
    particular: string,
    description: string
}

export interface CreateReceiptAndPaymentModel {
    date: string,
    accountHeadId: number,
    transactionType: number,
    amount: number,
    particular: string,
    description: string
}

export class IncomeAndExpenditureModel {
    openingBalance: number;
    closingBalance: number;
    particulars: [
        {
            date: Date,
            accountHeadId: number,
            accountHeadName: string,
            particular: string,
            transactionType: string,
            receivedAmount: number,
            paidAmount: number,
            amount: number,
            balance: number
        }
    ];
}



