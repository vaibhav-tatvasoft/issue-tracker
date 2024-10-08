'use client'
import localFont from "next/font/local";
import "./globals.css";
import useQuiz from "./store";

const geistSans = localFont({
  src: "./fonts/GeistVF.woff",
  variable: "--font-geist-sans",
  weight: "100 900",
});
const geistMono = localFont({
  src: "./fonts/GeistMonoVF.woff",
  variable: "--font-geist-mono",
  weight: "100 900",
});

export default function RootLayout({ children, quiz }) {
  const config = useQuiz(state => state.config);
  let render = config.status ==='start'? quiz:children;
  return (
    <html lang="en">
      <body
        className={`${geistSans.variable} ${geistMono.variable} antialiased`}
      >
        {render}
      </body>
    </html>
  );
}
