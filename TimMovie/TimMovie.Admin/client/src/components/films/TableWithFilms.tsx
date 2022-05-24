﻿import usePagination from "../../hook/dynamicLoading/usePagination";
import FilmForTableDto from "../../dto/FilmForTableDto";
import DynamicScrolling from "../common/dynamicLoading/DynamicScrolling";
import React, {useRef} from "react";
import SearchForPagination from "../common/search/SearchForPagination";
import NamePart from "../../common/interfaces/NamePart";
import {Table} from "react-bootstrap";

function TableWithFilms(){
    let urlQuery = useRef<NamePart>({namePart: ""});
    const pagination = usePagination<FilmForTableDto, NamePart>({pagination: 30, url: `/films/collection`, urlQuery});
    
    return (
        <div className="mt-5 mb-5">
            <SearchForPagination pagination={pagination} label={"Поиск по названию"} className={"form-control"} stringForSearch={urlQuery}/>
            <DynamicScrolling paginationResult={pagination} amountScreenBeforeLoading={1.5} className="mt-5">
                <Table bordered hover>
                    <thead>
                    <tr>
                        <th/>
                        <th>Название</th>
                        <th>Страна</th>
                        <th>Год</th>
                        <th>Жанры</th>
                        <th>Продюсеры и актеры</th>
                        <th/>
                    </tr>
                    </thead>
                    <tbody>
                    {pagination.records.map(value =>
                            <tr key={value.id} style={{verticalAlign: "middle"}}>
                                <td width={100}>
                                    <div className="justify-content-center d-flex">
                                        <img src={value.image} style={{height:150}} className="" alt=""/>
                                    </div>
                                </td>
                                <td width={250}>{value.title}</td>
                                <td width={110}>{value.countryName}</td>
                                <td>{value.year}</td>
                                <td>
                                    <ul className="list-group" style={{overflow: "auto", height:150}}>
                                        {value.genreNames.map(genre =>  
                                            <li key={genre} className="list-group-item">{genre}</li>)}
                                    </ul>
                                </td>
                                <td>
                                    <ul className="list-group" style={{overflow: "auto", height:150}}>
                                        {value.actorsAndProducers.map(value =>
                                            <li key={value} className="list-group-item">{value}</li>)}
                                    </ul>
                                </td>
                                <td width={50}>
                                    <div className="d-flex flex-column align-items-center">
                                        <div title="Редактировать">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16"
                                                 fill="currentColor" className="bi bi-pencil" viewBox="0 0 16 16">
                                                <path
                                                    d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168l10-10zM11.207 2.5 13.5 4.793 14.793 3.5 12.5 1.207 11.207 2.5zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293l6.5-6.5zm-9.761 5.175-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325z"/>
                                            </svg>
                                        </div>
                                        <div className="mt-5" title="Удалить">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="20"
                                                 fill="red" className="bi bi-trash" viewBox="0 0 16 16">
                                                <path
                                                    d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z"/>
                                                <path fill-rule="evenodd"
                                                      d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z"/>
                                            </svg>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                    )}
                    </tbody>
                </Table>
            </DynamicScrolling>
        </div>
    );
}

export default TableWithFilms;