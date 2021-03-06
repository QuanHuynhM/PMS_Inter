import { Observable } from 'rxjs/Observable';
import { UtilityService } from './utility.service';
import { NotificationService } from './notification.service';
import { SystemConstants } from './../common/system.constants';
import { AuthenService } from './authen.service';
import { Http, Response, Headers } from '@angular/http';
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';

@Injectable()
export class DataService {
  private headers : Headers;
  constructor(private _http: Http, private _router : Router, 
    private _authService : AuthenService,
    private _notificationService : NotificationService,
    private _utilityService : UtilityService) {
      this.headers = new Headers();
      this.headers.append('Content-Type', 'application/json');
     }

  get(url: string) {
    this.headers.delete("Authorization");
    this.headers.append("Authorization", "Bearer " + this._authService.getLoggedInUser().access_token);
    return this._http.get(SystemConstants.BASE_URL + url, { headers: this.headers })
      .map(this.extractData);
  }

  getGithub(url: string) {
    return this._http.get(SystemConstants.GITHUB_API_URL + url)
      .map(this.extractData);
  }

  post(url: string, data?: any) {
    this.headers.delete("Authorization");
    this.headers.append("Authorization", "Bearer " + this._authService.getLoggedInUser().access_token);
    return this._http.post(SystemConstants.BASE_URL + url, data, { headers: this.headers })
      .map(this.extractData);
  }

  put(url: string, data?: any) {
    this.headers.delete("Authorization");
    this.headers.append("Authorization", "Bearer " + this._authService.getLoggedInUser().access_token);
    return this._http.put(SystemConstants.BASE_URL + url, data, { headers: this.headers })
      .map(this.extractData);
  }

  delete(url: string) {
    this.headers.delete("Authorization");
    this.headers.append("Authorization", "Bearer " + this._authService.getLoggedInUser().access_token);
    return this._http.delete(SystemConstants.BASE_URL + url, { headers: this.headers });
  }

  upload(url: string, file){
    this.headers.delete("Authorization");
    this.headers.append("Authorization", "Bearer " + this._authService.getLoggedInUser().access_token);
    var formData= new FormData();
    formData.append('file',file);
    console.log(formData.get("file"))
    return this._http.post(SystemConstants.BASE_URL + url, formData)
      .map(res=>res.json());
  }

  private extractData(res: Response) {
    let body = res.json();
    return body || {};
  }

  public handleError(error : any) {
    if(error.status == 401) {
      localStorage.removeItem(SystemConstants.CURRENT_USER);
      this._notificationService.printErrorMessage("Error");
      this._utilityService.navigateToLogin();
    } else {
      let errMsg = (error.message) ? error.message : error.status ? `${error.statusText}` : "Error system";
      this._notificationService.printErrorMessage(errMsg);
      return Observable.throw(errMsg);
    }
  }
}
