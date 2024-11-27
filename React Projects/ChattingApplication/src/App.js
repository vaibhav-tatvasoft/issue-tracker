import React from "react";
import SignalR from "./Components/SignalR";
import { SignalRProvider } from "./Components/SignalRProvider";
import UserRegistration from "./Components/UserRegistration"
import { Routes, Route } from "react-router-dom";
import CreateGroup from "./Components/CreateGroup";


/* Don't forget to download the CSS file too 
OR remove the following line if you're already using Tailwind */

export default function App () {
  return (
    <div>
      <Routes>
        <Route path="/createGroup" element={<CreateGroup />} />
        <Route path="/" element={<UserRegistration />} />
        <Route path="/SignalRProvider" element={<SignalRProvider />} />
      </Routes>
      {/* <SignalRProvider>
        <SignalR />
      </SignalRProvider> */}
    </div>
   
  );
};
