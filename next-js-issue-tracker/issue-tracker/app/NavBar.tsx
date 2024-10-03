'use client'

import Link from "next/link";
import React from "react";
import { FaBug } from "react-icons/fa";
import classNames from 'classnames';
import { usePathname } from "next/navigation";

const NavBar = () => {
  const links = [
    { href: "/", lable: "Dashboard" },
    { href: "/issues", lable: "Issues" },
  ];

  const currentPathname = usePathname();
  console.log(currentPathname);

  return (
    <nav className="flex space-x-6 border-b mb-5 px-5 h-14 items-center">
      <Link href="/">
        <FaBug />
      </Link>
      <ul className="flex space-x-6">
        {links.map((link) => (
          <li key = {link.href}>
              <Link
                className={classNames({
                    'text-zinc-900' : link.href === currentPathname,
                    'text-zinc-400' : link.href !== currentPathname,
                    'text-zinc-500 hover:text-zinc-800 transition-colors' : true
                })}
                href={link.href}
              >
                {link.lable}
              </Link>
          </li>
        ))}
      </ul>
    </nav>
  );
};

export default NavBar;
