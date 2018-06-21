export const MESSAGE_GLOBAL_KEY: string = "message.dialog";
export const USER_LOCAL_STORAGE: string = "TEMPLATE_USER_STORAGE";

export enum MESSAGE_DIALOG_TYPE {
    INFORMATION,
    ERROR,
    WARNING,
    WARNING_QUESTION,
    QUESTION,
    QUESTION_OK,
    QUESTION_WITH_CANCEL
}
export enum MESSAGE_DIALOG_RETURN_TYPE {
    OK,
    CANCEL,
    YES,
    NO,
    CLOSE
}

export enum CONFIRM_CHANGE_SCREEN {
    CHANGE,
    CONFIRM_UNSAVE
}
export interface ConfirmChangeScreen {
    confirmChange: () => CONFIRM_CHANGE_SCREEN;
    confirmChangeResult: (result: boolean) => void;
}