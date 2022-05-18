import React from 'react';
import classes from './userProfile.module.css'
import './userProfile.css';
import Icon from "../../svg-icons/Icon";

interface LineProps {
    icon: string,
    line: string | null | undefined,
    isBold?: boolean
    isHeader?: boolean
    iconWidth?:number
    iconHeight?:number
}

const LineWithSvg: React.FC<LineProps> = ({icon, line, isBold, isHeader = false, iconWidth = 22, iconHeight = 22}: LineProps) => {
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
        </div>
    );
};

export default LineWithSvg;