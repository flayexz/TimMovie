import React from 'react';
import {IFullUserInfoDto} from "../../../dto/IFullUserInfoDto";
import LineWithSvg from "../LineWithSvg";
import ContainerHeader from "./ContainerHeader";
import Icon from "../../../svg-icons/Icon";
import {ClickHook} from "../ClickHook/ClickHook";

interface IContainerProps{
    info: string[] | undefined
    isSubscribes?: boolean
    clickHook: ClickHook
}

const addBtn = <div className="add_remove_btns add_btn">
    <Icon name={"Add"}/>
</div>

const removeBtn = <div className="add_remove_btns remove_btn">
    <Icon name={"Remove"}/>
</div>



const Container = ({info, isSubscribes = false, clickHook} :IContainerProps) => {

    function getItemTextWithIcon(item: string, icon: string){
        console.log(icon)
        item = item.charAt(0).toUpperCase() + item.slice(1)
        if (icon === "") icon = item;
        return <LineWithSvg icon={icon} line={item} clickHook={clickHook}/>
    }

    return (
        <div>
            {info?.map(item => getItemTextWithIcon(item, isSubscribes? "Subscribe" : ""))}
        </div>
    );
};

export default Container;