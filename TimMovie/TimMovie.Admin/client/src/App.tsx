import React from 'react';
import { Route, Routes } from "react-router-dom";
import UsersTablePage from "./components/tableWithUser/UsersTablePage";
import Login from "./components/login/Login";
import Layout from "./components/shared/Layout";
import UserProfile from "./components/userProfile/UserProfile";
import AuthProvider from "./components/auth/AuthProvider";
import RequireAuth from "./components/auth/RequireAuth";

function App() {
    return (
        <AuthProvider>
            <Routes>
                <Route path="/login" element={<Login/>}/>
                <Route path="/" element={<RequireAuth><Layout/></RequireAuth>}>
                    <Route index element={<UsersTablePage/>}/>
                    <Route path="/users/:id" element={<UserProfile/>}/>
                </Route>
            </Routes>
        </AuthProvider>
    );
}

export default App;
