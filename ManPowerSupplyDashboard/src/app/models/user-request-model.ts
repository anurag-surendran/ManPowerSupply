import { OrganizationModel } from "../authentication/models/user-model";
import { RolesModel } from "./user-response-model";

export class UserRequestModel{
    firstName: string;
    lastName: string;
    userName: string;
    mobileNumber: string;
    eMail: string;
    password: string;
    organizations: OrganizationModel[];
    roles: RolesModel[]
  }