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
      .withUrl("https://localhost:7219/realtimehub?userId=vaibhav")
      .build();

    setConn(connection);

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
        <aside className="bg-neutral-800 min-h-screen p-4 resize-l overflow-auto w-[250px]">
          {/* Searchbar */}
          <div className="mb-4">
            <input
              type="text"
              placeholder="Search..."
              className="w-full p-2 bg-neutral-700 text-neutral-300 border-none rounded-md focus:ring-2 focus:ring-teal-400"
            />
          </div>

          <ul className="flex flex-col gap-2">
            {/* Chat preview item - John Doe */}
            <li className="flex items-center gap-2 p-2 hover:bg-neutral-600 rounded-md cursor-pointer h-[70px]">
              <img
                src="https://tools-api.webcrumbs.org/image-placeholder/40/40/avatars/1"
                alt="User"
                className="rounded-full w-[40px] h-[40px] object-cover"
              />
              <div className="flex flex-col justify-center">
                <p className="text-neutral-300 text-sm font-bold">John Doe</p>
                <span className="text-xs text-neutral-400 whitespace-nowrap overflow-hidden text-ellipsis w-[180px]">
                  Hey, can we discuss the meeting agenda?
                </span>
              </div>
            </li>
            <hr className="border-neutral-600" />

            {/* Chat preview item - Jane Smith */}
            <li className="flex items-center gap-2 p-2 hover:bg-neutral-600 rounded-md cursor-pointer h-[70px]">
              <img
                src="https://tools-api.webcrumbs.org/image-placeholder/40/40/avatars/2"
                alt="User"
                className="rounded-full w-[40px] h-[40px] object-cover"
              />
              <div className="flex flex-col justify-center">
                <p className="text-neutral-300 text-sm font-bold">Jane Smith</p>
                <span className="text-xs text-neutral-400 whitespace-nowrap overflow-hidden text-ellipsis w-[180px]">
                  Sure, I'll get that fixed for you...
                </span>
              </div>
            </li>
            <hr className="border-neutral-600" />

            {/* Chat preview item - Alice Cooper */}
            <li className="flex items-center gap-2 p-2 hover:bg-neutral-600 rounded-md cursor-pointer h-[70px]">
              <img
                src="https://tools-api.webcrumbs.org/image-placeholder/40/40/avatars/3"
                alt="User"
                className="rounded-full w-[40px] h-[40px] object-cover"
              />
              <div className="flex flex-col justify-center">
                <p className="text-neutral-300 text-sm font-bold">
                  Alice Cooper
                </p>
                <span className="text-xs text-neutral-400 whitespace-nowrap overflow-hidden text-ellipsis w-[180px]">
                  Thank you for the help!...
                </span>
              </div>
            </li>
          </ul>
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
