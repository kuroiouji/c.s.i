export enum ROLE_PERMISSION {
    OPEN = 1,
    ADD,
    EDIT,
    DELETE,
    PRINT,
    IMPORT,
    EXPORT
}
export enum ROLE_PROJECT{
    EDIT_PROJECT = 1,
    EDIT_ISSUE,
    PLAN,
    PROCESS_FIX,
    CHECK,
    CLOSE,
    MISS,
    CANCEL,
    RE_DEBUG,
    CREATE_PRO,
    DELETE_PRO
}

export const ROLE_PERMISSION_TABLE = [
    {   
        permissionID: ROLE_PERMISSION.OPEN, 
        permissionName: "tablePermissionOpen", 
        propSCN: "SCN_1", 
        propROL: "ROL_1" 
    },
    { 
        permissionID: ROLE_PERMISSION.ADD, 
        permissionName: "tablePermissionAdd", 
        propSCN: "SCN_2", 
        propROL: "ROL_2" 
    },
    { 
        permissionID: ROLE_PERMISSION.EDIT, 
        permissionName: "tablePermissionEdit", 
        propSCN: "SCN_3", 
        propROL: "ROL_3" 
    },
    { 
        permissionID: ROLE_PERMISSION.DELETE, 
        permissionName: "tablePermissionDelete", 
        propSCN: "SCN_4", 
        propROL: "ROL_4" 
    },
    { 
        permissionID: ROLE_PERMISSION.PRINT, 
        permissionName: "tablePermissionPrint", 
        propSCN: "SCN_5", 
        propROL: "ROL_5" 
    },
    { 
        permissionID: ROLE_PERMISSION.IMPORT, 
        permissionName: "tablePermissionImport", 
        propSCN: "SCN_6", 
        propROL: "ROL_6" 
    },
    { 
        permissionID: ROLE_PERMISSION.EXPORT, 
        permissionName: "tablePermissionExport", 
        propSCN: "SCN_7", 
        propROL: "ROL_7" 
    }
];