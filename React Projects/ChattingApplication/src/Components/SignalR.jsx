import React, { useRef } from "react";
import { useState, useEffect } from "react";
import * as signalR from "@microsoft/signalr";
import "./SignalR.css";
import VerticalNavBar from "./VerticalNavBar";
import ChatPreview from "./ChatPreview";
import { useSignalR } from "./SignalRProvider";

import { useSelector, useDispatch } from "react-redux";
import {
  setClientData,
  setMessage,
  setGroupName,
  updateMessage,
} from "../Slices/MessageSlice";
import {
  setCreatedGroupObject
} from "../Slices/GroupSettingSlice";
import ChatWindow from "./ChatWindow";
import SendMessageFooter from "./SendMessageFooter";

const SignalR = () => {
  const dispatch = useDispatch();
  const { allMessages, clientData } = useSelector((state) => state.messages);
  const { clickedUser, prevClickedUser } = useSelector((state) => state.userSelected);

  const clickedUserRef = useRef(clickedUser);
  const { connection, isConnected } = useSignalR();

  const [inMessages, setInMessages] = useState([]);
  const [incomingMessageLocal, setIncomingMessageLocal] = useState({});

  useEffect(() => {
    if (clickedUser && Object.keys(clickedUser).length > 0) {
      console.log("🚀 ~ SignalR ~ clickedUserFromChild:", clickedUser);
      clickedUserRef.current = clickedUser;
      StartPrivateChat();

      if (prevClickedUser !== clickedUser) {
        if (allMessages.length > 0) {
          const updatedMessages = allMessages.map((e) =>
            e.from === clickedUser.value && e.isRead === false
              ? { ...e, isRead: true }
              : e
          );
          dispatch(setMessage(updatedMessages));
        }
      }
    }
  }, [clickedUser]);

  useEffect(() => {
    if (isConnected) {

      connection.current.on("ReceiveCreatedGroupObject", (groupObject, userObj) => {
          console.log("SignalR ~ ReceiveCreatedGroupObject ~ group + userobj: "+ JSON.parse(groupObject)+ " "+ JSON.parse(userObj));

          dispatch(setCreatedGroupObject(groupObject));
        })

      connection.current.on("ReceiveAllClientsList", (data) => {
        console.log(
          "🚀 ~ connection.on ReceiveAllClientsList ~ data:",
          JSON.stringify(data)
        );

        dispatch(setClientData(data));
      });

      connection.current.on("ReceiveGroupName", (groupName) => {
        console.log("GroupName : " + groupName);
        dispatch(setGroupName(groupName));
      });

      connection.current.on("ReceiveMessage", (fromUser, messageObject) => {
        console.log(
          "🚀 ~ connection.on ReceiveMessage ~ fromUser, messageObject:",
          clickedUserRef.current.value,
          fromUser,
          JSON.stringify(messageObject)
        );

        setInMessages((prevMessage) => [...prevMessage, messageObject]);
      });

      return () => {
        connection.current.off("ReceiveMessage");
      };
    }
  }, [dispatch, isConnected]);

  useEffect(() => {
    if (clientData && Object.keys(clientData).length > 0) {
      Object.entries(clientData).forEach(([key, value]) => {
        console.log(`Connection ID: ${key}, UserIdentifier: ${value}`);
      });
    }
  }, [clientData]);

  useEffect(() => {
    console.log(`Incoming message : ${JSON.stringify(inMessages)}`);
    //if(clickedUser.value){
    if (inMessages.length > 0) {
      NotificationBadgeCheck(inMessages[inMessages.length - 1]);
    }
  }, [inMessages]);

  useEffect(() => {
    if (allMessages && allMessages.length > 0) {
      console.log("🚀 ~ SignalR ~ allMessagesArray:", allMessages);
      console.log(
        "🚀 ~ SignalR ~ allMessages count",
        allMessages.filter((e) => e.isRead === false).length
      );
    }
  }, [allMessages]);

  //Create a group on server side and return groupName
  async function StartPrivateChat() {
    console.log("🚀 ~ StartPrivateChat ~ Entered the method:");

    await connection.current
      .invoke(
        "StartPrivateChat",
        connection.current.connectionId,
        clickedUser.key
      )
      .catch((e) =>
        console.log("Error occured while invoking StartPrivateChat " + e)
      );
  }

  function NotificationBadgeCheck(messageObject) {
    console.log("🚀 ~ NotificationBadgeCheck ~ messageObject:", messageObject);
    console.log(
      "🚀 ~ NotificationBadgeCheck ~ clickedUserRef:",
      clickedUserRef.current
    );

    if (messageObject.from === clickedUserRef.current.value) {
      setIncomingMessageLocal({ ...messageObject, isRead: true });
      dispatch(setMessage({ ...messageObject, isRead: true }));
    } else {
      setIncomingMessageLocal({ ...messageObject, isRead: false });
      dispatch(setMessage({ ...messageObject, isRead: false }));
    }
  }

  return (
    <div id="webcrumbs">
      <div className="w-full min-h-screen bg-cover bg-center flex">
        <VerticalNavBar />
        <ChatPreview />
        <div className="w-full min-h-screen bg-neutral-900 shadow-md flex flex-col">
          <header className="bg-neutral-800 p-4 text-neutral-100 flex items-center border-b border-neutral-700">
            <i className="fa-brands fa-facebook text-teal-400"></i>
            <h1 className="ml-3 font-title font-bold text-lg">
              {clickedUser && Object.keys(clickedUser).length === 0
                ? "Select a chat"
                : clickedUser.value.userName}
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
