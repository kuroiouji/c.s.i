import { Injectable } from "@angular/core";

@Injectable()
export class CtmApiLoader {
    constructor() {
    }

    public show(div = ".api-loading"): void {
        if (div == undefined) {
            div = ".api-loading";
        }

        $(div).show();
    }
    public hide(delay:number = 0, div = ".api-loading"):void {
        if (div == undefined) {
            div = ".api-loading";
        }

        if (delay == 0) {
            $(div).hide();
        }
        else {
            $(div).fadeOut(delay);
        }
    }

    public isShow(div = ".api-loading"): boolean {
        if (div == undefined) {
            div = ".api-loading";
        }

        return $(div).is(":visible") ; 
    }
}