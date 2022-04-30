import $api from "../http";
import axios, {AxiosResponse} from "axios";
import {IUserDto} from "../dto/IUserDto";
import {ILoginResponseDto} from "../dto/ILoginResponseDto";
import jwtDecode from 'jwt-decode';

export default class AuthService{
    static async login(username: string, password: string): Promise<void>{
        let authResponse = await axios.post(`${process.env.REACT_APP_IDENTITY_URL}/connect/token`,{grant_type:'password', username:username, password:password})
        if(authResponse.status === 200){
            let token = authResponse.data['access_token'];
            if(token){
                const decoded: any = jwtDecode(token)
                if(decoded[`${process.env.REACT_APP_ROLE_CLAIM}`].contains('admin')) {
                    localStorage.setItem('token', token);
                }
            }
        }
    }
}