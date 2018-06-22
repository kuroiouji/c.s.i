import { 
  Component, 
  ViewChild,
  ElementRef
}                                       from "@angular/core";
import {
  Router,
  ActivatedRoute
}                                       from "@angular/router";
import {
  FormBuilder,
  Validators
}                                       from "@angular/forms";

import { 
  Http, 
  Headers, 
  RequestOptions, 
  Response 
}                                           from "@angular/http";
import {
  ABaseComponent,
  BaThemeSpinner,
  ApiService,
  CtmScreenCommand,
  CtmTranslateService,
  CtmMessageService,
  CtmCommandService,
  CtmTableService,
  CtmTableData,
  CtmTextbox,
  CtmControlManagement,
  Md2Autocomplete,
  ConfirmChangeScreen,
  CONFIRM_CHANGE_SCREEN,
  ROLE_PERMISSION_TABLE,
  MESSAGE_DIALOG_TYPE,
  MESSAGE_DIALOG_RETURN_TYPE,
  DEFAULT_GROUP_CHECKER,
  PhaseAutoCompleteCriteria
}                                       from "../../../common";
import { ProjectService }                 from "../project.service";
import { Ng2FileInputService, Ng2FileInputAction } from 'ng2-file-input';//<----importtt

import {UploadService} from './upload.service';
 
import "style-loader!./upload.component.scss";
@Component({
  selector: "upload",
  templateUrl: "./upload.component.html"  
})
export class uploadComponent extends ABaseComponent implements ConfirmChangeScreen   {
  @ViewChild(CtmScreenCommand) commandCtrl: CtmScreenCommand;
  @ViewChild("fileInput") fileInput;

  
  picName: string;
  private ctrlMgr: CtmControlManagement;
  private myFileInputIdentifier:string = "tHiS_Id_IS_sPeeCiAL";
  public actionLog:string="";
  constructor(
    
    router: Router,
    route: ActivatedRoute,
    spinner: BaThemeSpinner,
    translate: CtmTranslateService,
    message: CtmMessageService,
    command: CtmCommandService,
    screenParam: ProjectService,

    private fb: FormBuilder,
    private api: ApiService,
    private table: CtmTableService,
    private http:Http,
    
    private ng2FileInputService: Ng2FileInputService,

    private service:UploadService
) {
  super(router, route, spinner, translate, message, command, {
        root: "/s/project/issue/",
        screenParam: screenParam,
        commandMgr: {
        }            
    });  

    this.service.progress$.subscribe(
      data => {
       console.log('progress = '+data);
       });
  }
  IResetBecauseICan():void{
    this.ng2FileInputService.reset(this.myFileInputIdentifier);
}
  public onAction(event: any){
    console.log(event);
    this.actionLog += "\n currentFiles: " + this.getFileNames(event.currentFiles);
    console.log(this.actionLog);
  }
  public onAdded(event: any){
    this.actionLog += "\n FileInput: "+event.id;
    this.actionLog += "\n Action: File added";
  }
  public onRemoved(event: any){
    this.actionLog += "\n FileInput: "+event.id;
    this.actionLog += "\n Action: File removed";
  }
  public onInvalidDenied(event: any){
    this.actionLog += "\n FileInput: "+event.id;
    this.actionLog += "\n Action: File denied";
  }
  public onCouldNotRemove(event: any){
    this.actionLog += "\n FileInput: "+event.id;
    this.actionLog += "\n Action: Could not remove file";
  }
  public resetFileInput():void{
    this.ng2FileInputService.reset(this.myFileInputIdentifier);
  }
  public logCurrentFiles():void{
    let files=this.ng2FileInputService.getCurrentFiles(this.myFileInputIdentifier);
    this.actionLog += "\n The currently added files are: " + this.getFileNames(files);
  }
  private getFileNames(files:File[]):string{
    let names=files.map(file => file.name);
    return names ? names.join(", "): "No files currently added.";
  }
  
  get screenCommand() { return this.commandCtrl; }
  get screenChanged() : boolean {
    return this.ctrlMgr.changed == true;
  }
  initScreen(): Promise<Object> {
      return ;     
  }
  resetChanged() {
    this.ctrlMgr.resetChanged();
  }

  public confirmChange(): CONFIRM_CHANGE_SCREEN {
    return this.screenChanged ? CONFIRM_CHANGE_SCREEN.CONFIRM_UNSAVE : CONFIRM_CHANGE_SCREEN.CHANGE;
  }

  public confirmChangeResult(result: boolean) {
  }
  

  addFile(): void {
    let fi = this.fileInput.nativeElement;
    if (fi.files && fi.files[0]) {
    let fileToUpload = fi.files[0];
    let formData = new FormData();
    for (let i = 0; i < fileToUpload.length; i++) {
        formData.append("file", fileToUpload[0]);
    }

    let headers = new Headers({
      "Content-Type": "application/json; charset=UTF-8",
      "Access-Control-Allow-Origin": "*",
      "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept",
  });
  let options = new RequestOptions({
      "headers": headers,
      //"withCredentials": true 
  });


    this.http
        .post("http://localhost:50574/api/ISSUE/upload", formData,options)
        .subscribe(res => {
          console.log(res);
      });


    }
  }
  
  onChange(event) {
    console.log('onChange');
      var files = event.srcElement.files;
     console.log(files);
        this.service.makeFileRequest('http://localhost:50574/api/ISSUE/upload', [],files).subscribe(() => {
     console.log('sent');
     this.picName = files;
  });
  }
}
