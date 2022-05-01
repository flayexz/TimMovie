import React from 'react';
import { Route, Routes } from "react-router-dom";
import UsersTablePage from "./components/tableWithUser/UsersTablePage";
import Layout from "./components/shared/Layout";
import UserProfile from "./components/userProfile/UserProfile";

function App() {
    return (
        <Routes>
            <Route path="/" element={<Layout/>}>
                <Route index element={<UsersTablePage/>}/>
                <Route path="/users/:id" element={<UserProfile/>}/>
            </Route>
        </Routes>
    );
}

export default App;
