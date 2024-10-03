import React from "react";
import { useDispatch, useSelector } from "react-redux";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { fetchUsersThunk } from "../reducers/userSlice";
import axios from "axios";
import {fetchUsersSaga} from '../actions/userSaga'

export default function App() {
  const dispatch = useDispatch();

  const {
    data: reduxData = [],
    loading: reduxLoading,
    error: reduxError,
  } = useSelector((state) => state.users || {});

  const {
    data: sagaData = [],
    loading: sagaLoading,
    error: sagaError,
  } = useSelector((state) => state.usersSaga || {});

  const fetchUsersQuery = async () => {
    const { data } = await axios.get(
      "https://jsonplaceholder.typicode.com/users"
    );
    console.log(data);
    return data;
  };

  const {
    data: queryData,
    isLoading: queryLoading,
    isError: queryError,
    refetch,
  } = useQuery({
    queryKey: ["users"],
    queryFn: fetchUsersQuery,
    enabled: false,
  });

  const handleFetchRedux = () => {
    dispatch(fetchUsersThunk());
  };

  const handleFetchReduxSaga = () => {
    dispatch(fetchUsersSaga());
  };

  const handleFetchQuery = () => {
    refetch();
  };

  return (
    <div>
      <h1>Fetch Users</h1>

      <button onClick={handleFetchRedux}>Fetch with Redux Thunk</button>
      {reduxLoading && <p>Loading Redux Data...</p>}
      {reduxError && <p>Error: {reduxError}</p>}
      {reduxData.length > 0 ? (
        reduxData.map((user) => (
          <div key={user.id}>
            <h3>{user.name}</h3>
            <p>{user.email}</p>
          </div>
        ))
      ) : (
        <p>No data found.</p>
      )}

      <hr />
      <button onClick={handleFetchReduxSaga}>Fetch with Redux Saga</button>
      {sagaData.length > 0 ? (
        sagaData.map((user) => (
          <div key={user.id}>
            <h3>{user.name}</h3>
            <p>{user.email}</p>
          </div>
        ))
      ) : (
        <p>No data found (Redux Saga). Click the button to fetch users.</p>
      )}

      <hr />

      <button onClick={handleFetchQuery}>Fetch with React Query</button>
      {queryLoading && <p>Loading React Query Data...</p>}
      {queryError && <p>Error: {queryError.message}</p>}
      {queryData && queryData.length > 0 ? (
        queryData.map((user) => (
          <div key={user.id}>
            <h3>{user.name}</h3>
            <p>{user.email}</p>
          </div>
        ))
      ) : (
        <p>No data found.</p>
      )}
    </div>
  );
}
