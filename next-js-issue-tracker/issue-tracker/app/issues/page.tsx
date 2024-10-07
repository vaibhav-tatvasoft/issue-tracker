'use client'
import { Button, Table } from "@radix-ui/themes";
import axios from "axios";
import Link from "next/link";
import { useDispatch, useSelector } from 'react-redux';
import React, { useEffect, useState } from "react";
import { Issue } from "@/types/IssueType";
import { RootState } from "@/store";
import { setIssues } from "@/actions/IssuesActions";

const IssuesPage = () => {
  const dispatch = useDispatch();
  const issuesData: Issue[] = useSelector((state: RootState) => state.issues.issuesList);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  
  const [editingIndex, setEditingIndex] = useState<number | null>(null);
  const [status, setStatus] = useState<string>("");

  const getIssues = async () => {
    try {
      const response = await axios.get("http://localhost:3000/api/issues");
      dispatch(setIssues(response.data));
    } catch (err) {
      setError("Failed to fetch issues. Please try again later.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    getIssues();
  }, [dispatch]);

  const handleEditClick = (index: number, currentStatus: string) => {
    setEditingIndex(index);
    setStatus(currentStatus);
  };

  const handleSaveClick = async (index: number) => {
    try {
      await axios.put(`http://localhost:3000/api/issues/${issuesData[index].id}`, {
        status,
      });
      dispatch(setIssues([...issuesData.slice(0, index), { ...issuesData[index], status }, ...issuesData.slice(index + 1)]));
      setEditingIndex(null);
    } catch (error) {
      console.error("Error saving status", error);
      setError("Failed to save status.");
    }
  };

  return (
    <div className="my-3 space-y-3">
      <Button>
        <Link href="/issues/new">New Issue</Link>
      </Button>
      {loading && <p>Loading...</p>}
      {error && <p className="text-red-500">{error}</p>}
      <Table.Root>
        <Table.Header>
          <Table.Row>
            <Table.ColumnHeaderCell>Title</Table.ColumnHeaderCell>
            <Table.ColumnHeaderCell>Description</Table.ColumnHeaderCell>
            <Table.ColumnHeaderCell>
              Status
            </Table.ColumnHeaderCell>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          {issuesData.map((issue, index) => (
            <Table.Row key={index}>
              <Table.RowHeaderCell>{issue.title}</Table.RowHeaderCell>
              <Table.Cell>{issue.description}</Table.Cell>
              <Table.Cell>
                {editingIndex === index ? (
                  <>
                    <select
                      value={status}
                      onChange={(e) => setStatus(e.target.value)}
                    >
                        <option value="Open">Open</option>
                      <option value="In Progress">In Progress</option>
                      <option value="Resolved">Resolved</option>
                      <option value="Closed">Closed</option>
                    </select>
                    <Button onClick={() => handleSaveClick(index)}>Save</Button>
                  </>
                ) : (
                  <>
                    {issue.status}
                    <Button onClick={() => handleEditClick(index, issue.status)}>Edit</Button>
                  </>
                )}
              </Table.Cell>
            </Table.Row>
          ))}
        </Table.Body>
      </Table.Root>
    </div>
  );
};

export default IssuesPage;
