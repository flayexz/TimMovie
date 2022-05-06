import React from 'react';

const UserInfo = () => {
    return (
        <div className="row">
            <div className="col-2">
                <button className="btns" id="edit_btn">Изменить</button>
            </div>
            <div className="col-2">
                <button className="btns" id="save_btn">Сохранить</button>
            </div>
        </div>
    );
};

export default UserInfo;