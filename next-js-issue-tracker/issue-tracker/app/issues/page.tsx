import { Button, Table } from "@radix-ui/themes";
import axios from "axios";
import Link from "next/link";
import React, { useEffect, useState } from "react";

const IssuesPage = () => {
  const [issues, setIssue] = useState([]);
  const getAllIssues = async () => {
    const response = await axios.get("/api/issues");
    const data = response.data;
    setIssue(data);
    return data;
  };

  useEffect(() => {
    getAllIssues();
  }, []);

  return (
    <div className="my-3 space-y-3">
      <Button>
        <Link href="/issues/new">New Issue</Link>
      </Button>
      <Table.Root>
        <Table.Header>
          <Table.Row>
            <Table.ColumnHeaderCell>Full name</Table.ColumnHeaderCell>
            <Table.ColumnHeaderCell>Email</Table.ColumnHeaderCell>
            <Table.ColumnHeaderCell>Group</Table.ColumnHeaderCell>
          </Table.Row>
        </Table.Header>

        <Table.Body>
          {issues.map((issue) => (
            <Table.Row>
              <Table.RowHeaderCell>xyz</Table.RowHeaderCell>
              <Table.Cell>abc</Table.Cell>
            </Table.Row>
          ))}
        </Table.Body>
      </Table.Root>
    </div>
  );
};

export default IssuesPage;
