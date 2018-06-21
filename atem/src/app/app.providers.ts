import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { BrowserXhr } from '@angular/http';
import { of } from 'rxjs';

import { AppState } from './app.service';
import { GlobalState, AuthGuard } from "./_common";
import { CtmConstantService } from "./_custom";

@Injectable()
export class DataResolver implements Resolve<any> {
  public resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return of({ res: 'I am data'});
  }
}
@Injectable()
export class CustomBrowserXhr extends BrowserXhr {
    build(): any {
        let xhr = super.build();
        xhr.withCredentials = true;
        return <any>(xhr);
    }
}

export const APP_PROVIDERS = [
    DataResolver,
    
    AppState,
    GlobalState,
    CtmConstantService,
    AuthGuard,
    
    {
        provide: BrowserXhr,
        useClass: CustomBrowserXhr
    },
];
