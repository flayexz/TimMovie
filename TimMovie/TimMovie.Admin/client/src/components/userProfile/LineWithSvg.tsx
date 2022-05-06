import React from 'react';
import SvgIcon from "./svgComponents/SvgIcon";
import classes from './userProfile.module.css'
import './userProfile.css';

interface LineProps{
    icon: string,
    line:string | undefined,
    isBold?:boolean
}

const LineWithSvg:React.FC<LineProps> = ({icon, line, isBold}:LineProps) => {
    return (
        <div>
            <div className={`${classes.line} d-flex align-items-center`}>
                <div className={classes.circleForIcon}>
                    <SvgIcon icon={icon}/>
                </div>
                <span className={classes.content_str} style={{fontWeight: isBold ? 'bold' : 'normal'}}>
                    {line}
                </span>
            </div>
        </div>
    );
};

export default LineWithSvg;