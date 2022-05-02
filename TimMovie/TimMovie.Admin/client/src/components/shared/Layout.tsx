import {Outlet, NavLink} from "react-router-dom";
import React from "react";
import Logout from "../logout/Logout";

function Layout(){
    return (
        <>
            <div className="container-xl">
                <header>
                    <NavLink to="/" >Пользоваетели</NavLink>
                    <Logout/>
                </header>
            </div>
            <div className="container-xl">
                <Outlet/>
            </div>
        </>
    );
}

export default Layout;