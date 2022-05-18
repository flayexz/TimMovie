import React, {useEffect, useState} from 'react';
import EditButtons from "./EditButtons";
import './userProfile.css';
import {IFullUserInfoDto} from "../../dto/IFullUserInfoDto";
import $api from "../../http";
import {useParams} from "react-router-dom";
import UserInfoContainer from "./containers/UserInfoContainer";
import ContainerHeader from "./containers/ContainerHeader";
import Container from "./containers/Container";
import Icon from "../../svg-icons/Icon";
import LineWithSvg from "./LineWithSvg";

const UserInfo = (props: { userId: string | undefined }) => {
    const [info, setInfo] = useState<IFullUserInfoDto>();

    useEffect(() => {
        getAllUserInfo();
    }, [])

    async function getAllUserInfo() {
        let url = `/users/user?id=${props.userId}`;
        const response = await $api.get<IFullUserInfoDto>(url);
        console.log(response);
        setInfo(response.data);
    }

    return (
        <div className="container">
            <EditButtons/>
            <div className="row mt-5">
                <div className="user_info_container col-12 col-md-3">
                    <UserInfoContainer info={info}/>
                </div>

                <div className="containers roles_container col-12 col-md-3 offset-md-1">
                    <LineWithSvg icon={"Roles"} line={"Роли пользователя"} isHeader={true} iconWidth={28} iconHeight={21}/>
                    <Container info={info?.roles}/>
                </div>

                <div className="containers subscribes_container col-12 col-md-4 offset-md-1">
                    <LineWithSvg icon={"Subscribes"} line={"Доступные подписки"} isHeader={true} iconWidth={28} iconHeight={21}/>
                    <Container info={info?.subscribes}/>
                </div>
            </div>
        </div>

    );
};

export default UserInfo;