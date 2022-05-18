import React, {FunctionComponent} from 'react';
import LineWithSvg from "../LineWithSvg";
import {IFullUserInfoDto} from "../../../dto/IFullUserInfoDto";

interface IUserInfoContainerProps {
    info: IFullUserInfoDto | undefined
}

const UserInfoContainer: React.FC<IUserInfoContainerProps> = ({info}:IUserInfoContainerProps) => {
    return (
        <div className="user_info">
            <LineWithSvg icon={"Profile"} line={info?.displayName} isBold={true}/>
            <LineWithSvg icon={"Login"} line={info?.login}/>
            <LineWithSvg icon={"Mail"} line={info?.email}/>
            <LineWithSvg icon={"BirthDate"} line={info?.birthDate}/>
            <LineWithSvg icon={"Country"} line={info?.countryName}/>
            <LineWithSvg icon={"RegisterDate"} line={info?.registrationDate?.toString()}/>
        </div>
    );
};

export default UserInfoContainer;