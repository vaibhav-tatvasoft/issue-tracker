import React, { useState } from 'react'
import axios from 'axios';
import{useFormik} from "formik";
import { useDispatch, useSelector } from 'react-redux';
import {setUser} from '../Slices/UserSlice'

const UserRegistration = () => {

  const dispatch = useDispatch();
  const {user} = useSelector(state => state.users)

    const formik = useFormik({
      initialValues: {
        email: "",
        userName: "" 
      },
      onSubmit: async (values) => {
          console.log("🚀 ~ UserRegistration ~ values:", values)
          try {
            const response = await axios.post('https://localhost:7219/api/User', values);
            console.log("🚀 ~ onSubmit: ~ response:", response.data)
            dispatch(setUser(response.data));
          } catch (error) {
            console.log("🚀 ~ onSubmit: ~ error:", error.response.data)
          }
      }
    })

    return (
        <div id="webcrumbs">
      <div className="w-full h-screen bg-gray-50 flex justify-center items-center">
        {" "}
        <div className="w-[600px] min-h-[600px] bg-white shadow-lg rounded-lg flex flex-col items-center p-10">
          <img
            src="https://tools-api.webcrumbs.org/image-placeholder/100/100/avatars/9"
            alt="Profile"
            className="w-[100px] h-[100px] rounded-full object-cover mb-6"
          />
          <h1 className="font-title text-3xl text-neutral-500 mb-8">
            Create an Account
          </h1>

          <form onSubmit={formik.handleSubmit} className="w-full flex flex-col gap-6">
            <div className="flex flex-col gap-2">
              <label htmlFor="userName" className="text-neutral-950">
                First Name
              </label>
              <input
                type="text"
                id="userName"
                name="userName"
                className="border border-neutral-300 p-3 rounded-md"
                placeholder="Enter your first name"
                onChange={formik.handleChange}
                value={formik.values.userName}
              />
            </div>

            <div className="flex flex-col gap-2">
              <label htmlFor="email" className="text-neutral-950">
                Email Address
              </label>
              <input
                type="email"
                id="email"
                name="email"
                className="border border-neutral-300 p-3 rounded-md"
                placeholder="Enter your email"
                onChange={formik.handleChange}
                value={formik.values.email}
              />
            </div>

            <div className="flex flex-col gap-2">
              <label htmlFor="image" className="text-neutral-950">
                Profile Image
              </label>
              <input
                type="file"
                id="image"
                name="image"
                className="border border-neutral-300 p-3 rounded-md"
              />
            </div>

            <button type="submit" className="bg-teal-500 text-primary-50 p-3 rounded-md mt-6">
              Sign Up
            </button>
          </form>

          <div className="mt-10 text-neutral-950">
            Already have an account?{" "}
            <a href="#" className="text-teal-500">
              Log in
            </a>
          </div>
        </div>
      </div>
    </div>
      );
}

export default UserRegistration;