import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-register-step1',
  standalone: false,
  templateUrl: './register.step1.component.html',
  styleUrls: ['./register.step1.component.scss']
})
export class RegisterStep1Component {
  registerForm: FormGroup;
  hidePassword = true;
  hideConfirmPassword = true;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar) {
    this.registerForm = this.fb.group({
      login: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, /*this.validatePassword*/ Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d).+$/)]],
      confirmPassword: ['', [Validators.required, this.validateConfirmPassword]],
      terms: [false, [Validators.requiredTrue]],
    });
  }

  private validateConfirmPassword(control: AbstractControl): ValidationErrors | null {
    const password = control.parent?.get('password');
    const confirmPassword = control.parent?.get('confirmPassword');
    return password?.value == confirmPassword?.value ? null : { 'passwordMismatch': true };
  }

  ngOnInit() {
    const savedForm = sessionStorage.getItem('register.step1.form');
    if (savedForm) {
      this.registerForm.setValue(JSON.parse(savedForm));
    }
  }

  onSubmit() {
    if (this.registerForm.valid) {
      const email = this.registerForm.get('login')?.value;
      const password = this.registerForm.get('password')?.value;
      this.authService.validate({ email: email }).subscribe(
        {
          next: () => {
            this.registerForm.get('password')?.setValue('');
            this.registerForm.get('confirmPassword')?.setValue('');
            sessionStorage.setItem('register.step1.form', JSON.stringify(this.registerForm.value));

            this.router.navigate(['step2'],
              {
                state: {
                  email: email,
                  password: password,
                }
              });
          },
          error: (err: any) => {
            const error = err.error;
            // Duplicate email error
            if (error && error.status === 409 && error.title === 'User.DuplicateEmail') {
              this.registerForm.get('login')?.setErrors({ 'duplicateEmail': true });
            }
            this.snackBar.open(`Validation Failed. ${err.error.detail}`, 'Close', {
              duration: 3000,
              panelClass: ['error-snackbar'],
            });
          }
        }
      )
    }

  }
}

