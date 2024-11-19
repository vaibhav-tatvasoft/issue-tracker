import React from "react";
import SignalR from "./Components/SignalR";
import { SignalRProvider } from "./Components/SignalRProvider";
import UserRegistration from "./Components/UserRegistration"
import { Routes, Route } from "react-router-dom";


/* Don't forget to download the CSS file too 
OR remove the following line if you're already using Tailwind */

export default function App () {
  return (
    <div>
      {/* <Routes>
        <Route path="/" element={<SignalR />} />
        <Route path="/auth" element={<UserRegistration />} />
      </Routes> */}
      {/* <UserRegistration /> */}
      <SignalRProvider>
        <SignalR />
      </SignalRProvider>
    </div>
   
  );
};
