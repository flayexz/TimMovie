import React, {useContext} from 'react';
import classes from './userProfile.module.css'
import './userProfile.css';
import Icon from "../../svg-icons/Icon";
import {ClickHook} from "./ClickHook/ClickHook"

interface LineProps {
    icon: string,
    line: string | null | undefined,
    isBold?: boolean
    isHeader?: boolean
    iconWidth?: number
    iconHeight?: number
    clickHook?: ClickHook
}

const addBtn = <div className="add_remove_btns add_btn">
    <Icon name={"Add"}/>
</div>

const removeBtn = <div className="add_remove_btns remove_btn">
    <Icon name={"Remove"}/>
</div>

const LineWithSvg: React.FC<LineProps> = ({
                                              icon,
                                              line,
                                              isBold,
                                              isHeader = false,
                                              iconWidth = 24,
                                              iconHeight = 24,
                                              clickHook
                                          }: LineProps) => {

    return (
        <div className={`${classes.line} d-flex align-items-center`}>
            <div className={classes.circleForIcon}>
                <Icon name={icon} width={iconWidth} height={iconHeight}/>
            </div>
            <span className={classes.content_str}
                  style={{
                      fontWeight: isBold ? 'bold' : 'normal',
                      textDecoration: isHeader ? 'underline' : 'normal',
                      textDecorationThickness: '2px',
                      textUnderlineOffset: "6px"
                  }}>
                    {line}
            </span>

            {clickHook?.clickState ?
                    <div className="add_remove_btns add_btn">
                        <Icon name={"Add"}/>
                    </div>
                    : ''}
        </div>
    );
};

export default LineWithSvg;