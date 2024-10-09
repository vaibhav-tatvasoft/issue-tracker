"use client";
import React, { useEffect, useState } from "react";
import axios from "axios";
import useQuiz from "../store";
import _ from "lodash";
import Link from "next/link";
import { Player } from "@lottiefiles/react-lottie-player";

const Quiz = () => {
  const quizConfig = useQuiz((state) => state.config);
  const [questions, setQuestions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [count, setCount] = useState(0);
  const [score, setScore] = useState(0);
  const resetConfig = useQuiz((state) => state.resetConfig);
  const [selectedAnswer, setSelectedAnswer] = useState(null);
  const [isAnswered, setIsAnswered] = useState(false);

  async function getQuestions() {
    try {
      const response = await axios.get(
        `https://opentdb.com/api.php?amount=${quizConfig.numberOfQuestion}&category=${quizConfig.category.id}&difficulty=${quizConfig.level}&type=${quizConfig.type}`
      );
      console.log("FETCHED QUESTIONS: "+ JSON.stringify(response.data.results))
      const result = response.data.results;
      const shuffledAnswers = result.map((question) => {
        const answers = [
          ...question.incorrect_answers,
          question.correct_answer,
        ];
        return { ...question, shuffledAnswers: _.shuffle(answers) };
      });

      setQuestions([...shuffledAnswers]);
      setLoading(false);
    } catch (error) {
      console.error("Error fetching questions:", error);
      setLoading(false);
    }
  }

  function handleAnswerClick(item) {
    setSelectedAnswer(item);
    setIsAnswered(true);

    if (item === questions[count].correct_answer) {
      console.log("correct answer!!!!");
      setScore((prevScore) => prevScore + 1);
    } else {
      console.log("wrong answer!!!!");
    }
  }

  useEffect(() => {
    async function fetchData() {
      await getQuestions();
    }

    fetchData();
  }, []);

  if (loading) {
    return (
      <section className="min-h-screen flex justify-center items-center">
        <div className="text-center mb-4 text-4xl font-extrabold leading-none tracking-tight text-gray-900 md:text-5xl lg:text-6xl dark:text-white">
          Loading...
        </div>
      </section>
    );
  }

  if (count >= questions.length) {
    return (
      <section className="min-h-screen flex justify-center items-center">
        <div className="text-center mb-4 text-4xl font-extrabold leading-none tracking-tight text-gray-900 md:text-5xl lg:text-6xl dark:text-white">
          <Player
            src="https://lottie.host/404480ec-9589-47a4-b27b-9fec4685966c/HGfEMYP6k9.json"
            className="player"
            loop
            autoplay
            style={{ height: "300px", width: "300px" }}
          />
          Score: {score}
          <button
            onClick={() => resetConfig()}
            className="w-[50%] py-2.5 px-5 me-2 mb-2 text-sm font-large text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 hover:bg-blue-900 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-100 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700"
          >
            <Link href="/">Play Again</Link>
          </button>
        </div>
      </section>
    );
  }

  return (
    <section className="flex flex-col justify-center items-center p-4">
      <h1 className="mb-4 text-4xl font-extrabold leading-none tracking-tight text-gray-900 md:text-5xl lg:text-6xl dark:text-white">
        Question Number{" "}
        <span className="text-blue-600 dark:text-blue-500">#{count + 1}</span>
      </h1>
      <p className="text-2xl">Score: {score}</p>
      <section className="shadow-2xl my-10 p-10 w-[90%] rounded-lg flex flex-col justify-center shadow-blue-100 items-center">
        <h4 className="mb-4 text-2xl font-extrabold leading-none tracking-tight md:text-5xl lg:text-5xl text-blue-600 dark:text-blue-500">
          {questions[count].question}
        </h4>
        <div className="flex justify-evenly items-center my-20 flex-wrap w-[90%]">
          {questions[count].shuffledAnswers &&
            questions[count].shuffledAnswers.map((item) => (
              <button
                onClick={() => handleAnswerClick(item)}
                type="button"
                disabled={isAnswered}
                className={`py-5 w-[33%] px-5 me-2 mb-2 text-sm font-extrabold text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 ${
                  isAnswered
                    ? item === questions[count].correct_answer
                      ? "bg-green-500"
                      : selectedAnswer === item
                      ? "bg-red-500"
                      : "bg-white"
                    : "hover:bg-blue-600 hover:text-blue-700"
                } focus:z-10 focus:ring-4 focus:ring-gray-100 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700 shadow-blue-200 shadow-2xl`}
              >
                {item}
              </button>
            ))}
        </div>
        <button
          onClick={() => {
            setCount((prevCount) => prevCount + 1);
            setIsAnswered(false);
            setSelectedAnswer(null);
          }}
          type="button"
          className="py-5 w-[33%] px-5 me-2 mb-2 text-sm font-extrabold text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 hover:bg-blue-600 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-100 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700 shadow-blue-200 shadow-2xl"
          disabled={!isAnswered}
        >
          Next
        </button>
      </section>
    </section>
  );
};

export default Quiz;
