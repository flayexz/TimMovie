import React from 'react';
import {IFullUserInfoDto} from "../../../dto/IFullUserInfoDto";
import LineWithSvg from "../LineWithSvg";
import ContainerHeader from "./ContainerHeader";
import Icon from "../../../svg-icons/Icon";

interface IContainerProps{
    info: string[] | undefined

}

function getItemTextWithIcon(item: string){
    item = item.charAt(0).toUpperCase() + item.slice(1)
    return <LineWithSvg icon={item} line={item}/>
}

const Container = ({info} :IContainerProps) => {
    return (
        <div>
            {info?.map(item => getItemTextWithIcon(item))}
        </div>
    );
};

export default Container;