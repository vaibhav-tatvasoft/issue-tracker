import React from "react";
import { useDispatch, useSelector } from "react-redux";
import { setClickedUser } from "../Slices/UserClickedSlice";

const ChatPreview = () => {
  const dispatch = useDispatch();
  const { conn, clientData } = useSelector((state) => state.messages);

  const handleButtonClicked = ({ key, value }) => {
    dispatch(setClickedUser({ key, value }));
  };

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

        {/* User List */}
        <ul className="flex flex-col gap-4">
          {Object.entries(clientData).map(([key, value]) => (
            <li
              key={key}
              className="flex items-center gap-3 p-3 bg-neutral-700 hover:bg-neutral-600 rounded-md cursor-pointer"
              onClick={() => handleButtonClicked({ key, value })}
            >
              {/* User Avatar */}
              <img
                src="https://tools-api.webcrumbs.org/image-placeholder/40/40/avatars/1"
                alt="User"
                className="rounded-full w-[50px] h-[50px] object-cover"
              />

              {/* User Info */}
              <div className="flex-1">
                <p className="text-neutral-300 text-base font-bold truncate">
                  {key === conn.connectionId ? `${value} (You)` : value}
                </p>
                <p className="text-xs text-neutral-400 truncate">
                  Hey, can we discuss the meeting agenda?
                </p>
              </div>

              {/* Notification Icon */}
              <div className="relative flex items-center">
                <span className="material-symbols-outlined text-neutral-500">
                  chat
                </span>
                <div className="absolute top-0 right-0 w-2.5 h-2.5 bg-red-500 rounded-full"></div>
              </div>
            </li>
          ))}
        </ul>
      </aside>
    </div>
  );
};

export default ChatPreview;
