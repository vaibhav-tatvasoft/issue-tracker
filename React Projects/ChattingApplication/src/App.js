import React from "react";
import SignalR from "./Components/SignalR";
import UserRegistration from "./Components/UserRegistration"

/* Don't forget to download the CSS file too 
OR remove the following line if you're already using Tailwind */

export default function App () {
  return (
    <div>
      {/* <UserRegistration /> */}
      <SignalR />
    </div>
    // <div id="webcrumbs">
    //   <div className="w-full bg-neutral-800 rounded-lg flex">
    //     {" "}
    //     <nav className="bg-neutral-900 w-[60px] h-screen flex flex-col items-center py-4">
    //       <div className="w-[40px] h-[40px] bg-teal-600 rounded-full flex items-center justify-center mb-6">
    //         <i className="fa-solid fa-message text-white"></i>
    //       </div>
    //       <div className="flex flex-col gap-8">
    //         <button className="w-[40px] h-[40px] bg-neutral-700 rounded-full flex items-center justify-center">
    //           <span className="material-symbols-outlined text-neutral-50">
    //             home
    //           </span>
    //         </button>
    //         <button className="w-[40px] h-[40px] bg-neutral-700 rounded-full flex items-center justify-center">
    //           <span className="material-symbols-outlined text-neutral-50">
    //             group
    //           </span>
    //         </button>
    //         <button className="w-[40px] h-[40px] bg-neutral-700 rounded-full flex items-center justify-center">
    //           <span className="material-symbols-outlined text-neutral-50">
    //             settings
    //           </span>
    //         </button>
    //       </div>
    //     </nav>
    //     <aside className="w-[400px] bg-neutral-900 flex flex-col items-center py-4">
    //       <div className="w-[80px] h-[80px] bg-neutral-700 rounded-full mb-4 flex items-center justify-center">
    //         <i className="fa-solid fa-message text-white text-xl"></i>
    //       </div>
    //       <div className="flex flex-col items-start w-full px-2 mb-8">
    //         <input
    //           type="text"
    //           placeholder="Search"
    //           className="bg-neutral-700 w-full h-[40px] rounded-full text-neutral-400 px-4"
    //         />
    //       </div>
    //       <div className="flex items-center justify-between w-full px-4 mb-4">
    //         <span className="text-neutral-400">Chats</span>
    //         <div className="flex items-center gap-2">
    //           <span className="material-symbols-outlined text-white">add</span>
    //           <span className="material-symbols-outlined text-white">
    //             more_vert
    //           </span>
    //         </div>
    //       </div>
    //       <div className="w-full px-2 mb-4 flex gap-2">
    //         <button className="px-4 py-1 bg-teal-600 rounded-full text-white">
    //           All
    //         </button>
    //         <button className="px-4 py-1 bg-neutral-700 rounded-full text-neutral-400">
    //           Unread
    //         </button>
    //         <button className="px-4 py-1 bg-neutral-700 rounded-full text-neutral-400">
    //           Favourites
    //         </button>
    //         <button className="px-4 py-1 bg-neutral-700 rounded-full text-neutral-400">
    //           Groups
    //         </button>
    //       </div>
    //       <div className="w-full text-neutral-400 px-4 mb-4">Locked chats</div>
    //       <div className="w-full flex flex-col gap-4 px-2">
    //         <div className="flex items-center justify-between">
    //           <div className="flex items-center gap-4">
    //             <img
    //               src="https://tools-api.webcrumbs.org/image-placeholder/40/40/avatars/1"
    //               className="w-[40px] h-[40px] rounded-full object-contain"
    //             />
    //             <div>
    //               <p className="text-neutral-50"> Name</p>
    //               <p className="text-neutral-400">message</p>
    //             </div>
    //           </div>
    //           <div className="text-teal-400 text-sm">16:30</div>
    //         </div>
    //       </div>
    //     </aside>
    //     <main className="flex-1 bg-neutral-800 flex flex-col items-center justify-center py-10">
    //       <div className="relative w-[300px] h-[200px] mb-8">
    //         <img
    //           src="https://tools-api.webcrumbs.org/image-placeholder/300/200/abstract/20"
    //           className="w-[300px] h-[200px] object-cover rounded-lg"
    //         />
    //         <img
    //           src="https://cdn.webcrumbs.org/assets/images/ask-ai/qrcode.png"
    //           className="w-[50px] h-[50px] absolute bottom-0 right-[120px]"
    //         />
    //       </div>
    //       <h1 className="font-title text-neutral-50 text-3xl mb-4">
    //         Download WhatsApp for Windows
    //       </h1>
    //       <p className="text-neutral-50 mb-6 text-center">
    //         Make calls, share your screen and get a faster experience when you
    //         download the Windows app.
    //       </p>
    //       <button className="bg-teal-600 px-8 py-3 text-neutral-50 rounded-full">
    //         Get from Microsoft Store
    //       </button>
    //       <p className="text-neutral-400 mt-6">
    //         <i className="fa-solid fa-lock"></i> Your personal messages are
    //         end-to-end encrypted
    //       </p>
    //     </main>
    //   </div>
    // </div>
  );
};
