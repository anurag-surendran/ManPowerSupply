export class User{
  id: number;
  firstName: string;
  lastName: string;
  userName: string;
  mobileNumber: string;
  eMail: string;
  accountStatus: string;
  organizations: [
    {
      id: number;
      name: string;
      address: string
    }
  ];
  roles: [
    {
      id: number,
      name: string
    }
  ]
  token: string
}

export class OrganizationModel{
  organizationId:number;
}