import React from "react";
import AddFilmForm from "./AddFilmForm";
import filmPageClasses from "./filmPage.module.css"
import TableWithFilms from "./TableWithFilms";

function FilmsPage() {
    return (
        <div className="d-flex flex-column mt-4">
            <div className={filmPageClasses.addFormContainer}> 
                <AddFilmForm/>
            </div>
            <TableWithFilms/>
        </div>
    );
}

export default FilmsPage;