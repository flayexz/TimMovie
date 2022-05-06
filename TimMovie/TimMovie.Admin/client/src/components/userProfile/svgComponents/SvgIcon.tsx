import React from 'react';
import AddIcon from "./Buttons/AddIcon";
import RemoveIcon from "./Buttons/RemoveIcon";
import BirthDateIcon from "./UserInfo/birthDateIcon";
import CountryIcon from "./UserInfo/countryIcon";
import LoginIcon from "./UserInfo/loginIcon";
import MailIcon from "./UserInfo/mailIcon";
import ProfileIcon from "./UserInfo/profileIcon";
import RegisterDateIcon from "./UserInfo/registerDateIcon";
import AdminIcon from "./Roles/adminIcon";
import ModeratorIcon from "./Roles/moderatorIcon";
import SupportIcon from "./Roles/supportIcon";
import BannedIcon from "./Roles/bannedIcon";
import SubscribesIcon from "./Subscribes/subscribesIcon";
import SubscribeIcon from "./Subscribes/subscribeIcon";

const SvgIcon = (props: { icon: string }) => {
    console.log(props);
    switch (props.icon) {
        //userInfo
        case ("Profile"):
            return <ProfileIcon/>
            break;
        case "Login":
            return <LoginIcon/>
            break;
        case "Mail":
            return <MailIcon/>
            break;
        case "BirthDate":
            return <BirthDateIcon/>
            break;
        case "Country":
            return <CountryIcon/>
            break;
        case "RegisterDate":
            return <RegisterDateIcon/>
            break;

        //Buttons
        case "Add":
            return <AddIcon/>
            break;
        case "Remove":
            return <RemoveIcon/>
            break;

        //Subscribes
        case "Subscribes":
            return <SubscribesIcon/>
            break;
        case "Subscribe":
            return <SubscribeIcon/>
            break;

        //Roles
        case ("User"):
            return <ProfileIcon/>
            break;
        case "Admin":
            return <AdminIcon/>
            break;
        case "Moderator":
            return <ModeratorIcon/>
            break;
        case "Support":
            return <SupportIcon/>
            break;
        case "Banned":
            return <BannedIcon/>
            break;
        default:
            return <AddIcon/>
    }
};

export default SvgIcon;