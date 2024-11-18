import { createSlice } from "@reduxjs/toolkit";

export const initialState = {
    clickedUser : {}
};

export const UserSelectedSlice = createSlice({
    initialState,
    name: 'UserSelected',
    reducers: {
        setClickedUser(state, action) {
            state.clickedUser = action.payload
        }
    }
});

export const { setClickedUser } = UserSelectedSlice.actions;
export default UserSelectedSlice.reducer;