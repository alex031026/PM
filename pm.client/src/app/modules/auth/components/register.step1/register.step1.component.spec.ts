import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RegisterStep1Component } from './register.step1.component';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';

describe('RegisterStep1Component', () => {
  let component: RegisterStep1Component;
  let fixture: ComponentFixture<RegisterStep1Component>;
  let routerSpy: jasmine.SpyObj<Router>;
  let authServiceMock: AuthService;

  const mockAuthService = {
    validate(data: { email: string; }): Observable<any> {
      return of({});
    }
  };

  beforeEach(async () => {
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [RegisterStep1Component],
      imports: [MatCardModule,
        MatButtonModule,
        MatFormFieldModule,
        MatInputModule,
        MatIconModule,
        MatCheckboxModule,
        MatSelectModule,
        MatSnackBarModule],
      providers: [{ provide: AuthService, useValue: mockAuthService }, { provide: Router, useValue: routerSpy }]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterStep1Component);
    component = fixture.componentInstance;
    authServiceMock = TestBed.inject(AuthService);
  });

  afterEach(() => {
    
  });

  it('should validate with valid data and navigate to step2', () => {
    // Arrange
    component.registerForm.get('login')?.setValue('test@email.com');
    component.registerForm.get('password')?.setValue('A1');
    component.registerForm.get('confirmPassword')?.setValue('A1');
    component.registerForm.get('terms')?.setValue(true);

    // Act
    component.onSubmit();

    // Assert
    expect(routerSpy.navigate).toHaveBeenCalledWith(['step2'], { state: { email: 'test@email.com', password: 'A1' } });
  });

  const submitTestCases = [
    { login: null, password: 'D1', confirmPassword: 'D1', terms: true },
    { login: '', password: 'D1', confirmPassword: 'D1', terms: true },
    { login: 'email.com', password: 'D1', confirmPassword: 'D1', terms: true },
    { login: 'test@email.com', password: null, confirmPassword: 'D1', terms: true },
    { login: 'test@email.com', password: '', confirmPassword: 'D1', terms: true },
    { login: 'test@email.com', password: 'D', confirmPassword: 'D1', terms: true },
    { login: 'test@email.com', password: 'D1', confirmPassword: 'D2', terms: true },
    { login: 'test@email.com', password: 'D1', confirmPassword: null, terms: true },
    { login: 'test@email.com', password: 'D1', confirmPassword: 'D1', terms: false },
  ];

  submitTestCases.forEach(({ login, password, confirmPassword, terms }) => {
    it(`should not validate and navigate to step2`, () => {
          
      // Arrange
      component.registerForm.get('login')?.setValue(login);
      component.registerForm.get('password')?.setValue(password);
      component.registerForm.get('confirmPassword')?.setValue(confirmPassword);
      component.registerForm.get('terms')?.setValue(terms);

      // Act
      component.onSubmit();

      // Assert
      expect(routerSpy.navigate).not.toHaveBeenCalledWith(['step2'], { state: { email: login, password: password } });
    });
  });
});
