import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { User } from 'src/app/authentication/models/user-model';
import { AuthenticationService } from 'src/app/authentication/services/authentication.service';
import { MessagingService } from 'src/app/services/messaging.service';
import { AddUserComponent } from 'src/app/views/add-user/add-user.component';
import { UpdatePasswordComponent } from 'src/app/views/update-password/update-password.component';
import { UserProfileComponent } from 'src/app/views/user-profile/user-profile.component';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  routeDatas: string[] | any;
  routeData: string;
  user: User;

  constructor(private _messaging: MessagingService,
    private authenticationService: AuthenticationService,
    public dialog: MatDialog) {
    this.authenticationService.user.subscribe(x => this.user = x);
  }

  @Output() toggleSidebarEvent: EventEmitter<any> = new EventEmitter();

  ngOnInit(): void {
    this._messaging.headerRouteMessage.subscribe(routeData => {
      this.routeData = routeData;
      this.routeDatas = routeData.split('/');
    });
  }

  toggleSideBar() {
    this.toggleSidebarEvent.emit();
  }

  logout() {
    this.authenticationService.logout();
  }

  profile() {
    let routeData = this.routeData;
    const addEditDialog = this.dialog.open(UserProfileComponent, {
      width: '600px',
      panelClass: 'myapp-no-padding-dialog',
      height: '450px',
      data: this.user,
      disableClose: true
    });

    addEditDialog.afterClosed().subscribe(() => {
      this.routeDatas = routeData.split('/');
    });
  }

  addUser() {
    let routeData = this.routeData;
    const addEditDialog = this.dialog.open(AddUserComponent, {
      width: '600px',
      panelClass: 'myapp-no-padding-dialog',
      height: '450px',
      data: this.user,
      disableClose: true
    });

    addEditDialog.afterClosed().subscribe(() => {
      this.routeDatas = routeData.split('/');
    });
  }

  updatePassword(){
    let routeData = this.routeData;
    const addEditDialog = this.dialog.open(UpdatePasswordComponent, {
      width: '300px',
      panelClass: 'myapp-no-padding-dialog',
      height: '330px',
      data: this.user,
      disableClose: true
    });

    addEditDialog.afterClosed().subscribe(() => {
      this.routeDatas = routeData.split('/');
    });
  }

  isSuperAdmin(): Boolean {
    if (this.user == null || this.user == undefined)
      return false;

    var result = false;
    if (this.user.roles.filter(x => x.name.toLowerCase() == "SuperAdmin".toLowerCase()).length > 0)
      result = true;
    return result;
  }

}
