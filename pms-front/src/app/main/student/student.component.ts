import { SystemConstants } from './../../core/common/system.constants';
import { Response } from '@angular/http';
import { NotificationService } from './../../core/services/notification.service';
import { DataService } from './../../core/services/data.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { HubConnection } from '@aspnet/signalr-client';

@Component({
  selector: 'app-student',
  templateUrl: './student.component.html',
  styleUrls: ['./student.component.css']
})
export class StudentComponent implements OnInit {

  @ViewChild('modalAddEdit') public modalAddEdit: ModalDirective;
  public students: any[];
  public student: any;
  public isClicked: boolean;
  hubConnection: HubConnection;
  constructor(private _dataService: DataService, private _notificationService: NotificationService) {
    this.isClicked = false;
  }

  ngOnInit() {
    this.loadData();

    this.hubConnection = new HubConnection(SystemConstants.BASE_URL + "/hub");
    this.hubConnection.on('Send', (data: any) => {
      this.loadData();
    });
    this.hubConnection.start()
      .then(() => {
        console.log('Hub connection started')
      })
      .catch(err => {
        console.log('Error while establishing connection')
      });

  }

  loadData() {
    this._dataService.get("/api/students/getall").subscribe((response: any) => {
      console.log(response);
      this.students = response;
    });
  }

  //Create method
  showAddModal() {
    this.student = {};
    this.modalAddEdit.show();
  }

  //Edit method
  showEditModal(id: any) {
    this.loadStudent(id);
    this.modalAddEdit.show();
  }

  //Get Role with Id
  loadStudent(id: any) {
    this._dataService.get('/api/students/getstudent/' + id)
      .subscribe((response: any) => {
        this.student = response;
        console.log(this.student);
      });
  }
  saveChange(valid: boolean) {
    if (valid) {
      this.isClicked = true;
      if (this.student.id == undefined) {
        this._dataService.post('/api/students/add', JSON.stringify(this.student))
          .subscribe((response: any) => {
            this.loadData();
            this.modalAddEdit.hide();
            this.hubConnection.invoke('Send', this.student);
            this._notificationService.printSuccessMessage("Add Success");
            this.isClicked = false;
          }, error => this._dataService.handleError(error));
      }
      else {
        this._dataService.put('/api/students/update/' + this.student.id, JSON.stringify(this.student))
          .subscribe((response: any) => {
            this.loadData();
            this.modalAddEdit.hide();
            this._notificationService.printSuccessMessage("Update Success");
            this.isClicked = false;
          }, error => this._dataService.handleError(error));
      }
    }
  }

  deleteStudent(id: any) {
    this._notificationService.printConfirmationDialog("Delete confirm", () => this.deleteConfirm(id));
  }

  deleteConfirm(id: any) {
    this._dataService.delete('/api/students/delete/' + id)
      .subscribe((response: Response) => {
        this._notificationService.printSuccessMessage("Delete Success");
        this.loadData();
      });
  }
}
