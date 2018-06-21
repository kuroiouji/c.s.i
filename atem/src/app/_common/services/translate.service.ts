import { Injectable } from "@angular/core";
import { HttpClient} from '@angular/common/http';

import { CONSTANT, LANGUAGES, RESOURCES_DATA, RESOURCES_PATH } from "../../_custom";

@Injectable()
export class CtmTranslateService {
    public currentLang: string;

    // inject our translations
    constructor(
        private http: HttpClient
    ) {
    }

    public use(lang: string): void {
        // set current language
        this.currentLang = lang;
    }
    public initialResource(): Promise<Object> {
        return new Promise((resolve, reject) => {
            let lr = (idx) => {
                if (idx >= LANGUAGES.length) {
                    resolve();
                    return;
                }
                                
                let lang = LANGUAGES[idx];
                if (RESOURCES_DATA[lang.name] == undefined) {
                    this.http.get(`${RESOURCES_PATH}/${lang.name}.json`)
                        .toPromise()
                        .then((res) => {
                            RESOURCES_DATA[lang.name] = res;
        
                            lr(idx + 1);
                        })
                        .catch(() => {
                            lr(idx + 1);
                        });
                }
                else {
                    lr(idx + 1);
                }
            }

            lr(0);
        });
    }

    public instant(key: string, module: string) {
        return this.translate(key, module);
    }
    public get isLanguageEN() {
        return this.currentLang == "en-US";
    }

    private translate(key: string, module: string): string {
        if (this.currentLang == undefined) {
            this.use(CONSTANT.DEFAULT_LANGUAGE);
        }
        
        let translation = key;
        var tran = RESOURCES_DATA[this.currentLang];
        if (tran != undefined) {
            if (module != undefined) {
                var tranf = tran[module];
                if (tranf != undefined) {
                    if (tranf[key] != undefined) {
                        return tranf[key];
                    }
                }
            }
        }

        return translation;
    }
}