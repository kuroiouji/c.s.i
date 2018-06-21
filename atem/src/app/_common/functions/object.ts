export function cloneObject(object) {
    if (object == undefined) {
        return { };
    }

    return JSON.parse(JSON.stringify(object));;
}