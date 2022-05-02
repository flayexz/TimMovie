import React from "react"
import classes from "./table.module.css";
import {IUserDto} from "../../dto/IUserDto";
import {Link} from "react-router-dom";

interface IColumnTableProps{
    nameColumn: string;
    users: IUserDto[];
    userPropName: string;
    isLinked: boolean;
}

function ColumnTable({nameColumn, users, userPropName, isLinked}: IColumnTableProps){
    return (
        <div className="d-flex flex-column">
            <h5>{nameColumn}</h5>
            {users.map((user: any) => isLinked 
                ? <Link to={`/users/${user.id}`}><div className={classes.userRecord} key={user.id}>{user[userPropName]}</div></Link> 
                : <div className={classes.userRecord} key={user.id}>{user[userPropName]}</div>)}
        </div>
    );
}

export default ColumnTable;