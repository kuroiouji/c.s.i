import { Component, Input, ViewChild, ElementRef } from '@angular/core';

import { CtmTranslateService, GlobalState } from "../../services";
import { MESSAGE_GLOBAL_KEY, MESSAGE_DIALOG_TYPE, MESSAGE_DIALOG_RETURN_TYPE } from "../../common.constant";
import { CtmDialogComponent } from "../dialog/dialog.component";

@Component({
  selector: 'ctm-message',
  templateUrl: './message.component.html'
})
export class CtmMessageComponent {
  @ViewChild(CtmDialogComponent) dialogCtrl: CtmDialogComponent;
  @ViewChild("btnOK") btnOK: ElementRef;
  @ViewChild("btnClose") btnClose: ElementRef;
  @ViewChild("btnYes") btnYes: ElementRef;
  @ViewChild("btnNo") btnNo: ElementRef;
  @ViewChild("btnCancel") btnCancel: ElementRef;
  
  private msgContent: string;
  private caller: Function;
  private msgType: MESSAGE_DIALOG_TYPE;
  private returnType: MESSAGE_DIALOG_RETURN_TYPE;

  private dialogCss: string;
  private showErrorIcon: boolean = true;
  private showInformationIcon: boolean = false;
  private showQuestionIcon: boolean = false;
  private showWarningIcon: boolean = false;

  private defaultOKButton: boolean = false;
  private defaultCloseButton: boolean = false;
  private defaultYesButton: boolean = false;
  private defaultNoButton: boolean = false;
  private defaultCancelButton: boolean = false;
  
  constructor(
    private global: GlobalState,
    private translate: CtmTranslateService
  ) { 
    this.global.subscribe(MESSAGE_GLOBAL_KEY, (data) => {
      this.msgContent = data.message;
      this.msgType = data.type;
      this.caller = data.caller;

      this.noCaption = null;
      if (data.option != undefined) {
          if (data.option.okCaption != undefined) {
              this.okCaption = data.option.okCaption;
          }
          if (data.option.closeCaption != undefined) {
              this.closeCaption = data.option.closeCaption;
          }
          if (data.option.yesCaption != undefined) {
              this.yesCaption = data.option.yesCaption;
          }
          if (data.option.noCaption != undefined) {
              this.noCaption = data.option.noCaption;
          }
          if (data.option.cancelCaption != undefined) {
              this.cancelCaption = data.option.cancelCaption;
          }
      }

      let css = "msg-dialog";
      if (this.msgType == MESSAGE_DIALOG_TYPE.ERROR) {
        this.showErrorIcon = true;
        this.showInformationIcon = false;
        this.showQuestionIcon = false;
        this.showWarningIcon = false;

        css += " error";
      }
      else if (this.msgType == MESSAGE_DIALOG_TYPE.INFORMATION) {
        this.showErrorIcon = false;
        this.showInformationIcon = true;
        this.showQuestionIcon = false;
        this.showWarningIcon = false;

        css += " information";
      }
      else if (this.msgType == MESSAGE_DIALOG_TYPE.QUESTION
                || this.msgType == MESSAGE_DIALOG_TYPE.QUESTION_OK
                || this.msgType == MESSAGE_DIALOG_TYPE.QUESTION_WITH_CANCEL) {
        this.showErrorIcon = false;
        this.showInformationIcon = false;
        this.showQuestionIcon = true;
        this.showWarningIcon = false;

        css += " question";
      }
      else if (this.msgType == MESSAGE_DIALOG_TYPE.WARNING
                || this.msgType == MESSAGE_DIALOG_TYPE.WARNING_QUESTION) {
        this.showErrorIcon = false;
        this.showInformationIcon = false;
        this.showQuestionIcon = false;
        this.showWarningIcon = true;

        css += " warning";
      }
      this.dialogCss = css;

      if (this.msgType == MESSAGE_DIALOG_TYPE.INFORMATION
          || this.msgType == MESSAGE_DIALOG_TYPE.WARNING) {
          this.returnType = MESSAGE_DIALOG_RETURN_TYPE.OK;
      }
      else if (this.msgType == MESSAGE_DIALOG_TYPE.ERROR) {
          this.returnType = MESSAGE_DIALOG_RETURN_TYPE.CLOSE;
      }
      else if (this.msgType == MESSAGE_DIALOG_TYPE.QUESTION) {
          this.returnType = MESSAGE_DIALOG_RETURN_TYPE.NO;
      }
      else if (this.msgType == MESSAGE_DIALOG_TYPE.QUESTION_WITH_CANCEL) {
          this.returnType = MESSAGE_DIALOG_RETURN_TYPE.CANCEL;
      }
      else if (this.msgType == MESSAGE_DIALOG_TYPE.QUESTION_OK) {
          this.returnType = MESSAGE_DIALOG_RETURN_TYPE.CANCEL;
      }

      if (this.msgType == MESSAGE_DIALOG_TYPE.INFORMATION
          || this.msgType == MESSAGE_DIALOG_TYPE.WARNING) {
        this.defaultOKButton = true;
        this.defaultCloseButton = false;
        this.defaultYesButton = false;
        this.defaultNoButton = false;
        this.defaultCancelButton = false;
      }
      else if (this.msgType == MESSAGE_DIALOG_TYPE.ERROR) {
        this.defaultOKButton = false;
        this.defaultCloseButton = true;
        this.defaultYesButton = false;
        this.defaultNoButton = false;
        this.defaultCancelButton = false;
      }
      else if (this.msgType == MESSAGE_DIALOG_TYPE.QUESTION) {
        this.defaultOKButton = false;
        this.defaultCloseButton = false;
        this.defaultYesButton = false;
        this.defaultNoButton = true;
        this.defaultCancelButton = false;
      }
      else if (this.msgType == MESSAGE_DIALOG_TYPE.QUESTION_WITH_CANCEL
                || this.msgType == MESSAGE_DIALOG_TYPE.WARNING_QUESTION) {
        this.defaultOKButton = false;
        this.defaultCloseButton = false;
        this.defaultYesButton = false;
        this.defaultNoButton = false;
        this.defaultCancelButton = true;
      }
      else if (this.msgType == MESSAGE_DIALOG_TYPE.QUESTION_OK) {
        this.defaultOKButton = false;
        this.defaultCloseButton = false;
        this.defaultYesButton = false;
        this.defaultNoButton = false;
        this.defaultCancelButton = true;
      }
      
      this.dialogCtrl.open();

      setTimeout(() => {
        if (this.defaultOKButton == true) {
            this.btnOK.nativeElement.focus();
        }
        else if (this.defaultCloseButton == true) {
            this.btnClose.nativeElement.focus();
        }
        else if (this.defaultNoButton == true) {
            this.btnNo.nativeElement.focus();
        }
        else if (this.defaultCancelButton == true) {
            this.btnCancel.nativeElement.focus();
        }
      }, 0);

    });
  }

  @Input("ok-caption")
  public okCaption: string;
  @Input("close-caption")
  public closeCaption: string;
  @Input("yes-caption")
  public yesCaption: string;
  @Input("no-caption")
  public noCaption: string;
  @Input("cancel-caption")
  public cancelCaption: string;

  private get buttonOKCaption(): string {
    if (this.okCaption != undefined) {
      return this.okCaption;
    }

    return "OK";
  }
  private get buttonCloseCaption(): string {
    if (this.closeCaption != undefined) {
      return this.closeCaption;
    }

    return "Close";
  }
  private get buttonYesCaption(): string {
    if (this.yesCaption != undefined) {
      return this.yesCaption;
    }

    return "Yes";
  }
  private get buttonNoCaption(): string {
    if (this.noCaption != undefined) {
      return this.noCaption;
    }

    return "No";
  }
  private get buttonCancelCaption(): string {
    if (this.cancelCaption != undefined) {
      return this.cancelCaption;
    }

    return "Cancel";
  }

  private get showOKButton() {
    return this.msgType == MESSAGE_DIALOG_TYPE.INFORMATION
        || this.msgType == MESSAGE_DIALOG_TYPE.WARNING
        || this.msgType == MESSAGE_DIALOG_TYPE.WARNING_QUESTION
        || this.msgType == MESSAGE_DIALOG_TYPE.QUESTION_OK;
  }
  private get showCloseButton() {
      return this.msgType == MESSAGE_DIALOG_TYPE.ERROR;
  }
  private get showYesButton() {
      return this.msgType == MESSAGE_DIALOG_TYPE.QUESTION
          || this.msgType == MESSAGE_DIALOG_TYPE.QUESTION_WITH_CANCEL;
  }
  private get showNoButton() {
      return this.msgType == MESSAGE_DIALOG_TYPE.QUESTION
          || this.msgType == MESSAGE_DIALOG_TYPE.QUESTION_WITH_CANCEL;
  }
  private get showCancelButton() {
      return this.msgType == MESSAGE_DIALOG_TYPE.QUESTION_WITH_CANCEL
      || this.msgType == MESSAGE_DIALOG_TYPE.WARNING_QUESTION
      || this.msgType == MESSAGE_DIALOG_TYPE.QUESTION_OK;
  }

  private onOKClick() {
    this.returnType = MESSAGE_DIALOG_RETURN_TYPE.OK;
    this.dialogCtrl.close()
        .then(() => {
          if (typeof (this.caller) == "function") {
            this.caller.call(null, null);
          }
        });
  }
  private onCloseClick() {
      this.returnType = MESSAGE_DIALOG_RETURN_TYPE.CLOSE;
      this.dialogCtrl.close()
        .then(() => {
          if (typeof (this.caller) == "function") {
            this.caller.call(null, null);
          }
        });
  }
  private onYesClick() {
      this.returnType = MESSAGE_DIALOG_RETURN_TYPE.YES;
      this.dialogCtrl.close()
        .then(() => {
          if (typeof (this.caller) == "function") {
            this.caller.call(null, null);
          }
        });
  }
  private onNoClick() {
      this.returnType = MESSAGE_DIALOG_RETURN_TYPE.NO;
      this.dialogCtrl.close()
        .then(() => {
          if (typeof (this.caller) == "function") {
            this.caller.call(null, null);
          }
        });
  }
  private onCancelClick() {
      this.returnType = MESSAGE_DIALOG_RETURN_TYPE.CANCEL;
      this.dialogCtrl.close()
        .then(() => {
          if (typeof (this.caller) == "function") {
            this.caller.call(null, null);
          }
        });
  }
}
