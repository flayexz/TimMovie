import React from "react"
import classes from "./table.module.css";
import {IUserDto} from "../../dto/IUserDto";

interface IColumnTableProps{
    nameColumn: string;
    users: IUserDto[];
    userPropName: string;
}

function ColumnTable({nameColumn, users, userPropName}: IColumnTableProps){
    return (
        <div className="d-flex flex-column">
            <h5>{nameColumn}</h5>
            {users.map((user: any) => <div className={classes.userRecord} key={user.id}>{user[userPropName]}</div>)}
        </div>
    );
}

export default ColumnTable;