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
  const [conn, setConn] = useState({});
  const [messageType, setMessageType] = useState([]);
  const [clientData, setClientData] = useState({});

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7219/realtimehub?userId=vaibhav")
      .build();

    setConn(connection);

    connection.on("ReceiveAllClientsList", (data) => {
      setClientData(data);
      console.log(`Incoming connected clients from hub:  ${data}`);
    });

    connection.on("ReceiveMessage", (fromUser, chat) => {
      console.log(`Incoming Message from client/hub ${fromUser} : ${chat}`);

      setInMessages((prevMessage) => [...prevMessage, chat]);

      setMessageType((prevState) => [
        ...prevState,
        {
          type: "incoming",
          data: chat,
        },
      ]);
    });

    connection.onclose(start);

    start();

    async function start() {
      try {
        await connection.start();

        console.log("connection successful");
        console.log("Requesting list of all clients from hub...");
        connection.invoke("SendAllClientsList");
      } catch (error) {
        console.log("connection failure :" + error);
      }
    }

    return () => {
      connection.off("ReceiveMessage");
      connection.stop();
    };
  }, []);

  async function sendMessage(user, message) {
    console.log(`Sending message to hub with details : ${user} , ${message}`);
    await conn
      .invoke("SendMessage", user, message)
      .catch((err) => console.error(err));
  }

  // const GetAllClients = async () => {
  //   if (conn && conn.state === signalR.HubConnectionState.Connected) {
  //     try {
  //       console.log("Requesting list of all clients from hub...");
  //       await conn.invoke("SendAllClientsList");
  //     } catch (error) {
  //       console.error("Error fetching all clients:", error);
  //     }
  //   } else {
  //     console.warn("Connection is not ready to invoke methods.");
  //   }
  // };

  // useEffect(() => {
  //   if (conn) {
  //     GetAllClients();
  //   }
  // }, [conn]);
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

  useEffect(() => {
    console.log(`All messages : ${JSON.stringify(messageType)}`);
  }, [inMessages]);

  return (
    <div id="webcrumbs">
      <div className="w-full min-h-screen bg-black flex">
        {" "}
        {/* Vertical Navbar */} <VerticalNavBar />
        <ChatPreview clientData = {clientData} connContext = {conn}/>
        {/* Main Chat Section */}
        <div className="w-full min-h-screen bg-neutral-900 shadow-md flex flex-col">
          <header className="bg-neutral-700 p-4 text-neutral-50 flex items-center">
            <i className="fa-brands fa-facebook text-teal-500"></i>
            <h1 className="ml-3 font-title font-bold text-lg">Chat</h1>
          </header>
          <main className="flex-1 p-4 overflow-y-auto">
            {messageType.map((object, index) => {
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
                    <p>{object.data}</p>
                    <span className="text-xs text-neutral-500 mt-2 inline-block">
                      {new Date().toLocaleTimeString([], {
                        hour: "2-digit",
                        minute: "2-digit",
                        hour12: false,
                      })}
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
                if (e.key === "Enter") {
                  setOutMessages((prevMessages) => [
                    ...prevMessages,
                    outMessage,
                  ]);

                  setMessageType((prevState) => [
                    ...prevState,
                    {
                      type: "outgoing",
                      data: outMessage,
                    },
                  ]);

                  setOutMessage("");

                  return sendMessage("akhil", outMessage);
                }
              }}
            />
            <button
              className="bg-teal-500 text-white w-[50px] h-[50px] p-3 rounded-full flex items-center justify-center"
              onClick={() => {
                setOutMessages((prevMessages) => [...prevMessages, outMessage]);

                setMessageType((prevState) => [
                  ...prevState,
                  {
                    type: "outgoing",
                    data: outMessage,
                  },
                ]);

                return sendMessage("akhil", outMessage);
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
