"use client";
import { Button, TextArea, TextField } from "@radix-ui/themes";
import { useForm } from "react-hook-form";
import React from "react";
import axios from "axios";
import { useRouter } from "next/navigation";

interface IssueForm {
  title: String;
  description: String;
}

const NewIssuePage = () => {
  const { register, handleSubmit } = useForm<IssueForm>();
  const router = useRouter();
  return (
    <form
      onSubmit={handleSubmit((data) => {
        axios.post("/api/issues", data);
        router.push("/issues");
      })}
      className="max-w-xl space-y-5"
    >
      <TextField.Root placeholder="type something" {...register("title")} />
      <TextArea
        placeholder="write description..."
        {...register("description")}
      />
      <Button>Submit new issue</Button>
    </form>
  );
};

export default NewIssuePage;
