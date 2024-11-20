import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  clientData: {},
  allMessages: [],
  groupName: "",
  userId: "",
  conn: {},
  outMessages: [],
  outMessage: ""
};

const MessageSlice = createSlice({
  name: "messages",
  initialState,
  reducers: {
    setClientData(state, action) {
      state.clientData = action.payload;
    },
    setMessage(state, action) {
      state.allMessages.push(action.payload);
    },
    updateMessage(state, action){
      const updatedMessage = action.payload;
      state.allMessages = state.allMessages.map((message, index) => {
        return (message.id === updatedMessage[index].id ? updatedMessage : message)
      });
    },
    setGroupName(state, action) { 
      state.groupName = action.payload;
    },
    setUserId(state, action) {
      state.userId = action.payload;
    },
    setConn(state, action){
      state.conn = action.payload
    },
    setOutMessages(state, action){
      state.outMessages.push(action.payload)
    },
    setOutMessage(state, action){
      state.outMessage = action.payload
    }
  },
});

export const {setClientData, setMessage, setGroupName, setUserId, setConn, setOutMessages, setOutMessage,updateMessage} = MessageSlice.actions;
export default MessageSlice.reducer
