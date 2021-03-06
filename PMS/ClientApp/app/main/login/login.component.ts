import { NotificationService } from './../../core/services/notification.service';
import { AuthenService } from './../../core/services/authen.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, AbstractControl, FormBuilder, Validators } from '@angular/forms';
import { Router, NavigationStart } from '@angular/router';
@Component({
  selector: 'login',
  templateUrl: './login.html',
  styleUrls: ['./login.scss']
})
export class Login {

  public form: FormGroup;
  public email: AbstractControl;
  public password: AbstractControl;
  public submitted: boolean = false;
  public submittedGuest: boolean = false;

  loading = false;
  user: any = {};

  constructor(fb: FormBuilder, private router: Router, private authenService: AuthenService,
    private notificationService: NotificationService,) {
    this.form = fb.group({
      'email': ['', Validators.compose([Validators.required, Validators.minLength(4)])],
      'password': ['', Validators.compose([Validators.required, Validators.minLength(4)])]
    });
    this.email = this.form.controls['email'];
    this.password = this.form.controls['password'];

  }

  public onSubmit(values: Object): void {
    this.user.email = this.email.value;
    this.user.password = this.password.value;
    this.submitted = true;
    if (this.form.valid) {
      this.authenService.login(this.user.email, this.user.password).subscribe(data => {
        this.router.navigate(["/main/dashboard"]);
      }, error => {
        this.notificationService.printErrorMessage("Failed to login");
        this.submitted = false;
      });
    }
  }

  loginWithGuest() {
    console.log(this.user.email);
    this.user.email = "guest@eiu.edu.vn"
    this.user.password = "eiu@123";
    this.submittedGuest = true;
      this.authenService.login(this.user.email, this.user.password).subscribe(data => {
        this.router.navigate(["/main/dashboard"]);
      }
  )};
}
