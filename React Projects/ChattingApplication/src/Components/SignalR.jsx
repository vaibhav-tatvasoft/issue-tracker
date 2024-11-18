import React from "react";
import { useState, useEffect } from "react";
import * as signalR from "@microsoft/signalr";
import "./SignalR.css";
import VerticalNavBar from "./VerticalNavBar";
import ChatPreview from "./ChatPreview";

import { useSelector, useDispatch } from "react-redux";
import {
  setClientData,
  setMessage,
  setGroupName,
  setUserId,
  setConn,
} from "../Slices/MessageSlice";
import ChatWindow from "./ChatWindow";
import SendMessageFooter from "./SendMessageFooter";

const SignalR = () => {
  const dispatch = useDispatch();
  const { clientData, conn } = useSelector((state) => state.messages);
  const clickedUser = useSelector(
    (state) => state.userSelected.clickedUser || {}
  );

  const [inMessages, setInMessages] = useState([]);
  const [incomingMessageObject, setIncomingMessageObject] = useState({}); //{content, timestamp, to, from}
  const [currentChatSessionDetails, setCurrentChatSessionDetails] = useState(
    {}
  );

  useEffect(() => {
    if (clickedUser && Object.keys(clickedUser).length > 0) {
      console.log("🚀 ~ SignalR ~ clickedUserFromChild:", clickedUser);
      StartPrivateChat();
    }
  }, [clickedUser]);

  useEffect(() => {
    const urlParams = new URLSearchParams(window.location.search);
    console.log("🚀 ~ useEffect ~ urlParams:", urlParams.get("userId"));
    const uid = urlParams.get("userId");

    dispatch(setUserId(uid));

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`https://localhost:7219/realtimehub?userId=${uid}`)
      .build();

    dispatch(setConn(connection));

    connection.on("ReceiveAllClientsList", (data) => {
      console.log(
        "🚀 ~ connection.on ReceiveAllClientsList ~ data:",
        JSON.stringify(data)
      );

      dispatch(setClientData(data));
    });

    connection.on("ReceiveGroupName", (groupName) => {
      console.log("GroupName : " + groupName);
      dispatch(setGroupName(groupName));
    });

    connection.on("ReceiveMessage", (fromUser, messageObject) => {
      console.log(
        "🚀 ~ connection.on ReceiveMessage ~ fromUser, messageObject:",
        fromUser,
        JSON.stringify(messageObject)
      );

      dispatch(setMessage(messageObject));

      setIncomingMessageObject(messageObject);

      setInMessages((prevMessage) => [...prevMessage, messageObject.content]);
    });

    connection.onclose(start);

    start();

    async function start() {
      try {
        await connection.start();

        console.log("🚀 ~ start ~ connection successful:");

        //console.log("connection successful");
        console.log("Requesting list of all clients from hub...");
        connection.invoke("SendAllClientsList", [connection.connectionId]);
      } catch (error) {
        console.log("connection failure :" + error);
      }
    }

    return () => {
      connection.off("ReceiveMessage");
      connection.stop();
    };
  }, [dispatch]);

  useEffect(() => {
    if (clientData && Object.keys(clientData).length > 0) {
      Object.entries(clientData).forEach(([key, value]) => {
        console.log(`Connection ID: ${key}, UserIdentifier: ${value}`);
      });
    }
  }, [clientData]);

  useEffect(() => {
    console.log(`Incoming message : ${JSON.stringify(inMessages)}`);
  }, [inMessages]);

  //Create a group on server side and return groupName
  async function StartPrivateChat() {
    console.log("🚀 ~ StartPrivateChat ~ Entered the method:");

    await conn
      .invoke("StartPrivateChat", conn.connectionId, clickedUser.key)
      .catch((e) =>
        console.log("Error occured while invoking StartPrivateChat " + e)
      );
  }

  // useEffect(() => {
  //   Conversation();
  // }, [incomingMessageObject]);

  // async function Conversation() {
  //   // build an object with all details of current chat
  //   //groupName, message, some sort of boolean to verify that chat is active
  //   setCurrentChatSessionDetails({
  //     groupName: groupName,
  //     messageObj: incomingMessageObject,
  //   });
  // }

  return (
    <div id="webcrumbs">
  <div
    className="w-full min-h-screen bg-cover bg-center flex"
  >
    <VerticalNavBar />
    <ChatPreview />
    <div className="w-full min-h-screen bg-white/90 shadow-md flex flex-col">
      <header className="bg-neutral-100 p-4 text-neutral-900 flex items-center border-b border-neutral-300">
        <i className="fa-brands fa-facebook text-teal-500"></i>
        <h1 className="ml-3 font-title font-bold text-lg">
          {clickedUser && Object.keys(clickedUser).length === 0
            ? "Select a chat"
            : clickedUser.value}
        </h1>
      </header>
      <ChatWindow />
      <SendMessageFooter />
    </div>
  </div>
</div>

  );
};

export default SignalR;