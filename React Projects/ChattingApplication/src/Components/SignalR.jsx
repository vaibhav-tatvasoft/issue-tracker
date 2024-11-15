import React from "react";
import { useState, useEffect } from "react";
import * as signalR from "@microsoft/signalr";
import "./SignalR.css";
import VerticalNavBar from "./VerticalNavBar";
import ChatPreview from "./ChatPreview";

const SignalR = () => {
  const [outMessages, setOutMessages] = useState([]);
  const [inMessages, setInMessages] = useState([]);
  const [outMessage, setOutMessage] = useState("");
  const [conn, setConn] = useState({}); //{connection object}
  const [clientData, setClientData] = useState({}); // {ConnectionId, UserIdentifier}
  const [selectedUserFromChild, setSelectedUserFromChild] = useState({}); // {ConnectionId, UserIdentifier} from ChatPreview.jsx
  const [userId, setUserId] = useState("");
  const [incomingMessageObject, setIncomingMessageObject] = useState({}); //{content, timestamp, to, from}
  const [groupName, setGroupName] = useState();
  const [currentChatSessionDetails, setCurrentChatSessionDetails] = useState(
    {}
  );
  const [allMessagesObject, setAllMessagesObject] = useState([]);

  const fetchClickedUserFromChild = ({ key, value }) => {
    setSelectedUserFromChild({ key, value });
  };

  useEffect(() => {
    if (Object.keys(selectedUserFromChild).length > 0) {
      console.log(
        "🚀 ~ SignalR ~ selectedUserFromChild:",
        selectedUserFromChild
      );
      StartPrivateChat();
    }
  }, [selectedUserFromChild]);

  useEffect(() => {
    const urlParams = new URLSearchParams(window.location.search);
    console.log("🚀 ~ useEffect ~ urlParams:", urlParams.get("userId"));
    const uid = urlParams.get("userId");
    setUserId(uid);

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`https://localhost:7219/realtimehub?userId=${uid}`)
      .build();

    setConn(connection);

    connection.on("ReceiveAllClientsList", (data) => {
      console.log(
        "🚀 ~ connection.on ReceiveAllClientsList ~ data:",
        JSON.stringify(data)
      );
      setClientData(data);
      //console.log(`Incoming connected clients from hub:  ${JSON.stringify(data)}`);
    });

    connection.on("ReceiveGroupName", (groupName) => {
      console.log("GroupName : " + groupName);
      setGroupName(groupName);
    });

    connection.on("ReceiveMessage", (fromUser, messageObject) => {
      console.log(
        "🚀 ~ connection.on ReceiveMessage ~ fromUser, messageObject:",
        fromUser,
        JSON.stringify(messageObject)
      );
      //console.log(`Incoming Message from client/hub ${fromUser} : ${JSON.stringify(messageObject)}`);

      setAllMessagesObject((prev) => [...prev, messageObject]);

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
  }, []);

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

  useEffect(() => {
    console.log(`Outgoing message : ${JSON.stringify(outMessages)}`);
  }, [inMessages]);

  // SEND MESSAGE FUNCTION
  async function sendMessage(messageObject) {
    console.log(
      `Sending message to hub with details : ${JSON.stringify(messageObject)}`
    );
    await conn
      .invoke("SendMessage", messageObject)
      .catch((err) => console.error(err));
  }

  //Create a group on server side and return groupName
  async function StartPrivateChat() {
    console.log("🚀 ~ StartPrivateChat ~ Entered the method:");

    await conn
      .invoke("StartPrivateChat", conn.connectionId, selectedUserFromChild.key)
      .catch((e) =>
        console.log("Error occured while invoking StartPrivateChat " + e)
      );
  }

  useEffect(() => {
    Conversation();
  }, [incomingMessageObject]);

  async function Conversation() {
    // build an object with all details of current chat
    //groupName, message, some sort of boolean to verify that chat is active
    setCurrentChatSessionDetails({
      groupName: groupName,
      messageObj: incomingMessageObject,
    });
  }

  return (
    <div id="webcrumbs">
      <div className="w-full min-h-screen bg-black flex">
        {" "}
        <VerticalNavBar />
        <ChatPreview
          clientData={clientData}
          connContext={conn}
          sendSelectedUser={fetchClickedUserFromChild}
        />
        <div className="w-full min-h-screen bg-neutral-900 shadow-md flex flex-col">
          <header className="bg-neutral-700 p-4 text-neutral-50 flex items-center">
            <i className="fa-brands fa-facebook text-teal-500"></i>
            <h1 className="ml-3 font-title font-bold text-lg">
              {Object.keys(selectedUserFromChild) === 0
                ? "Chat"
                : selectedUserFromChild.value}
            </h1>
          </header>
          <main className="flex-1 p-4 overflow-y-auto">
            {allMessagesObject
              .filter(
                (object) =>
                  object.from === selectedUserFromChild.value ||
                  object.to === selectedUserFromChild.value
              )
              .map((object, index) => {
                return (
                  <div
                    key={index}
                    className={
                      object.type === "incoming"
                        ? "mb-4 flex justify-start"
                        : "mb-4 flex justify-end"
                    }
                  >
                    <div
                      className={
                        object.type === "incoming"
                          ? "bg-neutral-700 text-neutral-50 p-3 rounded-md inline-block max-w-[60%]"
                          : "bg-teal-500 text-white p-3 rounded-md inline-block max-w-[60%]"
                      }
                    >
                      <p>{object.content}</p>
                      <span className="text-xs text-neutral-500 mt-2 inline-block">
                        {object.timestamp}
                      </span>
                      &nbsp;&nbsp;
                      {object.type === "outgoing" && (
                        <i className="fa fa-check-double text-xs text-neutral-200"></i>
                      )}
                    </div>
                  </div>
                );
              })}
          </main>
          <footer className="p-4 border-t border-neutral-700 flex items-center gap-3">
            <input
              type="text"
              value={outMessage}
              placeholder="Type a message"
              className="flex-1 p-3 border border-neutral-700 bg-neutral-800 text-neutral-50 rounded-md"
              onChange={(chat) => {
                const newMessage = chat.target.value;
                setOutMessage(newMessage);
              }}
              onKeyDown={(e) => {
                if (e.key === "Enter" && selectedUserFromChild.value) {
                  setOutMessages((prevMessages) => [
                    ...prevMessages,
                    outMessage,
                  ]);

                  setOutMessage("");

                  const messageObject = {
                    type: "outgoing",
                    content: outMessage,
                    to: selectedUserFromChild.value,
                    from: userId,
                    groupName,
                  };

                  setAllMessagesObject((prev) => [...prev, messageObject]);

                  return sendMessage(JSON.stringify(messageObject));
                }
              }}
            />
            <button
              className="bg-teal-500 text-white w-[50px] h-[50px] p-3 rounded-full flex items-center justify-center"
              onClick={() => {
                setOutMessages((prevMessages) => [...prevMessages, outMessage]);

                return sendMessage(
                  JSON.stringify({
                    content: outMessage,
                    to: selectedUserFromChild.value,
                    from: userId,
                    groupName,
                  })
                );
              }}
            >
              <span className="material-symbols-outlined">send</span>
            </button>
          </footer>
        </div>
      </div>
    </div>
  );
};

export default SignalR;
