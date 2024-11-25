import React, { useState } from "react";
import Modal from "react-modal";

const CreateGroup = () => {
  const [modalIsOpen, setModalIsOpen] = useState(false);

  return (
    <div>
      <button
        onClick={() => setModalIsOpen(true)}
        className="bg-teal-500 text-white p-2 rounded-lg"
      >
        Open Modal
      </button>

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
            <label className="block text-neutral-50 mb-2" htmlFor="username">
              Group Name
            </label>
            <input
              id="username"
              type="text"
              className="w-full p-2 bg-neutral-700 border border-neutral-600 text-neutral-50 rounded-full"
              placeholder="Give a group name"
            />
          </div>
          <div className="flex w-full gap-4">
            <button className="w-full bg-teal-500 text-primary-50 p-2 rounded-full">
              Create
            </button>
            <button className="w-full bg-neutral-600 text-neutral-50 p-2 rounded-full">
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

export default CreateGroup;
