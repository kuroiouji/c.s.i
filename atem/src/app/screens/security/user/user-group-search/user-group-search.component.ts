import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from "@angular/forms";

import { CtmApiService, CtmTranslateService, CtmMessageService, MESSAGE_DIALOG_TYPE, USER_LOCAL_STORAGE } from "../../../../_common";
import { CtmControlManagement } from "../../../../_common";
import { Router } from '@angular/router';
import { ScreenParam } from '../user.screen-param';

@Component({
  selector: 'app-user-group-search',
  templateUrl: './user-group-search.component.html',
  styleUrls: ['./user-group-search.component.scss']
})
export class UserGroupSearchComponent implements OnInit {
  private ctrlMgr: CtmControlManagement;

  constructor(
    private translate: CtmTranslateService,
    private message: CtmMessageService,
    private fb: FormBuilder,
    private api: CtmApiService,
    private router:Router,
    private param:ScreenParam
  ) {
    this.ctrlMgr = new CtmControlManagement(
      fb.group({
        "GroupName": [{ value: null, disabled: false }],
        "Description": [{ value: null, disabled: false }],
        "UserName": [{ value: null, disabled: false }],
        "FlagActive": [{ value: null, disabled: false }],
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
    this.api.callApiController("api/SCS010/GetUserGroupList", {
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
    this.ctrlMgr.clear({});
    this.list = [];
  }

  private nextPage(GroupID){
    this.param.setKey(GroupID);
    this.router.navigate(['/s/u/gd']);
  }
  
}
