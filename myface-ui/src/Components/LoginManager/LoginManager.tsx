import React, {createContext, ReactNode, useState} from "react";

export const LoginContext = createContext({
    isLoggedIn: false,
    isAdmin: false,
    username: "",
    password: "",
    logIn: (username: string, password: string) => {},
    logOut: () => {},
});

interface LoginManagerProps {
    children: ReactNode
}

export function LoginManager(props: LoginManagerProps): JSX.Element {
    const [loggedIn, setLoggedIn] = useState(true);
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    
    function logIn(username: string, password: string) {
        console.log('username in LoginManager', username)
        console.log('password in LoginManager', password)
        setLoggedIn(true);
        setUsername(username);
        setPassword(password);
    }
    
    function logOut() {
        setLoggedIn(false);
        setUsername("");
        setPassword("");
    }
    
    const context = {
        isLoggedIn: loggedIn,
        isAdmin: loggedIn,
        username: username,
        password: password,
        logIn: logIn,
        logOut: logOut,
    };
    
    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}