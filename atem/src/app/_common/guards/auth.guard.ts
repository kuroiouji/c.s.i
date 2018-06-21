import { Injectable } from "@angular/core";
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

import { CtmApiService } from "../services";
import { USER_LOCAL_STORAGE } from "../common.constant";
import { CtmConstantService } from "../../_custom";

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(
        private http: HttpClient,
        private router: Router,
        private api: CtmApiService,
        private constant: CtmConstantService
    ) {       
    }

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean> | Promise<boolean> | boolean {
        let opt = {
			userKey: USER_LOCAL_STORAGE,
			anonymous: false,
			result: false
        };

        return this.constant.initialConstant()
            .then(() => {
                return this.api.checkToken(opt)
                        .then((res) => {
                            if (res != undefined) {
                                if (res["result"] != true) {
                                    this.router.navigate(["/login"], {
                                        queryParams: { returnUrl: state.url }
                                    });
                                    return false;
                                }
                                return true;
                            }
                            return false;
                        })
                        .catch(() => {
                            this.router.navigate(["/503"]);
                            return false;
                        });
            });
        
    }
}