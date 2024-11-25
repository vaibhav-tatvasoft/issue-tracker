import React, { useEffect } from "react";
import { v4 as uuidv4 } from "uuid";
import { useDispatch, useSelector } from "react-redux";
import {
  setMessage,
  setOutMessage,
  setOutMessages,
} from "../Slices/MessageSlice"; 
import { useSignalR } from "./SignalRProvider";

const SendMessageFooter = () => {
  const dispatch = useDispatch();
  const { connection } = useSignalR();
  const { groupName, userId, conn, outMessage, outMessages } = useSelector(
    (state) => state.messages
  );
  const clickedUser = useSelector(
    (state) => state.userSelected.clickedUser || {}
  );
  const {user} = useSelector(state => state.users);

  useEffect(() => {
    console.log(`Outgoing message : ${JSON.stringify(outMessages)}`);
  }, [outMessages]);

  // SEND MESSAGE FUNCTION
  async function sendMessage(messageObject) {
    console.log(
      `Sending message to hub with details : ${JSON.stringify(messageObject)}`
    );
    await connection.current
      .invoke("SendMessage", messageObject)
      .catch((err) => console.error(err));
  }

  return (
    <footer className="p-4 border-t border-neutral-700 flex items-center gap-3">
      <input
        type="text"
        value={outMessage}
        placeholder="Type a message"
        className="flex-1 p-3 border border-neutral-700 bg-neutral-600 text-neutral-50 rounded-lg focus:ring-2 focus:ring-teal-500"
        onChange={(chat) => {
          const newMessage = chat.target.value;
          dispatch(setOutMessage(newMessage));
        }}
        onKeyDown={(e) => {
          if (e.key === "Enter" && clickedUser.value) {
            dispatch(
              setOutMessages((prevMessages) => [...prevMessages, outMessage])
            );
            dispatch(setOutMessage(""));
            const messageObject = {
              id: uuidv4(),
              type: "outgoing",
              content: outMessage,
              to: clickedUser.value,
              from: user.userName,
              groupName,
              timestamp: new Date().toLocaleTimeString([], {
                hour: "2-digit",
                minute: "2-digit",
                hour12: false,
              }),
            };
            dispatch(setMessage(messageObject));
            return sendMessage(JSON.stringify(messageObject));
          }
        }}
      />
      <button
        className="bg-teal-500 text-white w-[50px] h-[50px] p-3 rounded-full flex items-center justify-center hover:bg-teal-400 transition duration-300"
        onClick={() => {
          dispatch(
            setOutMessages((prevMessages) => [...prevMessages, outMessage])
          );
          const messageObject = {
            type: "outgoing",
            content: outMessage,
            to: clickedUser.value,
            from: userId,
            groupName,
            timestamp: new Date().toLocaleTimeString([], {
              hour: "2-digit",
              minute: "2-digit",
              hour12: false,
            }),
          };
          dispatch(setMessage(messageObject));
          return sendMessage(JSON.stringify(messageObject));
        }}
      >
        <span className="material-symbols-outlined">send</span>
      </button>
    </footer>
  );
};

export default SendMessageFooter;
