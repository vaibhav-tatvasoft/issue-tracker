import React from "react";
import { useSelector } from "react-redux";

const Message = () => {
  const allMessages = useSelector(
    (state) => state.messages.allMessages
  );
  const clickedUser = useSelector(
    (state) => state.userSelected.clickedUser || {}
  );

  return (
    <main className="flex-1 p-4 overflow-y-auto bg-white text-neutral-900"
    style={{
      backgroundImage: "url('https://i.pinimg.com/originals/3d/f4/37/3df437922930cf2e2cbbe9f5b22132d3.jpg')",
      backgroundSize: "cover",   // Ensure the image covers the whole area
      backgroundPosition: "center",  // Center the image within the container
      backgroundColor: "#2596be", // Fallback color in case image fails
    }}>
      {allMessages
        .filter(
          (object) =>
            object.from === clickedUser.value || object.to === clickedUser.value
        )
        .map((object, index) => {
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
                    ? "bg-neutral-100 text-neutral-900 p-3 rounded-md inline-block max-w-[60%] border border-neutral-300"
                    : "bg-teal-500 text-white p-3 rounded-md inline-block max-w-[60%]"
                }
              >
                <p>{object.content}</p>
                <span className="text-xs text-neutral-600 mt-2 inline-block">
                  {object.timestamp}
                </span>
                &nbsp;&nbsp;
                {object.type === "outgoing" && (
                  <i className="fa fa-check-double text-xs text-neutral-300"></i>
                )}
              </div>
            </div>
          );
        })}
    </main>
  );
};

export default Message;
