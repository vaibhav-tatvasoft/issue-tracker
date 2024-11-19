import { createSlice } from "@reduxjs/toolkit";

export const initialState = {
    clickedUser : {},
    prevClickedUser : {}
};

export const UserSelectedSlice = createSlice({
    initialState,
    name: 'UserSelected',
    reducers: {
        setClickedUser(state, action) {
            state.clickedUser = action.payload
        },
        setPrevClickedUser(state, action){
            state.prevClickedUser = action.payload
        }
    }
});

export const { setClickedUser, setPrevClickedUser } = UserSelectedSlice.actions;
export default UserSelectedSlice.reducer;