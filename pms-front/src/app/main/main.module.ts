import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main.component';
import { MainRoutes } from './main.routes';
import { RouterModule, Routes } from '@angular/router';


import { UserModule } from './user/user.module';
import { HomeModule } from './home/home.module';
import { UtilityService } from './../core/services/utility.service';
import { AuthenService } from './../core/services/authen.service';

@NgModule({
  imports: [
    CommonModule,
    HomeModule,    
    UserModule,
    RouterModule.forChild(MainRoutes)
  ],
  declarations: [MainComponent],
  providers: [UtilityService, AuthenService]
})
export class MainModule { }