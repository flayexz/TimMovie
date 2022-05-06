import React, {useEffect, useState} from 'react';
import EditButtons from "./EditButtons";
import './userProfile.css';
import LineWithSvg from "./LineWithSvg";
import ProfileIcon from "./svgComponents/UserInfo/profileIcon";
import {IFullUserInfoDto} from "../../dto/IFullUserInfoDto";
import $api from "../../http";
import {useParams} from "react-router-dom";

const UserInfo = (props:{userId:string | undefined}) => {
    const [info, setInfo] = useState<IFullUserInfoDto>();
    const [fetching, setFetching] = useState(true);

    useEffect(()=>{
        getUserInfo();
    }, [])

    async function getUserInfo(){
        let url = `/users/user?id=${props.userId}`;
        const response = await $api.get<IFullUserInfoDto>(url);
        console.log(response);
        setInfo(response.data);
    }

    return (
        <div className="container-xl">
            <EditButtons/>
            <div className="row mt-5">
                <div className="user_info_container col-3">
                    <div className="user_info">
                        <LineWithSvg icon={"Profile"} line={info?.displayName?.toString()} isBold={true}/>
                        <LineWithSvg icon={"Login"} line={info?.login?.toString()}/>
                        <LineWithSvg icon={"Mail"} line={info?.email?.toString()}/>
                        {/*<LineWithSvg icon={"BirthDate"} line={info?.birthDate.toString()}/>*/}
                        <LineWithSvg icon={"Country"} line={info?.countryName?.toString()}/>
                        {/*<LineWithSvg icon={"RegisterDate"} line={info?.registrationDate.toString()}/>*/}
                    </div>
                </div>
            </div>
        </div>

    );
};

export default UserInfo;