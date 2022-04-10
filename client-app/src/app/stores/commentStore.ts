import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { makeAutoObservable, runInAction } from "mobx";
import { ChatComment } from "../models/comment";
import { store } from "./store";

export default class CommentStore {
    comments: ChatComment[] = [];
    hubConnection: HubConnection | null = null; // represents a connection to a SignalR Hub

    constructor() {
        makeAutoObservable(this);
    }

    createHubConnection = (activityId: string) => {
        if (store.activityStore.selectedActivity) {
            this.hubConnection = new HubConnectionBuilder()
                .withUrl('http://localhost:5000/chat?activityId=' + activityId, {
                    accessTokenFactory: () => store.userStore.user?.token!
                })
                .withAutomaticReconnect()
                .configureLogging(LogLevel.Information)
                .build();

            this.hubConnection.start().catch(error => 
                console.log('Error was occured while establishing a connection: ', error));

            this.hubConnection.on("LoadComments", (comments: ChatComment[]) => {
                runInAction(() => {
                    comments.forEach(comment => {
                        // append 'Z' in the end to turn a date into a UTC format
                        comment.createdAt = new Date(comment.createdAt + 'Z');
                    })
                    this.comments = comments;
                });
            })

            this.hubConnection.on("ReceiveComment", (comment: ChatComment) => {
                runInAction(() => {
                    // turn a createdAt property from string into a Date
                    // (to be able to use it inside a formatDistanceToNow() function in ActivityDetailedChat.tsx)
                    comment.createdAt = new Date(comment.createdAt); 
                    this.comments.unshift(comment); // unshift appends an item to the start of the array
                });
            })
        }
    }

    stopHubConnection = () => {
        this.hubConnection?.stop().catch(error => console.log("Error was occured while stopping a connection: ", error));
    }

    clearComments = () => {
        this.comments = [];
        this.stopHubConnection();
    }

    addComment = async (values: any) => {
        values.activityId = store.activityStore.selectedActivity?.id;
        try {
            // invoke a method on the server
            // name of the method must match the method name inside a ChatHub class 
            await this.hubConnection?.invoke('SendComment', values); 
        } catch (error) {
            console.log(error);
        }
    }
}