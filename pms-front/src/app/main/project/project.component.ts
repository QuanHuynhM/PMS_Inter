import { Response } from '@angular/http';
import { NotificationService } from './../../core/services/notification.service';
import { DataService } from './../../core/services/data.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent implements OnInit {

  @ViewChild('modalAddEdit') public modalAddEdit: ModalDirective;
  public projects: any[];
  public project: any;
  public isClicked: boolean;
  constructor(private _dataService: DataService, private _notificationService: NotificationService) {
    this.isClicked = false;
  }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this._dataService.get("/api/projects/getall").subscribe((response: any) => {
      console.log(response);
      this.projects = response;
    });
  }

  //Create method
  showAddModal() {
    this.project = {};
    this.modalAddEdit.show();
  }

  //Edit method
  showEditModal(id: any) {
    this.loadproject(id);
    this.modalAddEdit.show();
  }

  //Get Role with Id
  loadproject(id: any) {
    this._dataService.get('/api/projects/getproject/' + id)
      .subscribe((response: any) => {
        this.project = response;
        console.log(this.project);
      });
  }

  saveChange(valid: boolean) {
    if (valid) {
      this.isClicked = true;
      if (this.project.projectId == undefined) {
        this._dataService.post('/api/projects/add', JSON.stringify(this.project))
          .subscribe((response: any) => {
            this.loadData();
            this.modalAddEdit.hide();
            this._notificationService.printSuccessMessage("Add Success");
            this.isClicked = false;
          }, error => this._dataService.handleError(error));
      }
      else {
        this._dataService.put('/api/projects/update/' + this.project.projectId, JSON.stringify(this.project))
          .subscribe((response: any) => {
            this.loadData();
            this.modalAddEdit.hide();
            this._notificationService.printSuccessMessage("Update Success");
            this.isClicked = false;
          }, error => this._dataService.handleError(error));
      }
    }
  }

  deleteproject(id: any) {
    this._notificationService.printConfirmationDialog("Delete confirm", () => this.deleteConfirm(id));
  }

  deleteConfirm(id: any) {
    this._dataService.delete('/api/projects/delete/' + id)
      .subscribe((response: Response) => {
        this._notificationService.printSuccessMessage("Delete Success");
        this.loadData();
      });
  }
}