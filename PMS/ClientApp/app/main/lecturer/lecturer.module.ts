import { PaginationModule } from './../../core/pagination/pagination.module';
import { LecturerComponent } from './lecturer.component';
import { NotificationService } from './../../core/services/notification.service';
import { DataService } from './../../core/services/data.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';
import { routing } from './lecturer.routing';
import { NgaModule } from '../../theme/nga.module';
@NgModule({
  imports: [
    NgaModule,
    ModalModule.forRoot(),
    CommonModule,
    PaginationModule,
    FormsModule,
    routing
  ],
  declarations: [
    LecturerComponent
  ],
  providers: [DataService, NotificationService]
})
export class LecturerModule { }
