"use client";
import React, { useEffect, useState } from "react";
import axios from 'axios'
import useQuiz from "../store";

const Quiz = () => {
  const quizConfig = useQuiz((state) => state.config);
  console.log(quizConfig);
  const [questions, setQuestions] = useState([])
  const [answer, setAnswer] = useState('')
  const [loading, setLoading] = useState(false)
  const addScore = useQuiz(state => state.addScore)

  useEffect(() => {
    async function getQuestions(){
      const response = await axios.get(`https://opentdb.com/api.php?amount=${quizConfig.numberOfQuestion}&category=${quizConfig.category.id}&difficulty=${quizConfig.level}&type=${quizConfig.type}`)
      console.log(JSON.stringify(response.data.results))
    }
    getQuestions();
  })

  return (
    <section className="flex flex-col justify-center items-center p-4">
      <h1 className="mb-4 text-4xl font-extrabold leading-none tracking-tight text-gray-900 md:text-5xl lg:text-6xl dark:text-white">
        Question Number <span class="text-blue-600 dark:text-blue-500">#1</span>
      </h1>
      <p className="text-2xl">Score: 0</p>
      <section className="shadow-2xl my-10 p-10 w-[90%] rounded-lg flex flex-col justify-center shadow-blue-100 items-center">
        <h4 className="mb-4 text-2xl font-extrabold leading-none tracking-tight md:text-5xl lg:text-5xl text-blue-600 dark:text-blue-500">
          wha is my name?
        </h4>
        <div className="flex justify-evenly items-center my-20 flex-wrap w-[90%]">
          <button
            type="button"
            className="w-[33%] py-2.5 px-5 me-2 mb-2 text-sm font-medium text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 hover:bg-blue-600 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-100 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700 shadow-blue-200 shadow-2xl"
          >
            Alternative
          </button>
          <button
            type="button"
            className="w-[33%] py-2.5 px-5 me-2 mb-2 text-sm font-medium text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 hover:bg-blue-600 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-100 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700 shadow-blue-200 shadow-2xl"
          >
            Alternative
          </button>
          <button
            type="button"
            className="w-[33%] py-2.5 px-5 me-2 mb-2 text-sm font-medium text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 hover:bg-blue-600 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-100 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700 shadow-blue-200 shadow-2xl"
          >
            Alternative
          </button>
          <button
            type="button"
            className="w-[33%] py-2.5 px-5 me-2 mb-2 text-sm font-medium text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 hover:bg-blue-600 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-100 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700 shadow-blue-200 shadow-2xl"
          >
            Alternative
          </button>
        </div>
        <button
          type="button"
          className="w-[33%] py-2.5 px-5 me-2 mb-2 text-sm font-medium text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 hover:bg-blue-900 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-100 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700"
        >
          Next
        </button>
      </section>
    </section>
  );
};

export default Quiz;
