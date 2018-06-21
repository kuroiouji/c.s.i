import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from "@angular/forms";

import { CtmApiService, CtmTranslateService, CtmMessageService, MESSAGE_DIALOG_TYPE, USER_LOCAL_STORAGE } from "../../../../_common";
import { CtmControlManagement } from "../../../../_common";

let moment = require('moment');
@Component({
  selector: 'app-user-serch',
  templateUrl: './user-serch.component.html',
  styleUrls: ['./user-serch.component.scss']
})
export class UserSerchComponent implements OnInit {
  private ctrlMgr: CtmControlManagement;
  constructor(
    private translate: CtmTranslateService,
    private message: CtmMessageService,
    private fb: FormBuilder,
    private api: CtmApiService) { 
      this.ctrlMgr = new CtmControlManagement(
        fb.group({
            "UserName": [{ value: null, disabled: false }],
            "GroupID": [{ value: null, disabled: false }],
            "FlagActive": [{ value: true, disabled: false }],
        })
      );
    }

  ngOnInit() {
  }

  private list:Array<object> = [];
  private onSearch(){
    let data = this.ctrlMgr.data;
    data["page"] =  1;
    data["paging"]  = 50;
    this.api.callApiController("api/SCS020/GetUserList", {
      type: "POST",
      anonymous: true,
      data: data
    }).then((res) => {
      if (res != undefined) {
        console.log(res);
        this.list = res["Rows"];
      }
    });
  }

  private onClear(){
    this.list = [];
  }

  private dateText(date){
    return moment(date).format("DD/MM/YYYY");
  }
}
