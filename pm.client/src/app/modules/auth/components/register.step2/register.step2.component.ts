import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService, CountryItem, ProvinceItem, UserState } from '../../services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';


@Component({
  selector: 'app-register-step2',
  standalone: false,
  templateUrl: './register.step2.component.html',
  styleUrls: ['./register.step2.component.scss']
})
export class RegisterStep2Component {
  registerForm: FormGroup;
  receiveData: UserState;
  countries: CountryItem[] = [];
  provinces: ProvinceItem[] = [];
  serverError: any = null;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private snackBar: MatSnackBar) {
    this.receiveData = history.state;
    this.registerForm = this.fb.group({
      country: ['', Validators.required],
      province: ['', [Validators.required]],
    });
  }

  ngOnInit() {
    this.authService.getCountries().subscribe((list: CountryItem[]) => {
      this.countries = list;
    });

    this.registerForm.get('country')?.valueChanges.subscribe((id: string) => {
      this.authService.getProvinces(id).subscribe((list: ProvinceItem[]) => {
        this.provinces = list;
      });
    });
  }

  onSubmit() {
    this.serverError = null;
    if (this.registerForm.valid) {
      this.authService.register({
        email: this.receiveData.email,
        password: this.receiveData.password,
        provinceId: this.registerForm.get('province')?.value
      }).subscribe({
        next: () => {
          this.snackBar.open('Registration Successful!', 'Close', {
            duration: 5000,
            panelClass: ['success-snackbar'],
          });
        },
        error: (err: any) => {
          this.serverError = err.error;

          this.snackBar.open(`Registration Failed. ${err.error.detail}`, 'Close', {
            duration: 5000,
            panelClass: ['error-snackbar'],
          });
        }
      });
    }
  }
}
