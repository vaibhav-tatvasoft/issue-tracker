import React from "react";
import { useDispatch, useSelector } from "react-redux";
import { setClickedUser } from "../Slices/UserClickedSlice";
import { useSignalR } from "./SignalRProvider";

const ChatPreview = () => {
  const { connection } = useSignalR();
  const dispatch = useDispatch();
  const { clientData, allMessages } = useSelector((state) => state.messages);

  const handleButtonClicked = ({ key, value }) => {
    dispatch(setClickedUser({ key, value }));
  };

  return (
    <div className="h-screen w-[40vw] flex flex-col bg-neutral-800">
      {/* Search */}
      <div className="p-4 bg-neutral-700">
        <div className="flex items-center bg-neutral-600 rounded-md px-4 py-[10px]">
          <span className="material-symbols-outlined text-neutral-300 mr-3">
            search
          </span>
          <input
            type="text"
            placeholder="Search..."
            className="bg-transparent focus:outline-none text-neutral-50 w-full"
          />
        </div>
      </div>

      {/* Chat List */}
      <div className="overflow-y-auto flex-1">
        {Object.entries(clientData).map(([key, value]) => (
          <div
            key={key}
            className="px-5 py-4 hover:bg-neutral-700 flex items-center gap-5 relative cursor-pointer"
            onClick={() => handleButtonClicked({ key, value })}
          >
            <img
              className="w-[70px] h-[70px] rounded-full object-contain"
              src="https://tools-api.webcrumbs.org/image-placeholder/70/70/avatars/1"
              alt="avatar"
            />
            <div className="text-neutral-50 flex-1">
              <div className="font-title">
                {key === connection.current.connectionId
                  ? ` ${value} (You)`
                  : value}
              </div>
              <div className="text-neutral-400 text-base truncate">
                Hey, can we discuss the meeting agenda?
              </div>
            </div>
            <div className="text-neutral-400 text-base">10:30 AM</div>

            {/* Larger Notification Dot */}
            {allMessages.length > 0 &&
              value === allMessages[allMessages.length - 1].from &&
              allMessages.filter((e) => e.isRead === false).length > 0 && (
                <div className="absolute right-[10px] top-[12px] w-[24px] h-[24px] bg-green-500 rounded-full border-2 border-neutral-800 flex items-center justify-center text-[12px] text-white font-bold">
                  {allMessages.filter((e) => e.isRead === false).length}
                </div>
              )}
          </div>
        ))}
      </div>
    </div>
  );
};

export default ChatPreview;
