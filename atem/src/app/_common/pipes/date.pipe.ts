import { Pipe, PipeTransform } from "@angular/core";
import { textDateLocale } from "../functions";
  
@Pipe({ name: "date" })
export class CtmDatePipe implements PipeTransform {
    transform(value: Date, format = "DD/MM/YYYY", locale = null):string {
        return textDateLocale(value, format, locale);
    }
}