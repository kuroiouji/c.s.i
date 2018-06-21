import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from "@angular/forms";

import { CtmApiService, CtmTranslateService, CtmMessageService, MESSAGE_DIALOG_TYPE, USER_LOCAL_STORAGE } from "../_common";
import { CtmControlManagement } from "../_common";

@Component({
  selector: 'app-screens',
  templateUrl: './screens.component.html',
  styleUrls: ['./screens.component.scss']
})
export class ScreensComponent {
  private ctrlMgr: CtmControlManagement;
  private show:boolean = false;

  constructor(
    private translate: CtmTranslateService,
    private message: CtmMessageService,
    private fb: FormBuilder,
    private api: CtmApiService
  ) { 
    this.ctrlMgr = new CtmControlManagement(
      fb.group({
          "Value1": [{ value: null, disabled: false }, Validators.compose([Validators.required])],
          "Value2": [{ value: null, disabled: false }],
          "Value3": [{ value: "333", disabled: true }, Validators.compose([Validators.required])],
          "Value21": [{ value: 6, disabled: false }, Validators.compose([Validators.required])],
          "Value22": [{ value: null, disabled: false }, Validators.compose([Validators.required])],
          "Value31": [{ value: null, disabled: false }, Validators.compose([Validators.required])],
          "Value41": [{ value: null, disabled: false }],
          "Value42": [{ value: null, disabled: false }],
      })
    );
  }

  public initScreen() {
  }

  private onOpenError() {
    this.message.openMessageDialog("Error", MESSAGE_DIALOG_TYPE.ERROR);
  }
  private onOpenInformation() {
    this.message.openMessageDialog("Information", MESSAGE_DIALOG_TYPE.INFORMATION);
  }
  private onOpenQuestion() {
    this.message.openMessageDialog("Question", MESSAGE_DIALOG_TYPE.QUESTION);
  }
  private onOpenQuestionOk() {
    this.message.openMessageDialog("Question (OK)", MESSAGE_DIALOG_TYPE.QUESTION_OK);
  }
  private onOpenQuestionCancel() {
    this.message.openMessageDialog("Question with cancel", MESSAGE_DIALOG_TYPE.QUESTION_WITH_CANCEL);
  }
  private onOpenWarning() {
    this.message.openMessageDialog("Warning", MESSAGE_DIALOG_TYPE.WARNING);
  }
  private onOpenWarningQuestion() {
    this.message.openMessageDialog("Warning with question", MESSAGE_DIALOG_TYPE.WARNING_QUESTION);
  }

  private onSubmit() {
    console.log("Submit");
    console.log(this.ctrlMgr.validate());
  }
}
