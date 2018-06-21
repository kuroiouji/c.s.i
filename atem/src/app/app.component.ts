import { Component, ViewEncapsulation } from '@angular/core';
import { CtmTranslateService } from "./_common";
import { CONSTANT, CtmConstantService } from "./_custom";

@Component({
  selector: 'my-app',
  styleUrls: ['./app.component.scss'],
  templateUrl: './app.component.html',
  encapsulation: ViewEncapsulation.None
})
export class AppComponent {

  constructor(
    private translate: CtmTranslateService,
    private constant: CtmConstantService
  ) {
  }

  activateEvent(event) {
    if (ENV === 'development') {
      console.log('Activate Event:', event);
    }

    if ($(".app-loading").is(":visible")) {
      this.constant.initialConstant()
        .then(() => {
          this.translate.use(CONSTANT.DEFAULT_LANGUAGE);
          this.translate.initialResource()
            .then(() => {
              $(".app-loading").fadeOut(300);

              if (event["initScreen"] != undefined
                  && typeof(event["initScreen"]) == "function") {
                event["initScreen"]();
              }
            });
        });
    }
    else {
      if (event["initScreen"] != undefined
          && typeof(event["initScreen"]) == "function") {
        event["initScreen"]();
      }
    }
  }

  deactivateEvent(event) {
    if (ENV === 'development') {
      console.log('Deactivate Event', event);
    }
  }
}
