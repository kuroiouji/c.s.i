import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from "@angular/forms";

import { CtmApiService, CtmTranslateService, CtmMessageService, MESSAGE_DIALOG_TYPE, USER_LOCAL_STORAGE } from "../../../../_common";
import { CtmControlManagement } from "../../../../_common";
import { Router } from '@angular/router';
import { ScreenParam } from '../user.screen-param';
import { permission } from "../user.permission";

@Component({
  selector: 'app-user-group-detail',
  templateUrl: './user-group-detail.component.html',
  styleUrls: ['./user-group-detail.component.scss']
})
export class UserGroupDetailComponent implements OnInit {
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
        "GroupID": [{ value: this.param.getKey(), disabled: false }],
        "NameEN": [{ value: null, disabled: false }, Validators.compose([Validators.required])],
        "Description": [{ value: null, disabled: false }],
        "FlagActive": [{ value: null, disabled: false }],
        "CrashDiscount": [{ value: null, disabled: false }, Validators.compose([Validators.required])],
        "CreditDiscount": [{ value: null, disabled: false }, Validators.compose([Validators.required])],
      })
    );
   }

  ngOnInit() {
    if(this.param.getKey() != undefined){
      this.loadDataUG();
      this.loadDataUIG();
    }
    console.log(permission);
    
  }
  private permission = permission;
  private listP:Array<object> = [];
  private loadDataUG(){
    let data = this.ctrlMgr.data;
    this.api.callApiController("api/SCS011/GetUserGroup", {
      type: "POST",
      anonymous: true,
      data: data
    }).then((res) => {
      if (res != undefined) {
        this.listP = res["Permissions"];
        console.log(this.listP);
        this.ctrlMgr.data = res["Group"];
      }
    });
  }

  private listU:Array<object> = [];
  private loadDataUIG(){
    let data = this.ctrlMgr.data;
    this.api.callApiController("api/SCS011/GetUserInGroup", {
      type: "POST",
      anonymous: true,
      data: data
    }).then((res) => {
      if (res != undefined) {
        console.log(res);
        this.listU = res["Rows"];
      }
    });
  }

  private checkNull(data){
    if(data.GroupNameLC != null){
      return true;
    }else{
      return false;
    }
  }

  private goBack(){
    this.router.navigate(['/s/u/gs']);
  }

}
