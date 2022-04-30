import React, {useEffect, useState} from 'react';
import UsersTablePage from "./components/tableWithUser/UsersTablePage";
import Login from "./components/login/login";
import AuthService from "./services/authService";

function App() {
    if(!AuthService.isAdminAuth()){
        return <Login/>
    }
    return (
        <div className="container-xl">
            <UsersTablePage/>
        </div>
    );
}

export default App;
