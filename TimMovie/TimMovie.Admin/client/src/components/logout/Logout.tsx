import AuthService from "../../services/authService";
import 'bootstrap/dist/css/bootstrap.min.css';

function Logout(){
    const onlogoutOnClick = async (e:any) => {
        AuthService.logout();
        return window.location.href = '/';
    }

    return <a onClick={onlogoutOnClick} className='ms-3' style={{textDecoration:"underline", cursor:"pointer"}}>Выйти</a>
}

export default Logout