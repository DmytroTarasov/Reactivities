import { makeAutoObservable, reaction } from "mobx";
import { ServerError } from "../models/serverError";

export default class CommonStore {
    token: string | null = window.localStorage.getItem('jwt');
    appLoaded = false;
    error: ServerError | null = null;

    constructor() {
        makeAutoObservable(this);

        reaction(
            () => this.token, // reacting to 'token' (after token is changed, the reaction gets called)
            token => {
                if (token) window.localStorage.setItem('jwt', token); // set a key-value pair into a Local Storage
                else window.localStorage.removeItem('jwt');
            }
        )
    }

    setServerError = (error: ServerError) => this.error = error;

    setToken = (token: string | null) => {
        this.token = token;
    }

    setAppLoaded = () => {
        this.appLoaded = true;
    }

}