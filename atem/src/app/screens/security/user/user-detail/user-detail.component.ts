import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from "@angular/forms";

import { CtmApiService, CtmTranslateService, CtmMessageService, MESSAGE_DIALOG_TYPE, USER_LOCAL_STORAGE } from "../../../../_common";
import { CtmControlManagement } from "../../../../_common";
@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.scss']
})
export class UserDetailComponent implements OnInit {
  private ctrlMgr: CtmControlManagement;

  constructor(
    private translate: CtmTranslateService,
    private message: CtmMessageService,
    private fb: FormBuilder,
    private api: CtmApiService
  ) {
    this.ctrlMgr = new CtmControlManagement(
      fb.group({
          "UserName": [{ value: null, disabled: false }, Validators.compose([Validators.required])],
          "PassWord": [{ value: null, disabled: false }, Validators.compose([Validators.required])],
          "GroupID": [{ value: null, disabled: false }, Validators.compose([Validators.required])],
          "FlagActive": [{ value: null, disabled: false }, Validators.compose([Validators.required])],
          "FirstName": [{ value: null, disabled: false }, Validators.compose([Validators.required])],
          "LastName": [{ value: null, disabled: false }, Validators.compose([Validators.required])],
          "Gender": [{ value: null, disabled: false }],
          "NickName": [{ value: null, disabled: false }],
          "CitizenID": [{ value: null, disabled: false }],
          "BirthDate": [{ value: null, disabled: false }],
          "Address": [{ value: null, disabled: false }],
          "TelNo": [{ value: null, disabled: false }],
          "Remark": [{ value: null, disabled: false }],
      })
    );
   }

  ngOnInit() {
  }

  private onSubmit() {
    this.ctrlMgr.validate();
  }

}
