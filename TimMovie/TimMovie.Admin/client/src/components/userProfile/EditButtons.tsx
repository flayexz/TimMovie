import React, {useContext} from 'react';
import ReactDOM from 'react-dom';
import Icon from "../../svg-icons/Icon";
import {ClickHook} from "./ClickHook/ClickHook"

function addButtons() {
    const addBtn = <div className="add_remove_btns add_btn">
        <Icon name={"Add"}/>
    </div>

    const removeBtn = <div className="add_remove_btns remove_btn">
        <Icon name={"Remove"}/>
    </div>
}

interface EditButtonProps {
    clickHook: ClickHook
}

const EditButtons: React.FC<EditButtonProps> = ({clickHook}:EditButtonProps) => {
    return (
        <div className="row">
            <div className="col-md-2 col-6">
                <button className="btns" id="edit_btn"
                        onClick={() => clickHook.setClickState(!clickHook.clickState)}>Изменить
                </button>
            </div>
            <div className="col-md-2 col-6">
                <button className="btns" id="save_btn">Сохранить</button>
            </div>
        </div>
    );
};

export default EditButtons;