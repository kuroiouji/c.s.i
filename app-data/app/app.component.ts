import { 
	Component, 
	ViewContainerRef
}									from "@angular/core";
import {
	GlobalState
}									from "./global.state";

import {
  BaThemePreloader,
  BaThemeSpinner,
  ApiService,
  CtmTranslateService,
  MENU_GLOBAL_COLLAPSED,
  SERVER,
  SERVER_PATH
}                                 	from "./common";

import { Http }                 from "@angular/http";
import { 
  RESOURCES_PATH
}                               from "./common/models"; // import our opaque token

import "style-loader!./app.scss";
import "style-loader!./common/style/initial.scss";

@Component({
  selector: "app",
  templateUrl: "./app.component.html"
})
export class App {
	private isMenuCollapsed: boolean = true;

	constructor(
		private spinner: BaThemeSpinner,
    private api: ApiService,
    private http:Http,
		private global: GlobalState,
		private translate: CtmTranslateService
	) {
    this.get_path();
    setTimeout(()=>{    //<<<---    using ()=> syntax
      this.initLanguage();
    },500);
      
      this.global.subscribe(MENU_GLOBAL_COLLAPSED, (isCollapsed) => {
        this.isMenuCollapsed = isCollapsed;
        
      });
    
    }

private get_path(){
    this.http.get(`${RESOURCES_PATH}/config.json`)
    .toPromise()
    .then((res) => {
       SERVER.PATH = res.json().SERVER_PATH;
    })
    .catch(() => {
        this.get_path();
    });
  }

	public ngAfterViewInit(): void {
		// hide spinner once all loaders are completed
		BaThemePreloader.load().then((values) => {  
      this.spinner.hide(500);
		});
	}

	private initLanguage(): void {
      BaThemePreloader.registerLoader(new Promise((resolve) => {
        this.api.callApiController("api/Common/GetCurrentLanguage", {
              type: "POST"
          }).then(
            (result) => {
              this.translate.use(result as string);
              
              this.translate.initialResource()
                .then(() => {
                  resolve();
                });
          });
      }));
  }
}
