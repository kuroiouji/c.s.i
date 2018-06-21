import { Pipe, PipeTransform } from "@angular/core";
import { textNumeric } from "../functions";

@Pipe({ name: "numeric" })
export class CtmNumericPipe implements PipeTransform {
    transform(value: Number, option = null):string {
        return textNumeric(value, option);
    }
}