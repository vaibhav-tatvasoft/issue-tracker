import React from "react";
import { useState, useEffect } from "react";
import * as signalR from "@microsoft/signalr";
import "./SignalR.css";

const SignalR = () => {
  const [outMessages, setOutMessages] = useState([]);
  const [inMessages, setInMessages] = useState([]);

  const [outMessage, setOutMessage] = useState("");
  const [conn, setConn] = useState({});

  const [messageType, setMessageType] = useState([]);

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7219/realtimehub")
      .build();

    setConn(connection);

    connection.on("ReceiveMessage", (user, chat) => {
      console.log(`Incoming Message from client/hub ${user} : ${chat}`);

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
      } catch (error) {
        console.log("connection failure :" + error);
      }
    }
  }, []);

  async function sendMessage(user, message) {
    console.log(`Sending message to hub with details : ${user} , ${message}`);
    await conn
      .invoke("SendMessage", user, message)
      .catch((err) => console.error(err));
  }

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
        {/* Vertical Navbar */}{" "}
        <aside className="bg-neutral-700 w-[80px] min-h-screen flex flex-col items-center p-4 gap-6">
          <i className="fa-brands fa-facebook text-teal-500 text-3xl"></i>
          <i className="material-symbols-outlined text-neutral-50 text-3xl">
            home
          </i>
          <i className="material-symbols-outlined text-neutral-50 text-3xl">
            chat
          </i>
          <i className="material-symbols-outlined text-neutral-50 text-3xl">
            settings
          </i>
        </aside>
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
                  <div className="bg-neutral-700 text-neutral-50 p-3 rounded-md inline-block max-w-[60%]">
                    <p>{object.data}</p>
                  </div>
                </div>
              );
            })}
            {/* <div className="mb-4 flex justify-start">
                  <div className="bg-neutral-700 text-neutral-50 p-3 rounded-md inline-block max-w-[60%]">
                    <p>Hello! How can I assist you today?</p>
                  </div>
                </div>
                <div className="mb-4 flex justify-end">
                  <div className="bg-teal-500 text-white p-3 rounded-md inline-block max-w-[60%]">
                    <p>Can you help me with my account issue?</p>
                  </div>
                </div> */}
          </main>
          <footer className="p-4 border-t border-neutral-700 flex items-center gap-3">
            <input
              type="text"
              placeholder="Type a message"
              className="flex-1 p-3 border border-neutral-700 bg-neutral-800 text-neutral-50 rounded-md"
              onChange={(chat) => {
                const newMessage = chat.target.value;
                setOutMessage(newMessage)}}
            />
            <button
              className="bg-teal-500 text-white w-[50px] h-[50px] p-3 rounded-full flex items-center justify-center"
              onClick={() =>{

                setOutMessages((prevMessages) => [...prevMessages, outMessage])
              
                setMessageType((prevState) => [
                    ...prevState,
                    {
                      type: "outgoing",
                      data: outMessage,
                    },
                  ]);

                return sendMessage(
                  conn.connectionId,
                  outMessage
                )
              }
                
              }
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
