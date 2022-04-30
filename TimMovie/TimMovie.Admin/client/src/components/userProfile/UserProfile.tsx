import React from "react";
import {useParams} from "react-router-dom"

function UserProfile() {
    let {id: userId} = useParams();
    
    return (
        <>
            {userId}
        </>
    );
}

export default UserProfile;