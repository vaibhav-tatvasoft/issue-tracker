import React from "react";

const ChatPreview = ({ clientData , connContext}) => {
  return (
    <div>
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
          {Object.entries(clientData).map(([key, value], index) => {
            return (
              <li key={key} className="flex items-center gap-2 p-2 hover:bg-neutral-600 rounded-md cursor-pointer h-[70px]">
                <img
                  src="https://tools-api.webcrumbs.org/image-placeholder/40/40/avatars/1"
                  alt="User"
                  className="rounded-full w-[40px] h-[40px] object-cover"
                />
                <div className="flex flex-col justify-center">
                  <p className="text-neutral-300 text-sm font-bold">{key === connContext.ConnectionId ? `${value} (You)` : value}</p>
                  <span className="text-xs text-neutral-400 whitespace-nowrap overflow-hidden text-ellipsis w-[180px]">
                    Hey, can we discuss the meeting agenda?
                  </span>
                </div>
              </li>
            );
          })}

          <hr className="border-neutral-600" />
        </ul>
      </aside>
    </div>
  );
};

export default ChatPreview;
