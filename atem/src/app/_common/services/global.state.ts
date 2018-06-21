import { Injectable }   from '@angular/core';
import { Observable, Subscriber } from 'rxjs';


@Injectable()
export class GlobalState {
    private subscriptions: Map<string, Object> = new Map<string, Object>();

    constructor() {
    }

    public notify(key: string, value) {
        let obs = this.subscriptions.get(key);
        if (obs != undefined) {
            obs["sbs"].forEach((observer: Subscriber<Object>) => {
                observer.next(value);
            });
        }
    }
    public subscribe(key: string, callback: Function) {
        let obs = this.subscriptions.get(key);
        if (obs == undefined) {
            obs = {
                "ob": new Observable((observer) => {
                            console.log("init observer");
                
                            let iobs = this.subscriptions.get(key);
                            if (iobs != undefined) {
                                iobs["sbs"].push(observer);
                            }
                            return {
                                unsubscribe() {
                                    console.log("unsubscribe");
                                    
                                    let iobs = this.subscriptions.get(key);
                                    if (iobs != undefined) {
                                        iobs["sbs"] = [];
                                    }
                                }
                            };
                        }),
                "sbs": []
            };
            this.subscriptions.set(key, obs);
        }

        obs["ob"].subscribe({
            next: (value) => {
                console.log("callbak");
                callback.call(null, value);
            },
            error: (err) => {
                console.log("error");
                console.log(err);
            },
            complete: () => {
                console.log("Complete");
            }
        });
    }
    public unSubscribe(key: string) {
        let obs = this.subscriptions.get(key);
        if (obs != undefined) {
            obs["ob"].unsubscribe();
        }
    }
}