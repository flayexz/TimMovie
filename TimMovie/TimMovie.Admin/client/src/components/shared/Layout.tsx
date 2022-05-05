import {Outlet, NavLink} from "react-router-dom";
import React from "react";
import Logout from "../logout/Logout";

function Layout(){
    return (
        <>
            <header>
                <NavLink to="/" >Пользоваетели</NavLink>
                <Logout/>
            </header>
            <div className="container-xl">
                <Outlet/>
            </div>
        </>
    );
}

export default Layout;