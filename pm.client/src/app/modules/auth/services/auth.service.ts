import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface CountryItem {
  id: string;
  name: string;
}

export interface ProvinceItem {
  id: string;
  name: string;
}

export interface UserInfo {
  id: string;
  email: string;
}

export interface UserState {
  email: string;
  password: string;
}


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `http://localhost:5242/api/v1`;

  constructor(private http: HttpClient) { }

  validate(data: { email: string; }): Observable<any> {
    return this.http.post<UserInfo>(`${this.apiUrl}/auth/validate`, data);
  }

  register(data: { email: string; password: string, provinceId: string }): Observable<UserInfo> {
    return this.http.post<UserInfo>(`${this.apiUrl}/auth/register`, data);
  }

  getCountries(): Observable<CountryItem[]> {
    return this.http.get<CountryItem[]>(`${this.apiUrl}/country/list`);
  }

  getProvinces(countryId: string): Observable<ProvinceItem[]> {
    let params = new HttpParams()
      .set('countryId', countryId);

    return this.http.get<CountryItem[]>(`${this.apiUrl}/country/province/list`, { params });
  }
}
