export class UserResponseModel {
    id: number;
    firstName: string;
    lastName: string;
    userName: string;
    mobileNumber: string;
    eMail: string;
    accountStatus: string;
    organizations: OrganizationModel[];
    roles: RolesModel[]
}

export class OrganizationModel {
    id: number;
    name: string;
    address: string
}

export class RolesModel {
    id: number;
    name: string
}