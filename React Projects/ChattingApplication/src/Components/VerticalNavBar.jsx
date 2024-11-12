import React from "react";

const VerticalNavBar = () => {
  return (
    <aside className="bg-neutral-700 w-[80px] min-h-screen flex flex-col items-center p-4 gap-6">
      <i className="fa-brands fa-facebook text-teal-500 text-3xl"></i>
      <i className="material-symbols-outlined text-neutral-50 text-3xl">home</i>
      <i className="material-symbols-outlined text-neutral-50 text-3xl">chat</i>
      <i className="material-symbols-outlined text-neutral-50 text-3xl">
        settings
      </i>
    </aside>
  );
};

export default VerticalNavBar;
