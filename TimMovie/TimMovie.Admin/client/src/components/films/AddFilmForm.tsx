import React, {useEffect, useRef, useState} from "react";
import {UploadFiles} from "../common/upload/UploadFiles";
import filmFormClasses from "./filmForm.module.css";
import {classNameConcat} from "../../common/classNameConcat";
import DropdownWithMultipleSelector from "../common/dropdownWithMultipleSelector/DropdownWithMultipleSelector";
import DropdownSearchOneValue from "../common/dropdownSearchOneValue/DropdownSearchOneValue";
import {useInput} from "../../hook/input/useInput";
import {checkOnEmpty} from "../../templeteForPredicateWithErrorMessage/templetes";
import InputWithValidation from "../common/input/validation/InputWithValidation";
import TextareaWithValidation from "../common/textarea/validation/TextareaWithValidation";
import $api from "../../http";
import {Modal, Toast, ToastContainer} from "react-bootstrap";
import {AxiosResponse} from "axios";
import Result from "../../dto/Result";

function AddFilmForm() {
    const [preview, setPreview] = useState<string | null>(null);
    const [file, setFile] = useState<File>();
    const selectedActors = useRef(new Set<string>());
    const selectedProducers = useRef(new Set<string>());
    const selectedGenres = useRef(new Set<string>());
    const title = useInput({predicates: [checkOnEmpty]});
    const iframeLink = useInput({predicates: [checkOnEmpty]});
    const description = useInput({});
    const [isFree, setIsFree] = useState(false);
    const year = useInput({predicates: [
            {
                valueIsValid: value => Number(value) >= 1900 && Number(value) <= new Date().getFullYear(),
                errorMessage: `Дата должна находится в промежутке от 1900 до ${new Date().getFullYear()}`
            },
            checkOnEmpty
        ]});
    const [selectedCountry, setSelectedCountry] = useState("Выберете страну");
    const [formIsValid, setFormIsValid] = useState(false);
    const [messageAboutAddIsShow, setMessageAboutAddIsShow] = useState(false);
    const [errorMessageIsShow, setErrorMessageIsShow] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    
    function generateRequestData(){
        const formData = new FormData();
        formData.append("img", file!);
        formData.append("title", title.value);
        formData.append("filmLink", iframeLink.value);
        formData.append("description", description.value);
        formData.append("isFree", `${isFree}`);
        formData.append("year", year.value);
        formData.append("countryName", selectedCountry);
        appendCollection(formData, selectedActors.current, "actorNames");
        appendCollection(formData, selectedProducers.current, "producerNames");
        appendCollection(formData, selectedGenres.current, "genreNames");
        
        return formData;
    }
    
    useEffect(() => {
        setFormIsValid(checkFormOnValid())
    }, [title, iframeLink, description, year]);

    function appendCollection(formData: FormData, set: Iterable<string>, name: string){
        let i = 0;
        
        // @ts-ignore
        for (let value of set){
            formData.append(`${name}[]`, value);
            i++;
        }
    }
    
    async function trySaveFilm(): Promise<void>{
        if (!checkFormOnValid()){
            return;
        }
        let data = generateRequestData();
        
        $api.post("/films/add", data).then((response: AxiosResponse<Result<string>>) => {
            if (!response.data.success){
                setErrorMessage(response.data.textError ??
                    "Произошла неизвестная ошибка, перезагрузите страницу и попробуйте снова");
                setErrorMessageIsShow(true);
                return;
            }
            
            resetValueFields();
            setMessageAboutAddIsShow(true);
            console.log(response);
        })
    }
    
    function resetValueFields(){
        setPreview(null);
        setFile(undefined);
        selectedActors.current.clear();
        selectedProducers.current.clear();
        selectedGenres.current.clear();
        title.resetInput();
        iframeLink.resetInput();
        description.resetInput();
        setIsFree(false);
        year.resetInput();
        setSelectedCountry("Выберете страну");
    }
    
    function checkFormOnValid(): boolean{
        return title.validationState.inputIsValid && iframeLink.validationState.inputIsValid
        && description.validationState.inputIsValid && year.validationState.inputIsValid;
    }
    
    return (
        <>
            <Modal
                size="sm"
                show={errorMessageIsShow}
                onHide={() => setErrorMessageIsShow(false)}
                aria-labelledby="example-modal-sizes-title-sm"
            >
                <Modal.Header closeButton className={filmFormClasses.errorMessageHeader}>
                    <Modal.Title id="example-modal-sizes-title-sm" >
                        Ошибка
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>{errorMessage}</Modal.Body>
            </Modal>
            <ToastContainer className="p-3" position="top-center">
                <Toast show={messageAboutAddIsShow} bg="dark" onClose={() => setMessageAboutAddIsShow(false)}>
                    <Toast.Header>
                        <strong className="me-auto">TimMovie</strong>
                    </Toast.Header>
                    <Toast.Body className="text-white">Фильм успешно добавлен</Toast.Body>
                </Toast>
            </ToastContainer>
            <div className="d-flex">
                <UploadFiles
                    uploadProps={{photoWidth: 234, photoHeight:360, borderRadius: 10}}
                    uploadHooks={{preview:preview, setPreview: setPreview, setFile: setFile}}/>
                <form className={classNameConcat("d-flex flex-column flex-grow-1 mt-1", filmFormClasses.formWithFieldsFilm)}>
                    <div className="d-flex justify-content-between">
                        <div>
                            <InputWithValidation inputInfo={title} label="Название"
                                                 inputClasses={"mt-2 form-control"} typeInput="text"/>
                        </div>
                        <div className="col-6">
                            <InputWithValidation inputInfo={iframeLink} label="Ссылка на iframe"
                                inputClasses={"mt-2 form-control"} typeInput="text"/>
                        </div>
                    </div>
                    <div className="mt-4">
                        <TextareaWithValidation inputInfo={description} label="Описание"
                                                inputClasses={classNameConcat("mt-2 form-control", 
                                                    filmFormClasses.filmDescription)}/>
                    </div>
                    <div className="d-flex justify-content-between mt-4">
                        <div>
                            <DropdownWithMultipleSelector pagination={30} values={selectedActors}
                                                          urlRequestForEntity={"/actors/collection"}
                                                          title="Выберете актеров"/>
                        </div>
                         <DropdownWithMultipleSelector pagination={30} values={selectedProducers}
                                                       urlRequestForEntity={"/producers/collection"} 
                                                       title="Выберете продюсеров"/>
                        <DropdownWithMultipleSelector pagination={30} values={selectedGenres}
                                                      urlRequestForEntity={"/genres/collection"}
                                                      title="Выберете жанр"/>
                    </div>
                    <div className="d-flex justify-content-between mt-4">
                        <div className="col-4">
                            <InputWithValidation inputInfo={year} label="Год"
                                                 inputClasses={"mt-2 form-control"} typeInput="number"/>
                        </div>
                        <div className="col-3">
                            <div className="mb-2">Страна</div>
                            <DropdownSearchOneValue pagination={30} value={selectedCountry}
                                                    setValue={setSelectedCountry}
                                                          urlRequestForEntity={"/countries/collection"}
                                                          title={selectedCountry}/>
                        </div>
                    </div>
                    <div className="d-flex justify-content-between mt-4 align-items-center">
                        <div className="d-flex ">
                            <div>Бесплатный?</div>
                            <input type="checkbox" checked={isFree} onChange={event => setIsFree(event.target.checked)} className="form-check ms-2" />
                        </div>
                    </div>
                    <div className="d-flex justify-content-center">
                        <button disabled={!formIsValid}
                                className="btn btn-outline-success mt-4 col-3"
                                type="button"
                        onClick={trySaveFilm}> Сохранить</button>
                    </div>
                </form>
            </div>
        </>
    );
}

export default AddFilmForm;