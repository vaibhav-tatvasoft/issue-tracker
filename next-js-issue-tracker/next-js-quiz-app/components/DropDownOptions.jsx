'use client'
import React, { useEffect, useState } from "react";
import useQuiz from "../../next-js-quiz-app/app/store/index";
import { Check, ChevronDown, Circle, UserSquareIcon } from "lucide-react"
import axios from 'axios'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "../components/ui/dropdown-menu";

const DropDownOptions = () => {
  const Type = ["boolean", "multiple"];
  const Level = ["easy", "medium", "hard"]
  const [categories, setCategories] = useState([]);
  const addCategory = useQuiz(state=>state.addCategory)
  const addLevel = useQuiz(state => state.addLevel)
  const addType = useQuiz(state => state.addType)
  const quizConfig = useQuiz(state=>state.config)

  useEffect(() => {
    async function getCategories() {
      const response = await axios.get("https://opentdb.com/api_category.php");
      // console.log("getCategoriesData "+JSON.stringify(response.data.trivia_categories,null,2))
      setCategories([...response.data.trivia_categories]);
    }
    getCategories();
  }, []);

  return (
    <section className="flex justify-evenly items-center py-5">
      <div className="px-7 py-4 w-1/3 mx-4">
        <DropdownMenu>
          <DropdownMenuTrigger className="flex outline-none justify-between w-full px-10 py-3 rounded-lg shadow-lg hover:bg-blue-600 hover:text-gray-100">
          {quizConfig.category.name? quizConfig.category.name : quizConfig.category.name = 'CATEGORY'} <ChevronDown />
          </DropdownMenuTrigger>
          <DropdownMenuContent>
            <DropdownMenuLabel>SELECT CATEGORY</DropdownMenuLabel>
            <DropdownMenuSeparator />
            {categories.map((category) => (
              <DropdownMenuItem key={category.id} onClick={() =>addCategory(category.id, category.name)}>{category.name}</DropdownMenuItem>
            ))}
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
      <div className="px-7 py-4 w-1/3 mx-4">
        <DropdownMenu>
          <DropdownMenuTrigger className="flex outline-none justify-between w-full px-10 py-3 rounded-lg shadow-lg hover:bg-blue-600 hover:text-gray-100">
            {quizConfig.level? quizConfig.level : quizConfig.level  = 'LEVEL'} <ChevronDown />
          </DropdownMenuTrigger>
          <DropdownMenuContent>
            <DropdownMenuLabel>SELECT LEVEL</DropdownMenuLabel>
            <DropdownMenuSeparator />
            {Level.map((value, index) => (
              <DropdownMenuItem key={index} onClick ={() => addLevel(value)}>{value}</DropdownMenuItem>
            ))}
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
      <div className="px-7 py-4 w-1/3 mx-4">
        <DropdownMenu>
          <DropdownMenuTrigger className="flex outline-none justify-between w-full px-10 py-3 rounded-lg shadow-lg hover:bg-blue-600 hover:text-gray-100">
          {quizConfig.type? quizConfig.type : quizConfig.type = 'TYPE'} <ChevronDown />
          </DropdownMenuTrigger>
          <DropdownMenuContent>
            <DropdownMenuLabel>SELECT TYPE</DropdownMenuLabel>
            <DropdownMenuSeparator />
            {Type.map((value, index) => (
              <DropdownMenuItem key={index} onClick ={() => addType(value)}>{value}</DropdownMenuItem>
            ))}
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
    </section>
  );
};

export default DropDownOptions;
