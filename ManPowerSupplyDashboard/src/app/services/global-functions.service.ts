import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormGroup, FormControl } from '@angular/forms';
import { DatePipe } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class GlobalFunctionsService {

  constructor(private _snackBar: MatSnackBar, public datepipe: DatePipe) { }

  openSnackBar(message: string): void {
    this._snackBar.open(message, 'End now', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'top',
    });
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

  convertToYYYMMDD(date: Date): string | null {
    return this.datepipe.transform(date, 'yyyy-MM-dd');
  }

  convertToDDMMYYYY(date: Date): string | null {
    return this.datepipe.transform(date, 'dd-MM-yyyy');
  }

  convertToYYYMMDDHHMMSS(date: Date): string | null {
    return this.datepipe.transform(date, 'yyyy-MM-ddThh:mm:ss');
  }
}
