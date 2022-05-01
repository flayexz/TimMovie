import {Outlet, NavLink} from "react-router-dom";
import React from "react";

function Layout(){
    return (
        <>
            <header>
                <NavLink to="/" >Пользоваетели</NavLink>
            </header>
            <div className="container-xl">
                <Outlet/>
            </div>
        </>
    );
}

export default Layout;