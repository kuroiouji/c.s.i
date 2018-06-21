import { Injectable } from "@angular/core";
import { HttpClient} from '@angular/common/http';

import { CONSTANT, CONSTANT_PATH } from "../../_custom";

@Injectable()
export class CtmConstantService {
    constructor(
        private http: HttpClient
    ) {
    }

    public initialConstant(): Promise<Object>{
        if (CONSTANT.SERVER_PATH == undefined) {
            return this.http.get(CONSTANT_PATH)
                            .toPromise()
                            .then((res) => {
                                for(let prop in res) {
                                    CONSTANT[prop] = res[prop]
                                }
                                return true;
                            });
        }
        else {
            return new Promise((resolve)=> { resolve(true); });
        }
    }
}