import React, { createContext, useContext, useEffect, useRef, useState } from 'react'
import * as signalR from "@microsoft/signalr";
import { useSelector, useDispatch } from "react-redux";
import {
  setUserId
} from "../Slices/MessageSlice";

const SignalRContext = createContext(null);

export const SignalRProvider = ({children}) => {

  const [isConnected, setIsConnected] = useState(false);

    const dispatch = useDispatch();

    const connRef = useRef(null);

    useEffect(() => {
        
        const urlParams = new URLSearchParams(window.location.search);
    console.log("🚀 ~ useEffect ~ urlParams:", urlParams.get("userId"));
    const uid = urlParams.get("userId");

    dispatch(setUserId(uid));

    
        const connection = new signalR.HubConnectionBuilder()
          .withUrl(`https://localhost:7219/realtimehub?userId=${uid}`)
          .build();

          connection.onclose(start);

          start();
      
          async function start() {
            try {
              await connection.start().then(() => {
                connRef.current = connection; 
                console.log("🚀 ~ awaitconnection.start ~ connRef.current:", connRef.current)

                setIsConnected(true);

                console.log("🚀 ~ start ~ connection successful:");
                console.log("Requesting list of all clients from hub...");
                connection.invoke("SendAllClientsList", [connection.connectionId]);
              });
            } catch (error) {
              console.log("connection failure :" + error);
            }
          }
      
          return () => {
            connection.stop();
          };
        }, [dispatch]);

  return (
    <SignalRContext.Provider value = {{connection :connRef, isConnected}}>
        {children}
    </SignalRContext.Provider>
  )
}

export const useSignalR = () => useContext(SignalRContext);