import React from "react"
import {classNameConcat} from "../../common/classNameConcat";
import classes from "./table.module.css";
import {IShortUserInfoDto} from "../../dto/IShortUserInfoDto";

interface IColumnTableWithListProps{
    nameColumn: string;
    users: IShortUserInfoDto[];
    userPropName: string;
    messageInEmptyList: string;
}

function ColumnTableWithList({nameColumn, users, userPropName, messageInEmptyList}: IColumnTableWithListProps){
    return (
        <div className="d-flex flex-column">
            <h5>{nameColumn}</h5>
            {users.map((user: any) =>
                <textarea key={user.id} className={classNameConcat(classes.listItems, classes.userRecord)} disabled 
                          value={user[userPropName].length === 0 
                              ? messageInEmptyList 
                              : user[userPropName].join("\n")}/>)}
        </div>
    );
}

export default ColumnTableWithList;