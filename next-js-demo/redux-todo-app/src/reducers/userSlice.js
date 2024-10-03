import { createSlice } from "@reduxjs/toolkit";
import axios from "axios";

export const fetchUsersThunk = () => async (dispatch) => {
  try {
    dispatch(fetchUsersStart());
    const { data } = await axios.get( 
      "https://jsonplaceholder.typicode.com/users"
    );
    console.log("Fetched Users (Thunk):", data);
    dispatch(fetchUsersSuccess(data));
  } catch (error) {
    dispatch(fetchUsersFailure(error.message));
  }
};

const usersSlice = createSlice({
    name: 'users',
    initialState: {
      data: [],
      loading: false,
      error: null,
    },
    reducers: {
      fetchUsersStart: (state) => {
        state.loading = true;
        state.error = null;
      },
      fetchUsersSuccess: (state, action) => {
        state.loading = false;
        state.data = action.payload;
      },
      fetchUsersFailure: (state, action) => {
        state.loading = false;
        state.error = action.payload;
      },
    },
  });

export const { fetchUsersStart, fetchUsersSuccess, fetchUsersFailure } = usersSlice.actions;

export default usersSlice.reducer;
