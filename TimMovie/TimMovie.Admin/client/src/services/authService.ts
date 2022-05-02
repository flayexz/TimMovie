import jwtDecode from 'jwt-decode';

export default class AuthService{

    static login(token: string){
        localStorage.setItem(`${process.env.REACT_APP_ACCESS_TOKEN_KEY_NAME}`, token);
    }

    static logout(){
        localStorage.removeItem(`${process.env.REACT_APP_ACCESS_TOKEN_KEY_NAME}`);
    }

    static isAdmin(token: string | null): boolean{
        if(token === null)
            return false
        const decodedToken: any = jwtDecode(token);
        const role = decodedToken[`${process.env.REACT_APP_CLAIM_ROLE}`];
        if(role === undefined || role == null)
            return false
        return role.includes('admin');
    }

    static isAdminAuth(): boolean{
        const token = localStorage.getItem(`${process.env.REACT_APP_ACCESS_TOKEN_KEY_NAME}`)
        if(token === undefined)
            return false;
        return this.isAdmin(token)
    }
}