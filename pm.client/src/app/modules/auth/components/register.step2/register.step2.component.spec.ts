import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RegisterStep2Component } from './register.step2.component';
import { AuthService, CountryItem, ProvinceItem, UserInfo } from '../../services/auth.service';
import { Observable, of } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


describe('RegisterStep2Component', () => {
  let component: RegisterStep2Component;
  let fixture: ComponentFixture<RegisterStep2Component>;
  let authServiceMock: AuthService;

  const mockAuthService = {
    getCountries(): Observable<CountryItem[]> {
      return of(
        [
          { id: '1', name: 'country_1' },
          { id: '2', name: 'country_2' },
        ]);
    },

    getProvinces(countryId: string): Observable<ProvinceItem[]> {
      return of([
        { id: '1', name: 'province_1' },
        { id: '2', name: 'province_2' }
      ]);
    },

    register(data: { email: string; password: string, provinceId: string }): Observable<UserInfo> {
      return of({ id: '1', email: data.email });
    }
  };

  beforeEach(async () => {

    await TestBed.configureTestingModule({
      declarations: [RegisterStep2Component],
      imports: [MatCardModule,
        MatButtonModule,
        MatFormFieldModule,
        MatInputModule,
        MatIconModule,
        MatCheckboxModule,
        MatSelectModule,
        MatSnackBarModule,
        BrowserModule,
        BrowserAnimationsModule],
      providers: [{ provide: AuthService, useValue: mockAuthService }]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterStep2Component);
    component = fixture.componentInstance;
    authServiceMock = TestBed.inject(AuthService);
  });

  afterEach(() => {
    
  });

  it('should ok on init', () => {

    // Act
    component.ngOnInit();

    // Assert
    expect(component.countries).not.toBeNull();
    expect(component.countries).not.toBe([]);
  });

  it('should validate data and submit', () => {
    // Arrange
    component.receiveData = { email: 'test@email.com', password: 'P1' };
    component.registerForm.get('country')?.setValue('country_id');
    component.registerForm.get('province')?.setValue('province_id');

    // Act
    component.onSubmit();

    // Assert
    expect(component.registerForm.valid).toBeTrue();
    expect(component.serverError).toBeNull();
  });

  const submitTestCases = [
    { countryId: null, provinceId: 'province_id' },
    { countryId: '', provinceId: 'province_id' },
    { countryId: 'country_id', provinceId: null },
    { countryId: 'country_id', provinceId: '' },
  ];

  submitTestCases.forEach(({ countryId, provinceId }) => {
    it('should not validate data and submit', () => {
      // Arrange
      component.receiveData = { email: 'test@email.com', password: 'P1' };
      component.registerForm.get('country')?.setValue(countryId);
      component.registerForm.get('province')?.setValue(provinceId);

      // Act
      component.onSubmit();

      //// Assert
      expect(component.registerForm.valid).toBeFalse();
    });
  });
});
