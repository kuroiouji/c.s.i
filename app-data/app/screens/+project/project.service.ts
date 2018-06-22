import { Injectable }       from "@angular/core";

import { 
    AScreenParameter
}                           from "../../common";

@Injectable()
export class ProjectService extends AScreenParameter {
    public is_pj_id: string;
    public is_id: string;

    public get key() : any {
        return this.is_id;
    }
    public set key(k: any) {
        this.is_id = k;
    }
    
}