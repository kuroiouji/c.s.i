import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { FormBuilder, Validators } from "@angular/forms";

import { CtmApiService, CtmTranslateService, CtmMessageService, CtmControlManagement,
          MESSAGE_DIALOG_TYPE, USER_LOCAL_STORAGE } from "../../../_common";
import { CONSTANT } from "../../../_custom";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, AfterViewInit {
  private ctrlMgr: CtmControlManagement;
  private returnUrl: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,

    private translate: CtmTranslateService,
    private message: CtmMessageService,
    private api: CtmApiService
  ) { 
    this.ctrlMgr = new CtmControlManagement(
      fb.group({
          "Username": [{ value: null, disabled: false }, Validators.compose([Validators.required])],
          "Password": [{ value: null, disabled: false }]
      })
    );
  }

  ngOnInit() {
    this.translate.use(CONSTANT.DEFAULT_LANGUAGE);

    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams["returnUrl"] || "/";
  }
  ngAfterViewInit() {
    this.api.callApiController("api/CMS010/Initial", {
        type: "POST",
        showLoading: false,
        showError: false,
        anonymous: true
    }).then(() => {
        //this.apiLoader.hide(500);
        //this.userName.setFocus();
    });
  }

  private onLogin() {
    if (this.ctrlMgr.validate((field, key) => {
        if (field == "password"
            && (key == "minlength" || key == "maxlength")) {
            return this.translate.instant("CLE002", "MESSAGE");
        }
        return null;
      })) {
      let data = this.ctrlMgr.data;
      this.api.callApiController("api/CMS010/Login", {
        type: "POST",
        anonymous: true,
        data: data
      }).then((res) => {
        if (res != undefined) {
          if (res["IsPasswordExpired"] == true) {
              
          }
          else {
  
              let loc = {
                  userName: res["UserName"],
                  displayName: res["DisplayName"],
                  groupID:  res["GroupID"],
                  timeout: res["Timeout"],
                  date: new Date()
              };
  
              localStorage.removeItem(USER_LOCAL_STORAGE);
              localStorage.setItem(USER_LOCAL_STORAGE, JSON.stringify(loc));
  
              localStorage.removeItem(USER_LOCAL_STORAGE + ".TOKEN");
              localStorage.setItem(USER_LOCAL_STORAGE + ".TOKEN", res["Token"]);
  
              localStorage.removeItem(USER_LOCAL_STORAGE + ".RTOKEN");
              localStorage.setItem(USER_LOCAL_STORAGE + ".RTOKEN", res["RefreshToken"]);

              this.router.navigate([this.returnUrl]);
          }
        }
      });
    }
  }
}
