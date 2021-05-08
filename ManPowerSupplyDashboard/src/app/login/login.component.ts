import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { OrganizationModel, User } from '../authentication/models/user-model';
import { AuthenticationService } from '../authentication/services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup | any;
  returnUrl: string;

  constructor(private formBuilder: FormBuilder,
    private _snackBar: MatSnackBar,
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService) {
    if (this.authenticationService.userValue) {
      this.router.navigate(['/']);
    }
  }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      userName: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });

    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  get loginControls() { return this.loginForm.controls; }

  Login() {
    if (this.loginForm.valid) {
      let userName: string = this.loginControls.userName.value;
      let password: string = this.loginControls.password.value;

      this.authenticationService.login(userName, password)
        .pipe(first())
        .subscribe({
          next: (user:User) => {
            let organization:OrganizationModel={
              organizationId : user.organizations[0].id
            }
            this.authenticationService.setOrganization(organization,user.token).subscribe(()=>{
              this.router.navigate([this.returnUrl]);
            })
          },
          error: error => {
            this._snackBar.open(error, 'End now', {
              duration: 1000,
              horizontalPosition: 'center',
              verticalPosition: 'top',
            });
            this.loginForm.controls['userName'].setValue('');
            this.loginForm.controls['password'].setValue('');
          }
        });

      // if (userName.toLocaleLowerCase() != 'sreerag' || password != 'sreerag') {


      //   this.loginForm.controls['userName'].setValue('');
      //   this.loginForm.controls['password'].setValue('');
      // }
      // else {
      //   this.route.navigate(['dashboard'])
      // }

    } else {
      this.validateAllFormFields(this.loginForm);
    }
  }

  validateAllFormFields(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field);
      if (control instanceof FormControl) {
        control.markAsTouched({ onlySelf: true });
      } else if (control instanceof FormGroup) {
        this.validateAllFormFields(control);
      }
    });
  }

}
