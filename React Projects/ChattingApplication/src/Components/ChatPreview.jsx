import React, { useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { setClickedUser, setPrevClickedUser } from "../Slices/UserClickedSlice";
import { useSignalR } from "./SignalRProvider";
import Modal from "react-modal";

const ChatPreview = () => {
  const { connection } = useSignalR();
  const dispatch = useDispatch();
  const { clientData, allMessages } = useSelector((state) => state.messages);
  const [userChecked, setUserChecked] = useState({});
  const [groupUsersSelected, setGroupUsersSelected] = useState([]);
  const [groupNameInModal, setGroupNameInModal] = useState("")
  const { prevClickedUser, clickedUser } = useSelector(
    (state) => state.userSelected
  );
  const { isCreateGroupButtonEnabled, isUserSelectionEnabled } = useSelector(
    (state) => state.groupSetting
  );
  const {userId} = useSelector(state => state.messages);
  const [modalIsOpen, setModalIsOpen] = useState(false);

  const handleButtonClicked = ({ key, value }) => {
    dispatch(setPrevClickedUser(clickedUser));
    dispatch(setClickedUser({ key, value }));
  };

  function handleUserSelectionForModal(key) {
    setUserChecked((prevState) => {
      const updatedState = { ...prevState, [key]: !prevState[key] };
      console.log("updatedState[key]: " + updatedState[key]);
      if (updatedState[key]) {
        setGroupUsersSelected((prevGroupUsers) => [...prevGroupUsers, key]);
        console.log("selected group user " + key);
      } else {
        setGroupUsersSelected((prevGroupUsers) =>
          prevGroupUsers.filter((user) => user !== key)
        );
        console.log("Deselected group user: " + key);
      }
      return updatedState;
    });
  }

  async function createGroupOfUsersBasedOnGroupNameAndUsersSelected(groupUsersSelected, groupNameInModal){
    if(connection.current.state === "Connected"){
      await connection.current.invoke("StartGroupChat", connection.current.connectionId, JSON.stringify(groupUsersSelected)).catch((e) => console.log("Exception occured while invoking StartGroupChat "+ e))
    }
  }

  return (
    <div className="h-screen w-[40vw] flex flex-col bg-neutral-800 relative">
      {/* Search */}
      <div className="p-4 flex items-center pt-10" style={{ height: "65px" }}>
        <div className="flex items-center bg-neutral-600 rounded-md px-4 py-[10px] w-full">
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
            className="px-5 py-4 hover:bg-neutral-700 flex items-center gap-5 relative cursor-pointer transition duration-300 ease-in-out"
            onClick={() => handleButtonClicked({ key, value })}
          >
            <div className="relative w-[70px] h-[70px]">
              <img
                className="w-full h-full rounded-full object-contain shadow-md"
                src="https://tools-api.webcrumbs.org/image-placeholder/70/70/avatars/1"
                alt="avatar"
                onClick={(e) => {
                  e.stopPropagation(); // Prevent triggering parent onClick
                  isUserSelectionEnabled && handleUserSelectionForModal(key);
                }}
              />
              {userChecked[key] && ( // Check if this user is selected
                <div className="absolute inset-0 bg-black bg-opacity-80 rounded-full flex justify-center items-center">
                  <span
                    className="material-symbols-outlined text-white text-3xl"
                    onClick={() => handleUserSelectionForModal(key)}
                  >
                    check
                  </span>
                </div>
              )}
            </div>

            <div className="text-neutral-50 flex-1">
              <div className="font-title font-semibold">
                {key === connection.current.connectionId
                  ? ` ${value.userName} (You)`
                  : value.userName}
              </div>
              <div className="text-neutral-400 text-base truncate">
                {allMessages.length > 0 &&
                  allMessages[allMessages.length - 1].content}
              </div>
            </div>
            <div className="text-neutral-400 text-base">
              {allMessages.length > 0 &&
                allMessages[allMessages.length - 1].timestamp}
            </div>

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

      {/* Button at the Bottom */}
      {isCreateGroupButtonEnabled && (
        <button
          className="absolute bottom-5 right-5 bg-teal-500 text-white px-6 py-2 rounded-lg shadow-md flex items-center justify-center"
          onClick={() => setModalIsOpen(true)}
        >
          <span className="material-symbols-outlined">group_add</span>
          <span className="ml-2">Create Group</span>
        </button>
      )}

      <Modal
        isOpen={modalIsOpen}
        onRequestClose={() => setModalIsOpen(false)}
        style={{
          overlay: {
            backgroundColor: "rgba(0, 0, 0, 0.5)", // Dim background
          },
          content: {
            top: "50%",
            left: "50%",
            right: "auto",
            bottom: "auto",
            margin: "0", // No margin around modal
            padding: "0", // Remove default padding
            transform: "translate(-50%, -50%)", // Center the modal
            border: "none", // Optional: Remove default border if undesired
            background: "none", // Set transparent to rely on your custom styles
          },
        }}
      >
        <div id="webcrumbs">
          <div className="w-screen h-screen flex justify-center items-center bg-black bg-opacity-50">
            {" "}
            <div className="w-[400px] h-auto bg-neutral-800 rounded-lg shadow-lg p-6 flex flex-col justify-center items-center">
              <h1 className="text-teal-500 text-2xl font-title mb-6">
                Create Group
              </h1>
              <div className="relative mb-4">
                <img
                  src="https://tools-api.webcrumbs.org/image-placeholder/80/80/avatars/4"
                  className="object-contain rounded-full w-[80px] h-[80px]"
                  alt="User Avatar"
                />
              </div>
              <div className="mb-6 w-full">
                <label
                  className="block text-neutral-50 mb-2"
                  htmlFor="username"
                >
                  Group Name
                </label>
                <input
                  id="username"
                  type="text"
                  className="w-full p-2 bg-neutral-700 border border-neutral-600 text-neutral-50 rounded-full"
                  placeholder="Give a group name"
                  onChange={(e) => setGroupNameInModal(e)}
                />
              </div>
              <div className="flex w-full gap-4">
                <button className="w-full bg-teal-500 text-primary-50 p-2 rounded-full" onClick={() => createGroupOfUsersBasedOnGroupNameAndUsersSelected(groupUsersSelected, groupNameInModal)}>
                  Create
                </button>
                <button
                  className="w-full bg-neutral-600 text-neutral-50 p-2 rounded-full"
                  onClick={() => setModalIsOpen(false)}
                >
                  Cancel
                </button>
              </div>
            </div>
          </div>
        </div>
      </Modal>
    </div>
  );
};

export default ChatPreview;
